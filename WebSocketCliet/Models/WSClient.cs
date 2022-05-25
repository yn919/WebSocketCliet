﻿using System;
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

        private string _ipAddress = "127.0.0.1";
        public string ipAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
                RaisePropertyChange();
            }
        }

        private int _port = 19900;
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
            Uri uri = new Uri($"ws://{ipAddress}:{port}/ws/");

            try
            {
                await ws.ConnectAsync(uri, CancellationToken.None);
                isConnected = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public async Task DisconnectAsync()
        {
            if(ws != null && isConnected == true)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "close async", CancellationToken.None);
                await ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "close output async", CancellationToken.None);
                
                ws.Dispose();
                ws = null;

                isConnected = false;
            }
        }

        public async Task Reconnect()
        {
            await DisconnectAsync();
            await ConnectAsync();
            await StartReceiveAsync();
        }
        public async Task SendAsync()
        {
            if (isConnected == false) return;

            byte[] sendBuffer = Encoding.UTF8.GetBytes(sendMessage);
            ArraySegment<byte> segment = new ArraySegment<byte>(sendBuffer);

            await ws.SendAsync(segment, WebSocketMessageType.Text, false, CancellationToken.None);
        }

        public async Task StartReceiveAsync()
        {
            if (isConnected == false) return;

            byte[] buffer = new byte[bufferSize];

            while(true)
            {
                ArraySegment<byte> segment = new ArraySegment<byte>(buffer);

                WebSocketReceiveResult result = await ws.ReceiveAsync(segment, CancellationToken.None);

                int count = result.Count;
                while (!result.EndOfMessage)
                {
                    if (count >= buffer.Length)
                    {
                        await DisconnectAsync();
                        return;
                    }
                    segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                    result = await ws.ReceiveAsync(segment, CancellationToken.None);

                    count += result.Count;
                }

                receiveMessage = Encoding.UTF8.GetString(buffer, 0, count);
            }
        }
    }
}
