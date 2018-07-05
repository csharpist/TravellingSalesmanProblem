using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstAttempt
{
    public class Road
    {
        public bool IsBidirectional { get; set; }

        public Road(City origin, City destination)
        {
            if (origin == destination) throw new ArgumentException("Указаны два одинаковых города");
            _cities.Add(origin);
            _cities.Add(destination);
        }

        public City Origin => _cities.First();
        public City Destination => _cities.Last();

        public double Distance { get; set; }

        private readonly List<City> _cities = new List<City>();
        public IEnumerable<City> Cities => _cities;
    }
}