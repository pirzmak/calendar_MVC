using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarMVC.Controllers
{
    public interface IStateManager<T>
    {
        void save(string name, T state);
        T load(string name);
    }
}
