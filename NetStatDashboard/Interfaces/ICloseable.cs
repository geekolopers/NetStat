using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetStatDashboard.Interfaces
{
    public interface ICloseable
    {
        event EventHandler<EventArgs> RequestClose;
    }
}
