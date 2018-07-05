using System;
using System.Collections.Generic;

namespace FirstAttempt
{
    public struct City : IEquatable<City>
    {
        public string Name { get; }

        public City(string name)
        {
            Name = name;
        }

        public bool Equals(City other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj.GetType() == GetType() && Equals((City) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(City left, City right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(City left, City right)
        {
            return !Equals(left, right);
        }

        private sealed class NameEqualityComparer : IEqualityComparer<City>
        {
            public bool Equals(City x, City y)
            {
                return x.GetType() == y.GetType() && string.Equals(x.Name, y.Name);
            }

            public int GetHashCode(City obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public static IEqualityComparer<City> NameComparer { get; } = new NameEqualityComparer();
    }
}