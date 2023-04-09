using System.Collections.Generic;

namespace RussianMunchkin.Server.Player.Auth.AuthFull;

public class AuthModel
{
    public HashSet<string> AuthorizedClients = new HashSet<string>();
}