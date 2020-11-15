using _8_gyak.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_gyak.Entities
{
    public class CarFactory : IToyFactory
    {
        public Car CreateNew()
        {
            return new Car();
        }
    }
}
