using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstAttempt
{
    /// <summary>
    /// Класс, представляющий дорогу карты (ребро графа)
    /// </summary>
    public class Road
    {
        /// <summary>
        /// Является двусторонней дорогой (неориентированным ребром)
        /// </summary>
        public bool IsBidirectional { get; set; }

        /// <summary>
        /// Конструктор с инциализацией
        /// </summary>
        /// <param name="origin">Город отправления (исходная вершина)</param>
        /// <param name="destination">Города назначения (конечная вершина)</param>
        public Road(City origin, City destination)
        {
            if (origin == destination) throw new ArgumentException("Указаны два одинаковых города");
            _cities.Add(origin);
            _cities.Add(destination);
        }

        /// <summary>
        /// Пункт отправления (считаем его первым)
        /// </summary>
        public City Origin => _cities.First();
        /// <summary>
        /// Пункт назначения (считаем его последним)
        /// </summary>
        public City Destination => _cities.Last();

        /// <summary>
        /// Длина дороги (вес ребра)
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Города, соединящиеся дорогой (вершины ребра)
        /// </summary>
        public IEnumerable<City> Cities => _cities;

        private readonly List<City> _cities = new List<City>();
        
    }
}