using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketCliet.Models
{
    class MainModel
    {
        public Configs configs { get; };
        public WSClient wsClient { get; };

        public MainModel()
        {
            configs = new Configs();
            wsClient = new WSClient(configs);
        }

        public void Start()
        {

        }

        public void End()
        {

        }
    }
}
