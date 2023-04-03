using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Tasks;
using ServerFramework;

namespace RussianMunchkin.Server.Server
{
    public delegate void FailureResponseHandle(string log);
    public delegate void ChangeConnectionHandler(bool isConnected);
    public class NetPeer
    {
        private Queue<(Packet packet, bool withoutResponse)> _packets;
        private Queue<ResultSimpleTask> _responses;
        private IServer<Packet> _server;
        private readonly int _idUser;
        private bool _isConnected;
        public event ChangeConnectionHandler ConnectionChanged;
        public event FailureResponseHandle FailureResponse;

        public NetPeer(IServer<Packet> server, int idUser)
        {
            _packets = new Queue<(Packet, bool)>();
            _responses = new Queue<ResultSimpleTask>();

            _server = server;
            _idUser = idUser;
        }

        public void ChangeConnection(bool value)
        {
            _isConnected = value;
            ConnectionChanged?.Invoke(value);
        }
        public void ResponseHandle(ResponsePacket responsePacket)   
        {
            var task = _responses.Dequeue();
            task.Complete(new TaskResult(responsePacket.IsSuccess, responsePacket.Log));
            ForceSendPacket(_idUser);
        }

        private void ForceSendPacket(int id)
        {
            if (_packets.TryDequeue(out var packetItem))
            {
                _server.SendPacket(id, packetItem.packet);
                if (packetItem.withoutResponse) ForceSendPacket(id);
            }
        }
        public async Task<TaskResult> SendPacket(Packet packet, bool withoutResponse = false)
        {
            if (!_isConnected) return new TaskResult(false, "not connected");
            var responseTask = new ResultSimpleTask();
            if(!withoutResponse) _responses.Enqueue(responseTask);
            if (_packets.Count == 0)
            {
                _server.SendPacket(_idUser, packet);
            }
            else
            {
                _packets.Enqueue((packet, withoutResponse));
            }
            if (withoutResponse) return true;

            var res = await responseTask.Wait();

            if(!res) FailureResponse?.Invoke(res.Log);
            return res;
        }

        public async Task<bool> TestTask()
        {
            await Task.Delay(5000);
            return false;
        }
        public async void SendResponse(bool isComplete, string log = "")
        {
            Console.WriteLine($"Send response with {isComplete} {log}");
            await SendPacket(new ResponsePacket(isComplete, log), true);
        }
    }
}