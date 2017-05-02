using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cube
{
    class Program
    {
        static void Test()
        {
            var piece = new Piece(new[] { new Position(0, 0, 0) });
            var res = GenerateAllMoved(piece);
            Debug.Assert(res.Count == 27);

            piece = new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0,0,2) });
            res = GenerateAllMoved(piece);
            Debug.Assert(res.Count == 9);

            piece = new Piece(new[] { new Position(0, 0, 0), new Position(1, 1, 1), new Position(2, 2, 2) });
            res = GenerateAllMoved(piece);
            Debug.Assert(res.Count == 1);

            piece = new Piece(new[] { new Position(0, 0, 0), new Position(1, 1, 1) });
            res = GenerateAllMoved(piece);
            Debug.Assert(res.Count == 8);

            var p = new Position(0, 0, 0);
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                    Debug.Assert(p.Rotate(i, j).Equals(p));

            p = new Position(0, 0, 1);
            for (int j = 0; j < 3; j++)
            {
                Debug.Assert(p.Rotate(0, j).Equals(p));
                Debug.Assert(p.Rotate(4, j).Equals(p));
            }

            List<Position> rotated = new List<Position>();
            for (int j = 0; j < 4; j++)
            {
                rotated.Add(p.Rotate(j, 1));
            }
            Debug.Assert(rotated.Distinct().Count() == 4);

            piece = new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) });
            res = GenerateAllRotations(piece);
            Debug.Assert(res.Count == 64);

            List<Piece> total = new List<Piece>();
            foreach (var rotatedPiece in res)
            {
                total.AddRange(GenerateAllMoved(rotatedPiece));
            }
            var disticted = total.Distinct().ToArray();
            Debug.Assert(disticted.Length == 3*9);            

            piece = new Piece(new[] 
            {
                new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2),
                new Position(0, 1, 0), new Position(0, 1, 1), new Position(0, 1, 2),
                new Position(0, 2, 0), new Position(0, 2, 1), new Position(0, 2, 2),
            });

            var rep = GenerateAllRepresentations(piece);
            Debug.Assert(rep.Count() == 9);
        }

        static void Test2()
        {
            var piece = new Piece(new[] { new Position(0, 0, 0), new Position(1, 0, 0), new Position(2, 0, 0), new Position(1, 0, 1), new Position(0, 1, 0) });
            var piecesRepresentations = new List<KeyValuePair<Piece, Piece[]>>();

            var representations = GenerateAllRepresentations(piece);
            piecesRepresentations.Add(new KeyValuePair<Piece, Piece[]>(piece, representations.ToArray()));

            var board = new Board(piecesRepresentations);
            var x = GenerateAllRepresentations(piece);
            var y = x.ToArray();
            // y should b 8 + 8 for every axis rotation
            // +4 
            foreach (var p in GenerateAllRepresentations(piece))
            {
                Debug.Assert(board.Add(p, 0));                
                board.Print();
          //      Console.ReadKey();
                board.Remove(p);
            }
        }

        static void Main(string[] args)
        {
            Test();


            Test2();

            List<Piece> pieces = new List<Piece>
               {
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0), new Position(1,0,1), new Position(0,1,0)}),
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0), new Position(1,0,1)}),
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0), new Position(0,0,1)}),
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(1,0,1), new Position(2, 0, 1), new Position(1, 1, 1)}),
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(1,0,1), new Position(1, 1, 1) }),
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0), new Position(1, 0,1), new Position(1, 1, 1) }),
               };
        /*    List<Piece> pieces = new List<Piece>
            {
                new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0),new Position(0, 1, 0), new Position(1,1,0), new Position(2,1,0),new Position(0, 2, 0), new Position(1,2,0), new Position(2,2,0),}),
                new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0),new Position(0, 1, 0), new Position(1,1,0), new Position(2,1,0),new Position(0, 2, 0), new Position(1,2,0), new Position(2,2,0),}),
                new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0),new Position(0, 0, 1), new Position(1,0,1), new Position(2,0,1),new Position(0, 0, 2), new Position(1,0,2), new Position(2,0,2),}),
            };
           List<Piece> pieces = new List<Piece>
            {
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),

            };
            List<Piece> pieces = new List<Piece>
            {
                new Piece(new[] { new Position(0, 0, 0) } ),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) }),
                new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1)}),
            };
            */            
            // for each piece - all its possible placements in a cube
            var piecesRepresentations = new List<KeyValuePair<Piece, Piece[]>>();

            foreach (var piece in pieces)
            {
                var representations = GenerateAllRepresentations(piece);
                piecesRepresentations.Add(new KeyValuePair<Piece, Piece[]>(piece, representations.ToArray()));
            }

            var board = new Board(piecesRepresentations);
            board.Solve();
            board.Print();
            Console.ReadKey();      
        }

        private static IEnumerable<Piece> GenerateAllRepresentations(Piece piece)
        {
            // list of MOVED pieces (without any rotation compare to the original one)
            var allCombinations = new List<Piece>();

            foreach (var rotatedPiece in GenerateAllRotations(piece))
            {
                allCombinations.AddRange(GenerateAllMoved(rotatedPiece));
            }

            return allCombinations.Distinct();
        }

        private static List<Piece> GenerateAllMoved(Piece piece)
        {
            // list of MOVED pieces (without any rotation compare to the original one)
            var movedPieces = new List<Piece>();


            // sometimes need to move in x & y & z to get a valid position
            foreach (var x in piece.Move(0))
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

        private static List<Piece> GenerateAllRotations(Piece piece)
        {
            // list of rotated pieces (without any move compare to the original one)
            var rotatedPieces = new List<Piece>();

            foreach (var x in piece.Rotate(0))
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
