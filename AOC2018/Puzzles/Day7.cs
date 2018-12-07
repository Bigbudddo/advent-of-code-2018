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
            // get the inputs
            Dictionary<char, List<char>> input = this.ParseInput1();
            int baseWorkTime = (_runExample) ? 0 : 60;
            int numberOfWorkers = (_runExample) ? 2 : 5;

            int timeTaken = 0;
            var jobsQueue = new List<Job>();
            int stepsToComplete = input.Keys.Count;
            var stepsCompleted = new List<char>();
            
            do {
                // loop each worker and give them a job to do!
                for (var i = 0; i < numberOfWorkers; i++) {
                    // find the next step
                    List<char> currentJobIds = jobsQueue.Select(x => x.Id).ToList();
                    char id = input.OrderBy(x => x.Key).Where(y => y.Value.Count == 0 && !currentJobIds.Contains(y.Key)).Select(z => z.Key).FirstOrDefault();
                    // check to see we have an actual job for that worker to do?
                    if (id != '\0') {
                        jobsQueue.Add(new Job(id, (baseWorkTime + this.GetStepWorkTime(id))));
                    }
                }

                // deduct a measure of time from the current jobs in the queue
                foreach (var job in jobsQueue) {
                    job.TimeRemaining--;

                    if (job.TimeRemaining <= 0) {
                        // job is finished
                        stepsCompleted.Add(job.Id);
                        // remove this step from each input
                        foreach (var i in input) {
                            i.Value.Remove(job.Id);
                        }
                        // remove from input
                        input.Remove(job.Id);
                    }
                }

                // remove all finished/dead jobs
                jobsQueue.RemoveAll(x => stepsCompleted.Contains(x.Id));
                // increase the time
                timeTaken++;
            } while (stepsCompleted.Count() < stepsToComplete);

            string answer = new string(stepsCompleted.ToArray());
            Console.WriteLine(String.Format("Part-2 Solution: {0}. Time Taken: {1}", answer, timeTaken));
        }

        private int GetStepWorkTime(char step) {
            return Math.Abs(((int)step) - 64);
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

        private class Job {

            public char Id { get; set; }

            public int TimeRemaining { get; set; }

            public Job(char id, int timeRemaining) {
                Id = id;
                TimeRemaining = timeRemaining;
            }
        }
    }
}
