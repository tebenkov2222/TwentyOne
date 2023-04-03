using System;
using RussianMunchkin.Common.Tasks;

namespace Auth.Username.View
{
    public delegate TaskResult JoinToServer(string username);
    public interface IAuthUsernameView
    {
        public event Action<string> JoinToServer;
        public void SuccessJoin();
    }
}