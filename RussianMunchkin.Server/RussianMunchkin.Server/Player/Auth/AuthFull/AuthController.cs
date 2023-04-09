using System;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;
using RussianMunchkin.Server.Mapper;

namespace RussianMunchkin.Server.Player.Auth.AuthFull;

public class AuthController
{
    private AuthModel _model;

    public AuthController()
    {
        _model = new AuthModel();
    }

    public bool GetAccessLogIn(string login)
    {
        return !_model.AuthorizedClients.Contains(login);
    }
    public void LogIn(ServerPlayerToController player, PlayerAuthFullModel authFullModel)
    {
        _model.AuthorizedClients.Add(authFullModel.Login);
        player.AuthController.LogIn(authFullModel);
        player.LogIn(authFullModel.Login);
    }

    public void LogOut(ServerPlayerToController player)
    {
        _model.AuthorizedClients.Remove((string)player);
        player.AuthController.LogOut();
    }
}