using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.Common {

    //Generic immutable pair class
    public class GPSConqTuple2<TFirst,TSecond> : System.IEquatable<GPSConqTuple2<TFirst, TSecond>>
    {
        private readonly TFirst First;
        private readonly TSecond Second; 

        public GPSConqTuple2(TFirst _first, TSecond _second)
        {
            First = _first;
            Second = _second;
        }

        public TFirst GetFrist() { return First; }
        public TSecond GetSecond() { return Second; }


        public bool Equals(GPSConqTuple2<TFirst, TSecond> other)
        {
            if (other == null)
            {
                return false;
            }
            return EqualityComparer<TFirst>.Default.Equals(this.First, other.First) &&
                   EqualityComparer<TSecond>.Default.Equals(this.Second, other.Second);
        }

        public override bool Equals(object o)
        {
            return Equals(o as GPSConqTuple2<TFirst, TSecond>);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TFirst>.Default.GetHashCode(First) * 37 +
                   EqualityComparer<TSecond>.Default.GetHashCode(Second);
        }

        public override string ToString()
        {
            return First.ToString() + " - " + Second.ToString(); 
        }

    }

}