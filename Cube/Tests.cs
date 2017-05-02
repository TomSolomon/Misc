using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cube
{
    class Tests
    {
        public void RunTests()
        {
            MoveTests();
            RotationsTests();
            GeneratingRepresentationTest();
        }

        void MoveTests()
        {
            var piece = new Piece(new[] { new Position(0, 0, 0) });
            var res = piece.GenerateAllMoved();
            Debug.Assert(res.Count == 27);

            piece = new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) });
            res = piece.GenerateAllMoved();
            Debug.Assert(res.Count == 9);

            piece = new Piece(new[] { new Position(0, 0, 0), new Position(1, 1, 1), new Position(2, 2, 2) });
            res = piece.GenerateAllMoved();
            Debug.Assert(res.Count == 1);

            piece = new Piece(new[] { new Position(0, 0, 0), new Position(1, 1, 1) });
            res = piece.GenerateAllMoved();
            Debug.Assert(res.Count == 8);
        }

        void RotationsTests()
        {
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

            var piece = new Piece(new[] { new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2) });
            var res = piece.GenerateAllRotations();
            Debug.Assert(res.Count == 64);

            List<Piece> total = new List<Piece>();
            foreach (var rotatedPiece in res)
            {
                total.AddRange(rotatedPiece.GenerateAllMoved());
            }
            var disticted = total.Distinct().ToArray();
            Debug.Assert(disticted.Length == 3 * 9);
        }

        void GeneratingRepresentationTest()
        {
            var piece = new Piece(new[]
           {
                new Position(0, 0, 0), new Position(0, 0, 1), new Position(0, 0, 2),
                new Position(0, 1, 0), new Position(0, 1, 1), new Position(0, 1, 2),
                new Position(0, 2, 0), new Position(0, 2, 1), new Position(0, 2, 2),
            });

            var rep = piece.GenerateAllRepresentations();
            Debug.Assert(rep.Count() == 9);


            // a 3*3*3 piece with one of its corner removed.
            // this asymetric change will help making sure the generate rotations method works fine
            var piecePositions = new List<Position>();

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    for (int k = 0; k < 3; k++)
                        piecePositions.Add(new Position(i, j, k));

            // removing the corner:
            piecePositions.RemoveAt(piecePositions.Count - 1);
            piece = new Piece(piecePositions.ToArray());

            // we removed a corner, so since there are 8 corners in a cube, we will have 8 possible representations
            Debug.Assert(piece.GenerateAllRepresentations().Count() == 8);

            // this time, we will remove the middle position from one of the faces
            piecePositions = new List<Position>();

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    for (int k = 0; k < 3; k++)
                        piecePositions.Add(new Position(i, j, k));

            // removing the middle of some face:
            piecePositions.Remove(new Position(1, 1, 0));
            Debug.Assert(piecePositions.Count == 26);
            piece = new Piece(piecePositions.ToArray());

            // we removed the middle of one face, so since there are 6 faces in a cube, we will have 6 possible representations
            Debug.Assert(piece.GenerateAllRepresentations().Count() == 6);
        }
    }
}
