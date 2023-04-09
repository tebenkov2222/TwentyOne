using System.IO;
using Auth.Full.Model;
using AutoMapper;
using Core;
using Core.PacketHandlers;
using PacketHandlers.Core;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Packets.Auth;
using UnityEditor;
using UnityEngine;

namespace Auth.Full
{
    public class AuthFullPacketHandler: PacketHandlerDecorator
    {
        private readonly AuthFullController _authFullController;
        private readonly Mapper _mapper;

        public AuthFullPacketHandler(IPacketsHandler previewHandler, Peer peer, AuthFullController authFullController, Mapper mapper) : base(previewHandler, peer)
        {
            _authFullController = authFullController;
            _mapper = mapper;
        }

        protected override bool TryHandle(Packet packet)
        {
            switch (packet)
            {
                case AuthorizationResultPacket authorizationResultPacket:
                    string path = Application.persistentDataPath + "/token.cfg";
                    //File.WriteAllText(path, authorizationResultPacket.Token); todo: implement
                    File.WriteAllText(path, "12345678");
                    _authFullController.Authorize(_mapper.Map<AuthorizationUserModel>(authorizationResultPacket));
                    _peer.SendResponse(true);
                    break;
                default: return false;
            }
            return true;
        }
    }
}