using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstAttempt
{
    public static class MapExtensions
    {
        public class DataHolder
        {
            public Stack<City> Path { get; } = new Stack<City>();
            public double CurrentDistance { get; set; }
            public double MinimumDistance { get; set; } = double.MaxValue;
            public List<City> MinimumPath { get; set; }
        }

        public static IEnumerable<Road> GetRoadsFromCity(this Map map, City origin)
        {
            return map.Roads.Where(_ =>
                _.Origin == origin && !_.IsBidirectional || _.Cities.Contains(origin) && _.IsBidirectional);
        }

        public static void RecursiveWalk(this Map map, DataHolder holder)
        {
            if (map.AllCitiesVisited(holder.Path))
            {
                if (holder.CurrentDistance < holder.MinimumDistance)
                {
                    holder.MinimumDistance = holder.CurrentDistance;
                    holder.MinimumPath = holder.Path.Reverse().ToList();
                }
            }

            var currentCity = holder.Path.Peek();

            foreach (var road in map.GetRoadsFromCity(currentCity))
            {
                var nextCity = currentCity != road.Destination ? road.Destination : road.Origin;

                //if (path.Contains(nextCity)) continue;
                if (holder.Path.Count(_ => _ == nextCity) > 1) continue;

                holder.Path.Push(nextCity);
                holder.CurrentDistance += road.Distance;

                map.RecursiveWalk(holder);

                holder.CurrentDistance -= road.Distance;
                holder.Path.Pop();
            }
        }

        public static bool AllCitiesVisited(this Map map, Stack<City> path)
        {
            return map.Cities.Distinct().Count() == path.Distinct().Count();
        }

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

        public static string ToString(Stack<City> path)
        {
            return string.Join(" → ", path.Reverse().Select(_ => _.Name));
        }

        public static void SolveExample()
        {
            var map = ExampleMap;

            Console.WriteLine($"Всего городов: {map.Cities.Count()}");

            var origin = map.Cities.First(_ => _.Name == "Киев");

            var holder = new DataHolder();
            holder.Path.Push(origin);

            map.RecursiveWalk(holder);

            Console.WriteLine(
                $"Min path: {string.Join(" → ", holder.MinimumPath.Select(_ => _.Name))}, {holder.MinimumDistance:0.00} km");
        }
    }
}