using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CalendarMVC.Controllers
{
    public class SessionStateManager<T> : IStateManager<T>
    {
        public void save(string name, T state)
        {
            HttpContext.Current.Session[name] = state;
        }
        public T load(string name)
        {
            return (T)HttpContext.Current.Session[name];
        }
    }
}
