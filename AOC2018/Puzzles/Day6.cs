using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2018 {

    public class Day6 : IPuzzle {

        private readonly bool _runExample = false;
        private readonly bool _renderBoard = false;

        public void SolvePart1() {
            // get our inputs
            List<ChronalCoordinate> points = this.ParseInput1(out int maximumPointSize);
            char[,] board = this.PlotBoard(maximumPointSize, points.ToArray());

            if (_renderBoard)
                DrawBoard(board);

            // get all point/plot claims and tally them all up to get totals for each coord
            Dictionary<char, int> finiteClaims = ClaimBoard(ref board, ref points);

            if (_renderBoard)
                DrawBoard(board);

            // get the max one
            int max = finiteClaims.Values.Max();
            Console.WriteLine(String.Format("Part-1 Solution: {0}", max));
        }

        public void SolvePart2() {
            throw new NotImplementedException();
        }

        private void DrawBoard(char[,] board) {
            int rowLength = board.GetLength(0);
            int colLength = board.GetLength(1);

            Console.WriteLine();
            Console.WriteLine("----------------------------------");
            Console.WriteLine();

            for (int i = 0; i < rowLength; i++) {
                Console.Write("    ");
                for (int j = 0; j < colLength; j++) {
                    Console.Write(board[i,j]);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------");
            Console.WriteLine();
        }
        
        private char[,] PlotBoard(int boardSize, params ChronalCoordinate[] points) {
            var board = new char[boardSize + 1, boardSize + 1];

            int rowLength = board.GetLength(0);
            int colLength = board.GetLength(1);

            // foreach row in the 2D array
            for (int i = 0; i < rowLength; i++) {
                // foreach column point within that row
                for (int j = 0; j < colLength; j++) {
                    ChronalCoordinate chronalCoordinate = points.FirstOrDefault(x => x.X == j && x.Y == i);
                    board[i, j] = (chronalCoordinate == null) ? '.' : chronalCoordinate.Id;
                }
            }

            return board;
        }

        private List<ChronalCoordinate> ParseInput1(out int maximumPointSize) {
            // set a string[] of points
            string[] points = null;
            if (_runExample) {
                points = new string[] {
                    "1, 1",
                    "1, 6",
                    "8, 3",
                    "3, 4",
                    "5, 5",
                    "8, 9"
                };
            }
            else {
                points = File.ReadAllLines("Files/day-6-1.txt");
            }

            // parse the data into readable chunks
            maximumPointSize = 0;
            var retval = new List<ChronalCoordinate>();
            for (var i = 0; i < points.Length; i++) {
                char id = (i <= 25) ? (char)(i + 65) : (char)(i + 71); // covers 52 points! our input is 50!
                string[] aPoint = points[i].Split(',').Select(z => z.Trim()).ToArray(); // trims the spacing

                int x = int.Parse(aPoint[0]);
                int y = int.Parse(aPoint[1]);

                // check if either of these coords are bigger than the current
                // we are going to use maximum point size for our 'board' of points
                // as we know that it can be infinity but we don't want an infinity array?!
                if (x > maximumPointSize)
                    maximumPointSize = x;
                if (y > maximumPointSize)
                    maximumPointSize = y;

                // create the point
                retval.Add(new ChronalCoordinate(id, x, y));
            }

            // return the result
            return retval;
        }

        private Dictionary<char, int> ClaimBoard(ref char[,] board, ref List<ChronalCoordinate> points) {
            int rowLength = board.GetLength(0);
            int colLength = board.GetLength(1);

            var claimCoverage = new Dictionary<char, int>();
            for (int i = 0; i < rowLength; i++) {
                for (int j = 0; j < colLength; j++) {
                    var claims = new Dictionary<char, int>();
                    foreach (var coord in points) {
                        int distance = Heuristics.ManhattanDistance(coord.X, j, coord.Y, i);
                        claims.Add(coord.Id, distance);
                    }

                    int closestValue = claims.Values.Min();
                    if (claims.Values.Where(x => x == closestValue).Count() <= 1) {
                        // we have exactly 1 closest claim!
                        char id = claims.Where(x => x.Value == closestValue).FirstOrDefault().Key;
                        board[i, j] = id;

                        // add to our claim coverage
                        if (!claimCoverage.ContainsKey(id)) {
                            claimCoverage.Add(id, 1);
                        }
                        else {
                            claimCoverage[id]++;
                        }
                    }
                }
            }

            var finiteClaims = new Dictionary<char, int>();
            foreach (var point in points.OrderBy(x => x.Id)) {
                // check if we are around the boundaries of our board
                if (board.GetRow(0).Contains(point.Id) ||
                    board.GetRow(rowLength - 1).Contains(point.Id) ||
                    board.GetCol(0).Contains(point.Id) ||
                    board.GetCol(colLength - 1).Contains(point.Id)) {
                    // set finite to false, it's impossible for this point to be finite because we assume outside the boundaries of our board is infinity!
                    point.IsFinite = false;
                    continue;
                }

                // check to see if we have any claims at all?
                if (!claimCoverage.ContainsKey(point.Id)) {
                    point.IsFinite = false;
                    continue;
                }

                // now we need to check the ones we know could be finite. 
                // in the example only D & E should get to this point

                point.IsFinite = true; // test, this will be changed?
                finiteClaims.Add(point.Id, claimCoverage[point.Id]);
            }

            return finiteClaims;
        }

        private class ChronalCoordinate {

            public char Id { get; private set; }

            public int X { get; private set; }

            public int Y { get; private set; }

            public bool IsFinite { get; set; }

            public ChronalCoordinate(char id, int x, int y) {
                Id = id;
                X = x;
                Y = y;
            }
        }
    }
}
