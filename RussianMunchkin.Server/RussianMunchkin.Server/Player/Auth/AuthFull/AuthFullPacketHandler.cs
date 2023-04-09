using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Packets.Auth;
using RussianMunchkin.Server.Core.PacketsHandler.Decorator;
using RussianMunchkin.Server.DataBase;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;
using RussianMunchkin.Server.Player.Auth.AuthFull;

namespace RussianMunchkin.Server.Handlers;

public class AuthFullPacketHandler: PacketHandlerDecorator<ServerPlayerToController>
{
    private readonly DataBaseController _dataBaseController;
    private Authorization _authorization;
    private Registration _registration;
    private AuthController _authController;

    public AuthFullPacketHandler(IPacketsHandler<ServerPlayerToController> previewHandler, DataBaseController dataBaseController, AuthController authController) : base(previewHandler)
    {
        _dataBaseController = dataBaseController;
        _authController = authController;
        _authorization = new Authorization(_dataBaseController, _authController);
        _registration = new Registration(_dataBaseController, _authController);
    }

    protected override bool TryHandle(ServerPlayerToController player, Packet packet)
    {
        if (_authorization.TryHandle(player, packet)) return true;
        if(_registration.TryHandle(player, packet)) return true;
        return false;
    }
}