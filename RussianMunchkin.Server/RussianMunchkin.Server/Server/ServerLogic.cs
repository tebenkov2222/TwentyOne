using System;
using Prometheus;
using Repository;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Configuration;
using RussianMunchkin.Server.Core.PacketsHandler.Decorator;
using RussianMunchkin.Server.Core.Player;
using RussianMunchkin.Server.DataBase;
using RussianMunchkin.Server.Game.TwentyOne.Handlers;
using RussianMunchkin.Server.Game.TwentyOne.Player;
using RussianMunchkin.Server.Handlers;
using RussianMunchkin.Server.Mapper;
using RussianMunchkin.Server.MatchMaking;
using RussianMunchkin.Server.Player;
using RussianMunchkin.Server.Player.Auth.AuthFull;
using ServerPlayer = RussianMunchkin.Server.Game.TwentyOne.Player.Server.ServerPlayerToController;
namespace RussianMunchkin.Server.Server;

public class ServerLogic
{
    private readonly ConfigurationController _configurationController;
    
    private IPacketsHandler<ServerPlayer> _packetsHandler;
    private RoomsManager _roomsManager;
    private AuthController _authController;
    private Gauge  _countClientsGauge;
    
    private readonly DataBaseController _dataBase;

    public ServerLogic(ConfigurationController configurationController)
    {
        _configurationController = configurationController;
        var mapperInstance = new MapperInstance();
        
        _countClientsGauge = Prometheus.Metrics.CreateGauge("count_clients", "");

        _dataBase = new DataBaseController(new PostreSQL(_configurationController.ConfigurationDatabase));
        
        _authController = new AuthController();

        _roomsManager = new RoomsManager();
        InitHandlersPackets();
    }

    private void InitHandlersPackets()
    {
        var packetHandler = new PacketHandler<ServerPlayer>();
        
        var joinPacketHandler = new AuthFullPacketHandler(packetHandler, _dataBase, _authController);
        var responsePacketHandler = new ResponsePacketHandler(joinPacketHandler);
        var roomPacketHandler =  new RoomPacketHandler(responsePacketHandler, _roomsManager);
        var gamePacketHandler = new GamePacketHandler(roomPacketHandler);
        _packetsHandler = gamePacketHandler;
    }
    
    public void ClientConnected(ServerPlayer serverPlayer)
    {
        _countClientsGauge.Inc();
        Console.WriteLine($"Connect player {serverPlayer.Peer.ClientId}");
    }

    public void ClientDisconnected(ServerPlayer serverPlayer)
    {
        Console.WriteLine($"Disconnect player {serverPlayer.Peer.ClientId}, login ");

        if (serverPlayer.AuthController.AuthModel.IsAuthorized)
        {
            _authController.LogOut(serverPlayer);
            _roomsManager.TryRemovePlayer(serverPlayer);
        }
        _countClientsGauge.Dec();

    }

    public void HandlePacket(ServerPlayer serverPlayer, Packet packet)
    {
        _packetsHandler.Handle(serverPlayer, packet);
    }
}