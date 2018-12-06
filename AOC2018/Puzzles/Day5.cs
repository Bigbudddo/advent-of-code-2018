using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2018 {

    public class Day5 : IPuzzle {

        private readonly bool _runExample = false;
        private readonly bool _consoleLog = false;
        private readonly int _difference = 32;

        public void SolvePart1() {
            string input = this.GetInput();

            while (HasReactions(input, out int currentPosition, out int previousPosition)) {
                string i = input;
                StripFirstReaction(ref input, previousPosition);

                if (_consoleLog)
                    Console.WriteLine("Input: {0} after reaction: {1}", i, input);
            }

            Console.WriteLine(String.Format("Part-1 Solution: {0}", input.Length));
        }

        public void SolvePart2() {
            string input = this.GetInput();
            int shortestLength = 999999;

            // loop the alphabet
            for (var i = 97; i <= 122; i++) {
                char a = (char)i; // covert to lower-case value
                char b = char.ToUpper(a); // get the upper-case value
                // note; this replace is a bit messy!
                string strippedInput = input.Replace(a, ((char)94)).Replace(b, ((char)94)).Replace("^", "").Trim();

                if (_consoleLog)
                    Console.WriteLine(strippedInput);
                
                while (HasReactions(strippedInput, out int currentPosition, out int previousPosition)) {
                    string x = strippedInput;
                    StripFirstReaction(ref strippedInput, previousPosition);

                    if (_consoleLog)
                        Console.WriteLine("Input: {0} after reaction: {1}", x, strippedInput);
                }

                int l = strippedInput.Length;
                
                if (_consoleLog)
                    Console.WriteLine("for {0} the length was: {1}", a, l);
                
                if (l < shortestLength) {
                    shortestLength = l;
                }

                Console.WriteLine("Finished Pass on: {0}", a);
            }

            Console.WriteLine(String.Format("Part-2 Solution: {0}", shortestLength));
        }

        private bool HasReactions(string input, out int currentPosition, out int previousPosition) {
            // loop each character
            currentPosition = 1;
            previousPosition = 0;

            for (var i = 1; i < input.Length; i++) {
                currentPosition = i;
                previousPosition = i - 1;

                int difference = Math.Abs((int)input[previousPosition] - (int)input[currentPosition]);

                if (_consoleLog)
                    Console.WriteLine("{0} and {1} difference = {2}", input[previousPosition], input[currentPosition], difference);
                
                if (difference == _difference) {
                    if (_consoleLog)
                        Console.WriteLine(input[previousPosition] + " and " + input[currentPosition] + " reacted!");
                    return true;
                }
            }

            return false;
        }

        private void StripFirstReaction(ref string input, int x) {
            List<char> aInput = input.ToCharArray().ToList();
            aInput.RemoveRange(x, 2);
            input = new string(aInput.ToArray());
        }

        private string GetInput() {
            if (_runExample) {
                return "dabAcCaCBAcCcaDA";
            }
            else {
                return File.ReadAllText("Files/day-5-1.txt");
            }
        }
    }
}
