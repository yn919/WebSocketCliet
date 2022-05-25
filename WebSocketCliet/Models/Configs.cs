using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketCliet.Models
{
    public class Configs : NotifyPropertyChanged
    {
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
    }
}
