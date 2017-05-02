using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cube
{
    class Board
    {
        public const int Size = 3;
        private List<KeyValuePair<Piece, Piece[]>> piecesRepresentations;
        int[,,] boardStatus;
        int[] selectedPieces;
        const int EMPTY = -1;

        public Board(List<KeyValuePair<Piece, Piece[]>> piecesRepresentations)
        {
            selectedPieces = new int[piecesRepresentations.Count];
            this.piecesRepresentations = piecesRepresentations;
            boardStatus = new int[3,3,3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    for (int k = 0; k < 3; k++)
                        boardStatus[i, j, k] = EMPTY;
        }

        public void Solve()
        {
            if (SolveFor(0))
            {
                Console.WriteLine("Selected Pieces indexes: " + string.Join(", ", selectedPieces));
                for (int i = 0; i <selectedPieces.Length; i++)
                {
                    Console.WriteLine();
                    Console.WriteLine(i + " : " + piecesRepresentations[i].Value[selectedPieces[i]].ToString());
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Failed");
            }

        }

        public bool Add(Piece piece, int pieceId)
        {
            for (int i = 0; i < piece.Positions.Length; i++)
            {
                var currPos = piece.Positions[i];
                if (boardStatus[currPos.X, currPos.Y, currPos.Z] != EMPTY)
                {
                    // revvert all changes to the board
                    for (int j = 0; j < i; j++)
                        boardStatus[piece.Positions[j].X, piece.Positions[j].Y, piece.Positions[j].Z] = EMPTY;

                    return false;
                }
                boardStatus[currPos.X, currPos.Y, currPos.Z] = pieceId;
            }

            return true;
        }

        public bool SolveFor(int currPieceIndex)
        {
            var currPieceRepresentations = piecesRepresentations[currPieceIndex].Value;
            for (int i = 0; i < currPieceRepresentations.Length; i++)
            {
                if (Add(currPieceRepresentations[i],currPieceIndex))
                {
                    selectedPieces[currPieceIndex] = i;
                    if (currPieceIndex == piecesRepresentations.Count - 1 // we are DONE!!
                        || SolveFor(currPieceIndex + 1))
                        return true;

                    else
                        Remove(currPieceRepresentations[i]);
                        // couldn't find a match to current piece. removing it before continuing 
                }
            }
            return false;
        }

        public void Remove(Piece piece)
        {
            for (int i = 0; i < piece.Positions.Length; i++)
            {
                var currPos = piece.Positions[i];
                Debug.Assert(boardStatus[currPos.X, currPos.Y, currPos.Z] != EMPTY);
                boardStatus[currPos.X, currPos.Y, currPos.Z] = EMPTY;
            }
        }

        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("   z---y---");
            Console.WriteLine("   ^--/----");
            Console.WriteLine("   |-/-----");
            Console.WriteLine("   |/------");
            Console.WriteLine("   /------>x");
            Console.WriteLine();
            Console.WriteLine();

            int[] indexes = new int[3];
            Console.WriteLine();
            for (int z = 2; z >= 0; z--)
            {
                for (int y = 2; y >= 0; y--)
                {
                    for (int x = 0; x < 3; x++)
                        indexes[x] = boardStatus[x, y, z];
                    var chars = indexes.Select(digit => digit == EMPTY ? '*' : digit.ToString()[0]);
                    Console.WriteLine(string.Join(", ",chars));
                }
                Console.WriteLine();
            }
        }
    }

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
                Position.Rotate(newVals, first, second);

            return new Position(newVals);
        }

        static void Rotate(int[] vals, int first, int second)
        {
             int tmp = vals[second];
          /*       vals[second] = -vals[first];
               vals[first] = tmp;
           
          */ if (first == 1 && second == 2)
            {
                //x
                // x, -z, y
                vals[second] = vals[first];
                vals[first] = -tmp;
            }
           else
           if (first == 0 && second == 2)
            {
                //y
                //  z, y,-x
                vals[second] = -vals[first];
                vals[first] = tmp;
            }
            else
            {
                Debug.Assert(first == 0 && second == 1);
                //z
                // -y, x, z
                vals[second] = vals[first];
                vals[first] = -tmp;
            }
        }
    }
    [DebuggerDisplay("{ToString()}")]
    class Piece
    {
        Position[] initialPositions;

        public Position[] Positions { get { return initialPositions; } }

        public Piece(Position[] initialPositions)
        {
            //Debug.Assert(initialPositions[0].X == 0 && initialPositions[0].Y == 0);
            this.initialPositions = initialPositions;
        }

        // return all possible combinations of the piece rotated in index direction around (0,0,0)
        public List<Piece> Rotate(int index)
        {
            var res = new List<Piece>();

            for (int j = 0; j < 4; j++)
            {
                var newPositions = initialPositions.Select(pos => pos.Rotate(j, index)).ToArray();
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
                var newPositions = new Position[initialPositions.Length];
                for (int i = 0; i < initialPositions.Length; i++)
                {
                    var newPos = initialPositions[i].Inc(j, index);
                  //  if (newPos.Valid)
                        newPositions[i] = newPos;
                   // else
                    //{
                        // invalid point
                     //   return res;
                   // }
                }
                res.Add(new Piece(newPositions));
            }
            return res;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            sb.Append(string.Join(", ", initialPositions.Select(pos => pos.ToString())));
            sb.Append(" }");
            return sb.ToString();        
        }

        public override bool Equals(object obj)
        {
            var other = obj as Piece;
            if (other == null)
                return false;
            return ArraysEqual(other.initialPositions, initialPositions);
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
            int hash = initialPositions.Length;
            foreach (var pos in initialPositions)
            {
                hash ^= pos.GetHashCode();
            }
            return hash;
        }
    }
}