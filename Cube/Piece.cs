using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cube
{
    [DebuggerDisplay("{ToString()}")]
    class Piece
    {
        public Position[] Positions { get; private set; }

        public Piece(Position[] initialPositions)
        {
            Positions = initialPositions;
        }

        // return all possible combinations of the piece rotated in index direction around (0,0,0)
        public List<Piece> Rotate(int index)
        {
            var res = new List<Piece>();

            for (int j = 0; j < 4; j++)
            {
                var newPositions = Positions.Select(pos => pos.Rotate(j, index)).ToArray();
                res.Add(new Piece(newPositions));
            }
            return res;
        }

        // return all possible combinations of the piece moved in position index
        public List<Piece> Move(int index)
        {
            var res = new List<Piece>();

            for (int j = 0; j < 3; j++)
            {
                var newPositions = new Position[Positions.Length];
                for (int i = 0; i < Positions.Length; i++)
                {
                    newPositions[i] = Positions[i].Inc(j, index);                  
                }
                res.Add(new Piece(newPositions));
            }
            return res;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            sb.Append(string.Join(", ", Positions.Select(pos => pos.ToString())));
            sb.Append(" }");
            return sb.ToString();        
        }

        public override bool Equals(object obj)
        {
            var other = obj as Piece;
            if (other == null)
                return false;
            return ArraysEqual(other.Positions, Positions);
        }

        static bool ArraysEqual(Position[] a1, Position[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            HashSet<Position> set = new HashSet<Position>(a1);
            
            for (int i = 0; i < a2.Length; i++)
            {                
                if (!set.Contains(a2[i]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = Positions.Length;
            foreach (var pos in Positions)
            {
                hash ^= pos.GetHashCode();
            }
            return hash;
        }

        public IEnumerable<Piece> GenerateAllRepresentations()
        {
            // list of MOVED pieces (without any rotation compare to the original one)
            var allCombinations = new List<Piece>();

            foreach (var rotatedPiece in GenerateAllRotations())
            {
                allCombinations.AddRange(rotatedPiece.GenerateAllMoved());
            }

            return allCombinations.Distinct();
        }

        // creating all possible representations of current piece by only moving its in x,y,z directions.
        // returning only valid pieces
        internal List<Piece> GenerateAllMoved()
        {
            // list of MOVED pieces (without any rotation compare to the original one)
            var movedPieces = new List<Piece>();

            // sometimes need to move in x & y & z to get a valid position
            foreach (var x in Move(0))
            {
                foreach (var y in x.Move(1))
                {
                    foreach (var z in y.Move(2))
                        movedPieces.Add(z);
                }
            }
            movedPieces.RemoveAll(p => !p.Positions.All(pos => pos.Valid));
            return movedPieces;
        }

        // creating all possible representations of current piece by only rotating its in x,y,z directions.
        // returning valid AND INVALID pieces
        internal List<Piece> GenerateAllRotations()
        {
            // list of rotated pieces (without any move compare to the original one)
            var rotatedPieces = new List<Piece>();

            foreach (var x in Rotate(0))
            {
                foreach (var y in x.Rotate(1))
                {
                    foreach (var z in y.Rotate(2))
                        rotatedPieces.Add(z);
                }
            }

            return rotatedPieces;
        }
    }
}