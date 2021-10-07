using Airports.Logic.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airports.Logic.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private IEnumerable<T> _collection;

        public T Get(string[] param)
        {
            throw new NotImplementedException();
        }
    }
}
