using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketCliet.Models;

namespace WebSocketCliet.ViewModels
{
    public class WSClientViewModel : IDisposable
    {
        private WSClient innerModel;

        public ReactiveProperty<bool> isConnected { get; }
        public ReactiveProperty<string> sendMessage { get; }
        public ReactiveProperty<string> receiveMessage { get; }

        public WSClientViewModel(WSClient innerModel)
        {
            this.innerModel = innerModel;

            isConnected = innerModel.ToReactivePropertyAsSynchronized(x => x.isConnected);
            sendMessage = innerModel.ToReactivePropertyAsSynchronized(x => x.sendMessage);
            receiveMessage = innerModel.ToReactivePropertyAsSynchronized(x => x.receiveMessage);
        }

        public void Dispose()
        {
            isConnected.Dispose();
            sendMessage.Dispose();
            receiveMessage.Dispose();
        }
    }
}
