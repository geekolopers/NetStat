using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetStat.Models
{
    public sealed class ConfigModel
    {
        public string IPAddress { get; set; }
        public short? pingDelay { get; set; }
    }
}
