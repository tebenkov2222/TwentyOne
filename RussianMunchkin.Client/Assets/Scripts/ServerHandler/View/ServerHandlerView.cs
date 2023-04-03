using System;
using UnityEngine;

namespace ServerHandler.View
{
    public class ServerHandlerView: MonoBehaviour, IServerHandlerView
    {
        public event Action Disconnected;
        public event Action<string> FailedResponse;
        public void ChangeConnection(bool isConnected)
        {
            if(!isConnected)Disconnected?.Invoke();
        }

        public void Failed(string log)
        {
            FailedResponse?.Invoke(log);
            Debug.LogError($"Response is Failure with |{log}|");
        }

    }
}