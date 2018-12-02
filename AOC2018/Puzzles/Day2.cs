using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2018 {

    public class Day2 : IPuzzle {

        private readonly bool _runExample = false;

        public void SolvePart1() {
            int doubleCounter = 0, tripleCounter = 0;
            string[] boxIds = this.ParseInput1();

            // loop each box-ID
            // note; probably a better solution than this, but it works!
            foreach (string id in boxIds) {
                // get a count of each character to check for doubles & triple occurances
                // note; puzzle input seems to only be letters, so we shouldn't have to worry about handling numbers while checking for letters at this point!
                var letterOccurances = new Dictionary<char, int>();
                for (var i = 0; i < id.Length; i++) {
                    char x = id[i];
                    if (!letterOccurances.ContainsKey(x))
                        letterOccurances.Add(x, 1);
                    else
                        letterOccurances[x]++;
                }
                // check for doubles and triples now
                bool hasCountedTwo = false, hasCountedThree = false;
                foreach (var pair in letterOccurances) {
                    if (pair.Value == 2 && !hasCountedTwo) {
                        doubleCounter++;
                        hasCountedTwo = true;
                    }
                    else if (pair.Value == 3 && !hasCountedThree) {
                        tripleCounter++;
                        hasCountedThree = true;
                    }
                }
            }

            int checksum = doubleCounter * tripleCounter;
            Console.WriteLine(String.Format("Part-1 Solution: {0}", checksum));
        }

        public void SolvePart2() {
            // get our input data
            bool found = false;
            string answer = "";
            string[] boxIds = this.ParseInput2();

            // loop around the entire input
            foreach (string a in boxIds) {
                // loop around again to compare
                foreach (string b in boxIds) {
                    // get the distance it takes to change a into b
                    int difference = Levenshtein.Compute(a, b);
                    // we need a difference of 1!
                    if (difference == 1) {
                        found = true;
                        // build our new string of matching characters
                        var ab = new List<char>();
                        for (int i = 0; i < a.Length; i++) {
                            if (a[i] == b[i]) {
                                ab.Add(a[i]);
                            }
                        }
                        // put it all together!
                        answer = new string(ab.ToArray());
                    }

                    if (found) break;
                }

                if (found) break;
            }

            Console.WriteLine(String.Format("Part-2 Solution: {0}", answer));
        }

        private string[] ParseInput1() {
            if (_runExample) {
                return new string[] {
                    "abcdef",
                    "bababc",
                    "abbcde",
                    "abcccd",
                    "aabcdd",
                    "abcdee",
                    "ababab"
                };
            }
            else
                return File.ReadAllLines("Files/day-2-1.txt");
        }

        private string[] ParseInput2() {
            if (_runExample) {
                return new string[] {
                    "abcde",
                    "fghij",
                    "klmno",
                    "pqrst",
                    "fguij",
                    "axcye",
                    "wvxyz"
                };
            }
            else
                return File.ReadAllLines("Files/day-2-1.txt");
        }
    }
}
