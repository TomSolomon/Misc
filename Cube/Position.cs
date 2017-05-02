using System;
using System.Diagnostics;

namespace Cube
{
    [DebuggerDisplay("{ToString()}")]
    class Position : IEquatable<Position>
    {
        private int[] vals;

        public Position(int x, int y, int z)
        {
            vals = new int[3] { x, y, z };
        }

        private Position(int[] vals)
        {
            this.vals = vals;
        }

        public int this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < 3);
                return vals[index];
            }
        }

        public int X { get { return vals[0]; } }
        public int Y { get { return vals[1]; } }
        public int Z { get { return vals[2]; } }

        public bool Valid
        {
            get
            {
                return vals[0] >= 0 && vals[0] < Board.Size &&
                        vals[1] >= 0 && vals[1] < Board.Size &&
                        vals[2] >= 0 && vals[2] < Board.Size;
            }
        }

        public override string ToString()
        {
            return "[" + string.Join(",", vals) + "]";
        }

        public override bool Equals(object obj)
        {
            var other = obj as Position;
            return other == null ? false : other.Equals(this);
        }

        public bool Equals(Position other)
        {
            return this == other ||
                (X == other.X && Y == other.Y && Z == other.Z);
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Z;
        }

        // notice that result might be invalid
        public Position Inc(int inc, int index)
        {
            int[] newVals = new[] { vals[0], vals[1], vals[2] };
            newVals[index] += inc;
            return new Position(newVals);
        }

        // rotate point around (0,0,0). the order is the power of the rotation group generator.
        // i.e. order = 0: don't rotate.
        // order = 1: rotate 90 degrees.
        // order = 2: rotate 360 degrees. etc.
        // notice that result might be invalid
        public Position Rotate(int order, int ignoredIndex)
        {
            // rotation matrix is [0, 1, -1, 0]
            int[] newVals = new[] { vals[0], vals[1], vals[2] };
            int first = ignoredIndex == 0 ? 1 : 0;
            int second = ignoredIndex == 2 ? 1 : 2;
            Debug.Assert(first != second);
            for (int i = 0; i < order; i++)
                Rotate(newVals, first, second);

            return new Position(newVals);
        }

        static void Rotate(int[] vals, int first, int second)
        {
            // well.. rotation orientation is different in each axis, but this is the easiest solution
            int tmp = vals[second];
            vals[second] = -vals[first];
            vals[first] = tmp;
        }
    }
}
