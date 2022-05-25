using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WebSocketCliet.Models;

namespace WebSocketCliet.ViewModels
{
    public class WSClientViewModel : IDisposable
    {
        private WSClient innerModel;

        public ReactiveProperty<bool> isConnected { get; }
        public ReadOnlyReactiveProperty<string> connectedState { get; }
        public ReadOnlyReactiveProperty<SolidColorBrush> connectedStateColor { get; }
        public ReactiveProperty<string> sendMessage { get; }
        public ReactiveProperty<string> receiveMessage { get; }
        public ReactiveCommand reconnectCommand { get; }
        public ReactiveCommand sendCommand { get; }

        public WSClientViewModel(WSClient innerModel)
        {
            this.innerModel = innerModel;

            isConnected = innerModel.ToReactivePropertyAsSynchronized(x => x.isConnected);
            connectedState = isConnected.Select(x => x == true ? "connected" : "disconnected").ToReadOnlyReactiveProperty();
            connectedStateColor = isConnected.Select(x => x == true ? Brushes.LightGreen : Brushes.Red).ToReadOnlyReactiveProperty();
            sendMessage = innerModel.ToReactivePropertyAsSynchronized(x => x.sendMessage);
            receiveMessage = innerModel.ToReactivePropertyAsSynchronized(x => x.receiveMessage);

            reconnectCommand = new ReactiveCommand();
            reconnectCommand.Subscribe(async () => await innerModel.Reconnect());

            sendCommand = new ReactiveCommand();
            sendCommand.Subscribe(async () => await innerModel.SendAsync());
        }

        public void Dispose()
        {
            isConnected.Dispose();
            connectedState.Dispose();
            connectedStateColor.Dispose();
            sendMessage.Dispose();
            receiveMessage.Dispose();

            reconnectCommand.Dispose();
            sendCommand.Dispose();
        }
    }
}
