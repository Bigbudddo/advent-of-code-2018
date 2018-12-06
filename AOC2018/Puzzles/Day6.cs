using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2018 {

    public class Day6 : IPuzzle {

        private readonly bool _runExample = true;
        private readonly bool _renderBoard = true;

        public void SolvePart1() {
            // get our inputs
            List<ChronalCoordinate> points = this.ParseInput1(out int maximumPointSize);
            char[,] board = this.PlotBoard(maximumPointSize, points.ToArray());

            if (_renderBoard)
                DrawBoard(board);

            // get all point/plot claims and tally them all up to get totals for each coord
            ClaimBoard(ref board, points.ToArray());

            if (_renderBoard)
                DrawBoard(board);

            // get the max one
            //int max = totalPointClaims.Values.Max();
            int max = -1;
            Console.WriteLine(String.Format("Part-1 Solution: {0}", max));
        }

        public void SolvePart2() {
            throw new NotImplementedException();
        }

        private bool IsFinite(char[,] board, int x, int y) {
            if (x < 1 || x >= board.GetLength(0))
                return false;
            if (y < 1 || y >= board.GetLength(1))
                return false;

            // check north

            // check south

            // check west

            // check east

            return true;
        }

        private void ClaimBoard(ref char[,] board, params ChronalCoordinate[] points) {
            int rowLength = board.GetLength(0);
            int colLength = board.GetLength(1);

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
                    }
                }
            }
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
        
        private Dictionary<char, int> GetFiniteClaims(char[,] board) {
            var retval = new Dictionary<char, int>();

            int rowLength = board.GetLength(0);
            int colLength = board.GetLength(1);

            for (int i = 0; i < rowLength; i++) {
                for (int j = 0; j < colLength; j++) {
                    // check if the current character is finite
                    if (IsFinite(board, i, j)) {
                        char id = board[i, j];
                        if (!retval.ContainsKey(id)) {
                            retval.Add(id, 1);
                        }
                        else {
                            retval[id]++;
                        }
                    }
                }
            }

            return retval;
        }

        private class ChronalCoordinate {

            public char Id { get; private set; }

            public int X { get; private set; }

            public int Y { get; private set; }

            public ChronalCoordinate(char id, int x, int y) {
                Id = id;
                X = x;
                Y = y;
            }
        }
    }
}
