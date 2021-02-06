using NetStatDashboard.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetStatDashboard.Models
{
    public sealed class Host: NotifiyPropertyChanged
    {
        int _Id = 0;
        string _name = null;
        string _hostName = null;
        bool _pingable = false;
        byte _delayPing = 5;
        DateTime? lastStatusdate = null;
        string _color = "Red";
        string _lastUpdate = null;
        short _notifyTime = 15;
        bool _isStart = false;
        bool _isstop = true;
        string _stText = "";
        string _errorStatusCodes = "";
        string _lastStatusCode = "";
        bool _byPing = false;
        public Host()
        {
            if (!IsStart)
            {
                Color = "Gray";
                STText = "Start";
            }

        }

        public int Id
        {
            get { return _Id; }
            set
            {
                SetProperty(ref _Id, value);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public string HostName
        {
            get { return _hostName; }
            set
            {
                SetProperty(ref _hostName, value);
            }
        }

        public bool Pingable
        {
            get { return _pingable; }
            set
            {
                if (value)
                    Color = "Green";
                else
                    Color = "Orangered";

                SetProperty(ref _pingable, value);
            }
        }

        public byte DelayPing
        {
            get { return _delayPing; }
            set
            {

                SetProperty(ref _delayPing, value);
            }
        }

        public DateTime? LastStatusDate
        {
            get { return lastStatusdate; }
            set
            {
                SetProperty(ref lastStatusdate, value);
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                SetProperty(ref _color, value);
            }
        }

        public string LastUpdate
        {
            get { return _lastUpdate; }
            set
            {
                SetProperty(ref _lastUpdate, value);
            }
        }
        public short NotifyTime
        {
            get { return _notifyTime; }
            set
            {
                SetProperty(ref _notifyTime, value);
            }
        }
        public bool IsStart
        {
            get { return _isStart; }
            set
            {
                if (value)
                {
                    STText = "Stop";
                }
                SetProperty(ref _isStart, value);
            }
        }
        public bool IsStop
        {
            get { return _isstop; }
            set
            {
                if (value)
                {
                    Color = "Gray";
                    STText = "Start";
                }

                IsStart = !value;
                SetProperty(ref _isstop, value);
            }
        }
        public string STText
        {
            get { return _stText; }
            set
            {
                SetProperty(ref _stText, value);
            }
        }
        public string ErrorStatusCodes
        {
            get { return _errorStatusCodes; }
            set
            {
                SetProperty(ref _errorStatusCodes, value);
            }
        }        
        public string LastStatusCode
        {
            get { return _lastStatusCode; }
            set
            {
                SetProperty(ref _lastStatusCode, value);
            }
        }
        public bool ByPing
        {
            get { return _byPing; }
            set
            {
                SetProperty(ref _byPing, value);
            }
        }

    }
}
