using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airports.Logic.Models.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(string[] param);
    }
}
