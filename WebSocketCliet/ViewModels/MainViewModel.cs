using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketCliet.Models;

namespace WebSocketCliet.ViewModels
{
    public class MainViewModel : IDisposable
    {
        private MainModel innerModel;

        public ConfigsViewModel configsViewModel { get; set; }
        public WSClientViewModel wsClientViewModel { get; set; }

        public MainViewModel()
        {
            innerModel = new MainModel();

            configsViewModel = new ConfigsViewModel(innerModel.configs);
            wsClientViewModel = new WSClientViewModel(innerModel.wsClient);
        }

        public async void Start()
        {
            await innerModel.Start();
        }

        public async void End()
        {
            await innerModel.End();
        }

        public void Dispose()
        {
            configsViewModel.Dispose();
            wsClientViewModel.Dispose();
        }
    }
}
