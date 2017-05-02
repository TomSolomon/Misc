using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            boardStatus = new int[3, 3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    for (int k = 0; k < 3; k++)
                        boardStatus[i, j, k] = EMPTY;
        }

        public void Solve()
        {
            if (SolveFor(0))
            {
                Console.WriteLine("----------------------------- SUCCESS -----------------------------");
                Console.WriteLine("Selected Pieces indexes: " + string.Join(", ", selectedPieces));
                for (int i = 0; i < selectedPieces.Length; i++)
                {
                    Console.WriteLine("Piece " + i + " : " + piecesRepresentations[i].Value[selectedPieces[i]].ToString());
                }
            }
            else
            {
                Console.WriteLine("----------------------------- Fail -----------------------------");
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
                if (Add(currPieceRepresentations[i], currPieceIndex))
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
            Console.WriteLine("   z   y");
            Console.WriteLine("   ^  /");
            Console.WriteLine("   | /");
            Console.WriteLine("   |/");
            Console.WriteLine("   /------>x");
            Console.WriteLine();

            int[] indexes = new int[3];
            for (int z = 2; z >= 0; z--)
            {
                for (int y = 2; y >= 0; y--)
                {
                    for (int x = 0; x < 3; x++)
                        indexes[x] = boardStatus[x, y, z];
                    var chars = indexes.Select(digit => digit == EMPTY ? '*' : digit.ToString()[0]);
                    Console.WriteLine(string.Join(", ", chars));
                }
                Console.WriteLine();
            }
        }
    }
}
