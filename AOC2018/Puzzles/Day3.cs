using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2018 {

    public class Day3 : IPuzzle {

        private readonly bool _runExample = false;


        public void SolvePart1() {
            // correct : 110195
            int multipleClaimCount = 0;
            var fabric = this.GetFabric();
            List<Square> claims = this.ParseInput1();

            foreach (Square claim in claims) {
                for (int x = claim.X; x < (claim.X + claim.Width); x++) {
                    for (int y = claim.Y; y < (claim.Y + claim.Height); y++) {
                        // check if the fabric square has been set to a claim-id
                        fabric[y, x].Add(claim.Id);
                        
                        // check to see if we have at-least 1 overlapping
                        if (fabric[y, x].Count == 2) {
                            multipleClaimCount++;
                        }
                    }
                }   
            }

            if (_runExample) {
                DrawFabric(fabric);
                Console.WriteLine();
                Console.WriteLine();
            }
            
            Console.WriteLine(String.Format("Part-1 Solution: {0}", multipleClaimCount));
        }

        public void SolvePart2() {
            var fabric = this.GetFabric();
            var nonOverlapped = new List<int>();
            List<Square> claims = this.ParseInput1();

            // setup non-overlapped
            nonOverlapped = claims.Select(x => x.Id).ToList();

            foreach (Square claim in claims) {
                for (int x = claim.X; x < (claim.X + claim.Width); x++) {
                    for (int y = claim.Y; y < (claim.Y + claim.Height); y++) {
                        // check if the fabric square has been set to a claim-id
                        fabric[y, x].Add(claim.Id);

                        // check to see if we have at-least 1 overlapping
                        if (fabric[y, x].Count > 1) {
                            // remove any Id's that have been marked as overlapping
                            nonOverlapped.RemoveAll(z => fabric[y, x].Contains(z));
                        }
                    }
                }
            }
            
            Console.WriteLine(String.Format("Part-2 Solution: {0}", nonOverlapped.FirstOrDefault()));
        }
        
        private void DrawFabric(List<int>[,] fabric, bool clearScreen = false) {
            if (clearScreen) {
                Console.Clear();
            }

            int rowLength = fabric.GetLength(0);
            int colLength = fabric.GetLength(1);

            for (int i = 0; i < rowLength; i++) {
                for (int j = 0; j < colLength; j++) {
                    if (fabric[i, j].Count == 1) {
                        Console.Write(fabric[i, j][0]);
                    }
                    else if (fabric[i, j].Count > 1) {
                        Console.Write("X");
                    }
                    else {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        private List<int>[,] GetFabric() {
            var fabric = (_runExample) ? new List<int>[8, 8] : new List<int>[1000, 1000];

            int rowLength = fabric.GetLength(0);
            int colLength = fabric.GetLength(1);

            for (int i = 0; i < rowLength; i++) {
                for (int j = 0; j < colLength; j++) {
                    fabric[i, j] = new List<int>();
                }
            }

            return fabric;
        }

        private List<Square> ParseInput1() {
            string[] instructions = null;
            if (_runExample) {
                instructions = new string[] {
                    "#1 @ 1,3: 4x4",
                    "#2 @ 3,1: 4x4",
                    "#3 @ 5,5: 2x2"
                };
            }
            else {
                instructions = File.ReadAllLines("Files/day-3-1.txt");
            }

            var retval = new List<Square>();
            foreach (string instruction in instructions) {
                retval.Add(new Square(instruction));
            }

            return retval;
        }

        private class Square {

            public int Id { get; set; }

            public int X { get; set; }
            
            public int Y { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }

            private void ParseInstruction(string instruction) {
                // start: '#1 @ 1,3: 4x4'
                // remove the # at the start and any whitespace
                instruction = instruction.Replace("#", "").Replace(" ", "");
                // now: '1@1,3:4x4'
                // we will get the id at this point
                string[] aInstructions = instruction.Split('@');
                Id = int.Parse(aInstructions[0]);
                // we now want to deal with the second part of the split: '1,3:4x4'
                string[] bInstructions = aInstructions[1].Split(':');
                // get the coordinates now
                string[] coords = bInstructions[0].Split(',');
                string[] size = bInstructions[1].Split('x');
                // setup our properties now
                X = int.Parse(coords[0]);
                Y = int.Parse(coords[1]);
                Width = int.Parse(size[0]);
                Height = int.Parse(size[1]);
            }

            public Square(string instruction) {
                ParseInstruction(instruction);
            }
        }
    }
}
