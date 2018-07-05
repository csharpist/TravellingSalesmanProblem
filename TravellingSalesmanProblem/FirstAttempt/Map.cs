using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstAttempt
{
    public class Map
    {
        private readonly List<Road> _roads = new List<Road>();
        private readonly HashSet<City> _cities = new HashSet<City>();
        public IEnumerable<Road> Roads => _roads;
        public IEnumerable<City> Cities => _cities.OrderBy(_ => _.Name);

        private City LookupCityByName(string name)
        {
            var city = new City(name);
            _cities.Add(city);
            return city;
        }

        public void AddRoad(string originName, string destinationName, double distance, bool isBidirectional = false)
        {
            var origin = LookupCityByName(originName);
            var destination = LookupCityByName(destinationName);

            if (origin == destination)
            {
                throw new ArgumentException("Указан один и тот же город");
            }

            var road = new Road(origin, destination)
            {
                IsBidirectional = isBidirectional,
                Distance = distance
            };

            _roads.Add(road);
        }
    }
}