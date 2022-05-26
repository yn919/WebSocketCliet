using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketCliet.Models
{
    public class WSClient : NotifyPropertyChanged
    {
        private ClientWebSocket ws;
        private int bufferSize = 1024;

        private string _ipAddress = "localhost";
        public string ipAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
                RaisePropertyChange();
            }
        }

        private int _port = 51999;
        public int port
        {
            get { return _port; }
            set
            {
                _port = value;
                RaisePropertyChange();
            }
        }

        private bool _isConnected = false;
        public bool isConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                RaisePropertyChange();
            }
        }

        private string _sendMessage = string.Empty;
        public string sendMessage
        {
            get { return _sendMessage; }
            set
            {
                _sendMessage = value;
                RaisePropertyChange();
            }
        }

        private string _receiveMessage = string.Empty;
        public string receiveMessage
        {
            get { return _receiveMessage; }
            set
            {
                _receiveMessage = value;
                RaisePropertyChange();
            }
        }

        public WSClient()
        {
        }

        public async Task ConnectAsync()
        {
            ws = new ClientWebSocket();
            string changeIPAddress = ipAddress == "127.0.0.1" ? "localhost" : ipAddress;
            Uri uri = new Uri($"ws://{changeIPAddress}:{port}/ws/");

            try
            {
                await ws.ConnectAsync(uri, CancellationToken.None);
                isConnected = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if(ws != null && isConnected == true)
            {
                ws.Dispose();
                ws = null;

                isConnected = false;
            }
        }

        public async Task Reconnect()
        {
            Disconnect();
            await ConnectAsync();
            await StartReceiveAsync();
        }
        public async Task SendAsync()
        {
            if (isConnected == false) return;

            byte[] sendBuffer = new byte[bufferSize];
            byte[] stringBuffer = Encoding.UTF8.GetBytes(sendMessage);

            if (bufferSize < stringBuffer.Length) return;

            Array.Copy(stringBuffer, 0, sendBuffer, 0, stringBuffer.Length);

            ArraySegment<byte> segment = new ArraySegment<byte>(sendBuffer);

            await ws.SendAsync(segment, WebSocketMessageType.Text, false, CancellationToken.None);
        }

        public async Task StartReceiveAsync()
        {
            if (isConnected == false) return;

            byte[] buffer = new byte[bufferSize];

            while (true)
            {
                try
                {
                    ArraySegment<byte> segment = new ArraySegment<byte>(buffer);

                    WebSocketReceiveResult result = await ws.ReceiveAsync(segment, CancellationToken.None);

                    receiveMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    Disconnect();
                    break;
                }
            }
        }
    }
}
