using System.Collections.Generic;
using System.Threading.Tasks;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Tasks;
using ServerFramework;
using UnityEngine;

namespace Core
{
    public delegate void FailureResponseHandle(string log);
    public class Peer
    {
        private Queue<Packet> _packets;
        private Queue<ResultSimpleTask> _responses;
        private IClient<Packet> _client;
        private bool _isConnected;
        public event FailureResponseHandle FailureResponse;

        public Peer(IClient<Packet> client)
        {
            _packets = new Queue<Packet>();
            _responses = new Queue<ResultSimpleTask>();

            _client = client;
        }

        public void ChangeConnection(bool value)
        {
            _isConnected = value;
            if (!value)
            {
                _packets.Clear();
                _responses.Clear();
            }
        }

        public void ResponseHandle(ResponsePacket responsePacket)   
        {
            var task = _responses.Dequeue();
            task.Complete( new TaskResult(responsePacket.IsSuccess, responsePacket.Log));
            ForceSendPacket();
        }

        private void ForceSendPacket()
        {
            if (_packets.TryDequeue(out var packetItem))
            {
                _client.SendPacket(packetItem);
            }
        }
        public async Task<TaskResult> SendPacket(Packet packet, bool withoutResponse = false)
        {
            if (!_isConnected)
            {
                FailureResponse?.Invoke("Not Connection to server");
                return new TaskResult(false, "not connected");
            }
            if (withoutResponse)
            {
                SendPackerWithoutResponse(packet);
                return true;
            }
            
            var responseTask = new ResultSimpleTask();
            if (_responses.Count == 0)
            {
                _responses.Enqueue(responseTask);
                _client.SendPacket(packet);
            }
            else
            {
                _responses.Enqueue(responseTask);
                _packets.Enqueue(packet);
            }

            var res = await responseTask.Wait();
            if(!res) FailureResponse?.Invoke(res.Log);
            return res;
        }

        private void SendPackerWithoutResponse(Packet packet)
        {
            _client.SendPacket(packet);
        }
        public async void SendResponse(bool isComplete, string log = "")
        {
            await SendPacket(new ResponsePacket(isComplete, log), true);
        }
    }
}