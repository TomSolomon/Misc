using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube
{
    class Program
    {
       

        static void Main(string[] args)
        {
            new Tests().RunTests();        

            List<Piece> pieces = new List<Piece>
               {
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0), new Position(1,0,1), new Position(0,1,0)}),
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0), new Position(1,0,1)}),
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0), new Position(0,0,1)}),
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(1,0,1), new Position(2, 0, 1), new Position(1, 1, 1)}),
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(1,0,1), new Position(1, 1, 1) }),
                   new Piece(new[] { new Position(0, 0, 0), new Position(1,0,0), new Position(2,0,0), new Position(1, 0,1), new Position(1, 1, 1) }),
               };
                
            // for each piece - all its possible placements in a cube
            var piecesRepresentations = new List<KeyValuePair<Piece, Piece[]>>();

            foreach (var piece in pieces)
            {
                var representations = piece.GenerateAllRepresentations();
                piecesRepresentations.Add(new KeyValuePair<Piece, Piece[]>(piece, representations.ToArray()));
            }

            var board = new Board(piecesRepresentations);
            board.Solve();
            board.Print();
            Console.ReadKey();      
        }

   
    }
}
