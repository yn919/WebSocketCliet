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
    public class ConfigsViewModel : IDisposable
    {
        private Configs innerModel;

        public ReactiveProperty<string> ipAddress { get; }
        public ReactiveProperty<int> port { get; }

        public ConfigsViewModel(Configs innerModel)
        {
            this.innerModel = innerModel;

            ipAddress = innerModel.ToReactivePropertyAsSynchronized(x => x.ipAddress);
            port = innerModel.ToReactivePropertyAsSynchronized(x => x.port);
        }

        public void Dispose()
        {
            ipAddress.Dispose();
            port.Dispose();
        }
    }
}
