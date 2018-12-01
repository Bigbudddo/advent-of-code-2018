using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2018 {

    public class Day1 : IPuzzle {

        private int _value = 0;
        private int[] _frequencyChanges = null;

        public void SolvePart1() {
            _value = 0;
            _frequencyChanges = this.ParseInputPart1();

            foreach (int change in _frequencyChanges) {
                _value += change;
            }

            Console.WriteLine(String.Format("Part-1 Solution: {0}", _value));
        }

        public void SolvePart2() {
            _value = 0;
            _frequencyChanges = this.ParseInputPart1();

            int? answer = null;
            var aFrequency = new List<int>();

            do {
                foreach (int change in _frequencyChanges) {
                    _value += change;
                    
                    if (aFrequency.Contains(_value)) {
                        answer = _value;
                        break;
                    }
                    else {
                        aFrequency.Add(_value);
                    }
                }
            } while (!answer.HasValue);

            Console.WriteLine(String.Format("Part-2 Solution: {0}", answer));
        }

        private int[] ParseInputPart1() {
            var retval = new List<int>();
            string[] sInputs = File.ReadAllLines("Files/day-1-1.txt");

            // loop around all inputs and parse the data into int values
            foreach (string s in sInputs) {
                string x = s.Replace("+", "").Trim(); // ensure all white-space is gone and plus symbols
                retval.Add(int.Parse(x));
            }

            return retval.ToArray();
        }
    }
}
