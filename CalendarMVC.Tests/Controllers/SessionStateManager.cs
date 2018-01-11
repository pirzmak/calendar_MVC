using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CalendarMVC.Controllers
{
    public class TestSessionStateManager<T> : IStateManager<T>
    {
        private Dictionary<string,T> variable = new Dictionary<string, T>();

        public void save(string name, T state)
        {
            variable[name] = state;
        }
        public T load(string name)
        {
            if (variable.ContainsKey(name))
                return (T)variable[name];
            else return default(T);
        }
    }
}
