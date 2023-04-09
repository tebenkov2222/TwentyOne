using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Tasks;
using ServerFramework;

namespace RussianMunchkin.Server.Server
{
    public delegate void FailureResponseHandle(string log);
    public delegate void ChangeConnectionHandler(bool isConnected);
    public class Peer
    {
        private Queue<Packet> _packets;
        private Queue<ResultSimpleTask> _responses;
        private IServer<Packet> _server;
        private readonly int _clientId;

        public int ClientId => _clientId;
        private bool _isConnected;
        public event ChangeConnectionHandler ConnectionChanged;
        public event FailureResponseHandle FailureResponse;

        public Peer(IServer<Packet> server, int clientId)
        {
            _packets = new Queue<Packet>();
            _responses = new Queue<ResultSimpleTask>();

            _server = server;
            _clientId = clientId;
        }

        public void ChangeConnection(bool value)
        {
            _isConnected = value;
            ConnectionChanged?.Invoke(value);
        }
        public void ResponseHandle(ResponsePacket responsePacket)   
        {
            var task = _responses.Dequeue();
            Console.WriteLine($"PEER: ResponseHandle");

            task.Complete(new TaskResult(responsePacket.IsSuccess, responsePacket.Log));
            ForceSendPacket(_clientId);
        }

        private void ForceSendPacket(int id)
        {
            Console.WriteLine($"PEER: ForceSendPacket, packetsCount = {_packets.Count}, responseCount = {_responses.Count}");

            if (_packets.TryDequeue(out var packetItem))
            {
                Console.WriteLine($"PEER: TryDequeue Send |{packetItem.GetType().ToString().Split('.').Last()}|");

                _server.SendPacket(id, packetItem);
            }
        }
        public async Task<TaskResult> SendPacket(Packet packet, bool withoutResponse = false)
        {
            Console.WriteLine($"PEER: SendPacket |{packet.GetType().ToString().Split('.').Last()}|. withoutResponse = {withoutResponse}, responses = {_responses.Count}");

            if (!_isConnected)
            {
                return new TaskResult(false, "not connected");
            }
            if (withoutResponse)
            {
                SendPacketWithoutResponse(packet);
                return true;
            }

            var responseTask = new ResultSimpleTask();
            Console.WriteLine($"PEER: responses = {_responses.Count}");
            if (_responses.Count == 0)
            {
                _responses.Enqueue(responseTask);
                Console.WriteLine($"PEER: SendPacket {packet.GetType().ToString().Split('.').Last()}");
                _server.SendPacket(_clientId, packet);
            }
            else
            {
                Console.WriteLine($"PEER: Enqueue {packet.GetType().ToString().Split('.').Last()}");
                _responses.Enqueue(responseTask);
                _packets.Enqueue(packet);
            }

            var res = await responseTask.Wait();

            if(!res) FailureResponse?.Invoke(res.Log);
            return res;
        }

        private void SendPacketWithoutResponse(Packet packet)
        {
            _server.SendPacket(_clientId, packet);

        }
        public async void SendResponse(bool isComplete, string log = "")
        {
            Console.WriteLine($"PEER: Send response with {isComplete} {log}");
            await SendPacket(new ResponsePacket(isComplete, log), true);
        }
    }
}