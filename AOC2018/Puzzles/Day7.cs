using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2018 {

    public class Day7 : IPuzzle {

        private readonly bool _runExample = false;

        public void SolvePart1() {
            // get the input
            Dictionary<char, List<char>> input = this.ParseInput1();
            int stepsToComplete = input.Keys.Count;
            var stepsCompleted = new List<char>();

            do {
                // find the next step
                char id = input.OrderBy(x => x.Key).Where(y => y.Value.Count == 0).Select(z => z.Key).FirstOrDefault();
                stepsCompleted.Add(id);

                // remove this step from each input
                foreach (var i in input) {
                    i.Value.Remove(id);
                }

                // remove from input
                input.Remove(id);
            } while (stepsCompleted.Count() < stepsToComplete);

            string answer = new string(stepsCompleted.ToArray());
            Console.WriteLine(String.Format("Part-1 Solution: {0}", answer));
        }

        public void SolvePart2() {
            throw new NotImplementedException();
        }

        private Dictionary<char, List<char>> ParseInput1() {
            string[] instructions = null;
            if (_runExample) {
                instructions = new string[] {
                    "Step C must be finished before step A can begin.",
                    "Step C must be finished before step F can begin.",
                    "Step A must be finished before step B can begin.",
                    "Step A must be finished before step D can begin.",
                    "Step B must be finished before step E can begin.",
                    "Step D must be finished before step E can begin.",
                    "Step F must be finished before step E can begin."
                };
            }
            else {
                instructions = File.ReadAllLines("Files/day-7-1.txt");
            }

            var retval = new Dictionary<char, List<char>>();
            foreach (string instruction in instructions) {
                string[] aInstruction = instruction.Split(' ');
                char a = char.Parse(aInstruction[1]); // position 2 in the split string
                char b = char.Parse(aInstruction[7]); // position 8 in the split string

                // add the steps to the dictionary
                if (!retval.ContainsKey(a)) {
                    retval.Add(a, new List<char>());
                }

                if (!retval.ContainsKey(b)) {
                    retval.Add(b, new List<char>());
                }

                retval[b].Add(a);
            }
            
            return retval;
        }
    }
}
