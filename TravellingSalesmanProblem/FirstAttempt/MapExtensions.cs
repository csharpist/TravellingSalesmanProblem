using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstAttempt
{
    /// <summary>
    /// Методы-расширения для карты (графа) Map
    /// </summary>
    public static class MapExtensions
    {
        /// <summary>
        /// Служебный внутренний класс для хранения текущего пути, текущей длины пути,
        /// отслеживания минимального пути и его длины
        /// </summary>
        public class DataHolder
        {
            public Stack<City> Path { get; } = new Stack<City>();
            public double CurrentDistance { get; set; }
            public double MinimumDistance { get; set; } = double.MaxValue;
            public List<City> MinimumPath { get; set; }
        }

        /// <summary>
        /// Получение всех дорог (рёбер) из данного города (вершины)
        /// </summary>
        /// <param name="map">Карта (граф)</param>
        /// <param name="origin">Город отправления (исходная вершина)</param>
        /// <returns>Коллекция дорог (вершин)</returns>
        public static IEnumerable<Road> GetRoadsFromCity(this Map map, City origin)
        {
            return map.Roads.Where(_ =>
                _.Origin == origin && !_.IsBidirectional || _.Cities.Contains(origin) && _.IsBidirectional);
        }

        /// <summary>
        /// Рекурсивный обход карты (графа)
        /// </summary>
        /// <param name="map">Карта (граф)</param>
        /// <param name="holder">Служебный объект с данными о пути</param>
        public static void RecursiveWalk(this Map map, DataHolder holder)
        {
            if (map.AllCitiesVisited(holder.Path))                          // если все города (вершины) обошли
            {
                if (holder.CurrentDistance < holder.MinimumDistance)        // и это более короткий, чем ранее найденный путь
                {
                    holder.MinimumDistance = holder.CurrentDistance;        // обновляем данные о длине минимального пути
                    holder.MinimumPath = holder.Path.Reverse().ToList();    // и сохраняем сам путь
                }
            }

            var currentCity = holder.Path.Peek();                           // Текущий город (вершина) на вершине стека

            foreach (var road in map.GetRoadsFromCity(currentCity))         // Перебираем, в какие города из текущего можем попасть
            {
                var nextCity = currentCity != road.Destination ? road.Destination : road.Origin;
                                                                            // Если дорога в две стороны, то выбираем правильный город

                //if (path.Contains(nextCity)) continue;                    // в каждый город не более раза заходим
                if (holder.Path.Count(_ => _ == nextCity) > 1) continue;    // в каждый город не более двух раз заходим

                holder.Path.Push(nextCity);                                 // В стек наверх положили очередной город,
                holder.CurrentDistance += road.Distance;                    // прибавили к текущему расстоянию расстояние
                                                                            // выбранной дороги

                map.RecursiveWalk(holder);                                  // Рекурсивно повторяем

                holder.CurrentDistance -= road.Distance;                    // Выйдя из рекурсии, возвращаем значение текущей длины пути и
                holder.Path.Pop();                                          // с верхушки стека убираем город
            }
        }

        /// <summary>
        /// Проверка, все ли города (вершины) данным посещены путём
        /// </summary>
        /// <param name="map">Карта (граф)</param>
        /// <param name="path">Путь</param>
        /// <returns>Посещены ли все города (вершины)</returns>
        public static bool AllCitiesVisited(this Map map, Stack<City> path)
        {
            return map.Cities.Distinct().Count() == path.Distinct().Count();
        }

        /// <summary>
        /// Карта (граф) для примера
        /// </summary>
        public static Map ExampleMap
        {
            get
            {
                var map = new Map();
                map.AddRoad("Краснодар", "Москва", 1346, isBidirectional: true);
                map.AddRoad("Краснодар", "Белореченск", 102, isBidirectional: true);
                map.AddRoad("Москва", "Киев", 865, isBidirectional: true);
                map.AddRoad("Киев", "Житомир", 140, isBidirectional: true);
                map.AddRoad("Киев", "Краснодар", 1200, isBidirectional: true);
                map.AddRoad("Житомир", "Краснодар", 1226, isBidirectional: true);
                map.AddRoad("Житомир", "Москва", 1005, isBidirectional: true);
                map.AddRoad("Москва", "Улан-Удэ", 5625, isBidirectional: true);
                return map;
            }
        }

        /// <summary>
        /// Получение строки пути в виде A → B → C для стека
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Строковое представление пути</returns>
        public static string ToString(Stack<City> path)
        {
            return ToString(path.Reverse().ToList());
        }

        /// <summary>
        /// Получение строки пути в виде A → B → C для списка
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Строковое представление пути</returns>
        public static string ToString(List<City> path)
        {
            return string.Join(" → ", path.Select(_ => _.Name));
        }

        /// <summary>
        /// Пример вызова метода обхода карты (графа)
        /// </summary>
        public static void SolveExample()
        {
            var map = ExampleMap;

            Console.WriteLine($"Всего городов: {map.Cities.Count()}");

            var origin = map.Cities.First(_ => _.Name == "Киев"); // Выбираем, с какого города (вершины) начнём обход

            var holder = new DataHolder();
            holder.Path.Push(origin);

            map.RecursiveWalk(holder);

            Console.WriteLine(
                $"Min path: {string.Join(" → ", holder.MinimumPath.Select(_ => _.Name))}, {holder.MinimumDistance:0.00} km");
        }
    }
}