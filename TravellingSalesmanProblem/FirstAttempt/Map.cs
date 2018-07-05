using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstAttempt
{
    /// <summary>
    /// Класс, представляющий карту (граф)
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Перечисление всех дорог карты (рёбер графа)
        /// </summary>
        public IEnumerable<Road> Roads => _roads;
        /// <summary>
        /// Перечисление всех городов карты (вершин графа)
        /// </summary>
        public IEnumerable<City> Cities => _cities.OrderBy(_ => _.Name);
        
        /// <summary>
        /// Поиск и (или) добавление города (вершины)
        /// </summary>
        /// <param name="name">Наименование города (вершины)</param>
        /// <returns>Объект структуры города (вершины) из карты (графа)</returns>
        private City LookupCityByName(string name)
        {
            var city = new City(name);
            _cities.Add(city);
            return city;
        }

        /// <summary>
        /// Добавление дороги карты по названиям городов (ребра в граф по названиям вершин)
        /// </summary>
        /// <param name="originName">Пункт отравления</param>
        /// <param name="destinationName">Пукнт назначения</param>
        /// <param name="distance">Длина дороги (вес ребра)</param>
        /// <param name="isBidirectional">По дороге можно ехать в обе стороны (неориентированное ребро)</param>
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

        private readonly List<Road> _roads = new List<Road>();
        private readonly HashSet<City> _cities = new HashSet<City>();
    }
}