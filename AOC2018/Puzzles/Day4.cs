using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2018 {

    public class Day4 : IPuzzle {

        private enum Action {
            beginsShift,
            fallsAsleep,
            wakesUp
        };

        private readonly bool _runExample = false;

        public void SolvePart1() {
            var eventLogs = this.ParseInput1();
            var parsedEventLogs = this.ParseLog(eventLogs);

            int guardId = this.FindGuardMostAsleep(parsedEventLogs, out int totalTimeSlept);
            if (guardId < 0)
                throw new Exception("could not locate guard who slept the most!");

            int minuteMostAsleep = this.FindMinuteMostAsleepForGuardId(guardId, parsedEventLogs, out int value);

            Console.WriteLine(String.Format("Part-1 Solution: {0}", (guardId * minuteMostAsleep)));
        }

        public void SolvePart2() {
            var eventLogs = this.ParseInput1();
            var parsedEventLogs = this.ParseLog(eventLogs);

            int mostFrequentMinute = this.FindGuardMostFrequentMinute(parsedEventLogs, out int guardId);

            if (guardId < 0)
                throw new Exception("could not locate guard id");

            Console.WriteLine(String.Format("Part-2 Solution: {0}", (guardId * mostFrequentMinute)));
        }

        private int FindGuardMostAsleep(Dictionary<int, Dictionary<string, int[]>> parsedEventLogs, out int totalTimeSlept) {
            int guardId = -1;
            totalTimeSlept = 0;
            
            // loop each guard
            foreach (var guardData in parsedEventLogs) {
                // loop each date that the guard was on duty
                int timeAsleepForDate = 0;
                foreach (var date in guardData.Value) {
                    // count the minutes that they were asleep
                    timeAsleepForDate += date.Value.Where(x => x == 1).Sum();
                }

                // compare with our 'highest score'
                if (timeAsleepForDate > totalTimeSlept) {
                    // assign the data
                    totalTimeSlept = timeAsleepForDate;
                    guardId = guardData.Key;
                }
            }

            return guardId;
        }

        private int FindGuardMostFrequentMinute(Dictionary<int, Dictionary<string, int[]>> parsedEventLogs, out int guardId) {
            guardId = -1;
            int guardMostSleptMinute = -1;
            int guardMostSleptMinuteCount = 0;

            // loop each guard
            foreach (var guardData in parsedEventLogs) {
                // find the minute they were most asleep
                int minuteMostAsleep = this.FindMinuteMostAsleepForGuardId(guardData.Key, parsedEventLogs, out int value);
                // compare with most asleep minute
                if (value > guardMostSleptMinuteCount) {
                    guardMostSleptMinute = minuteMostAsleep;
                    guardMostSleptMinuteCount = value;
                    guardId = guardData.Key;
                }
            }

            return guardMostSleptMinute;
        }

        private int FindMinuteMostAsleepForGuardId(int guardId, Dictionary<int, Dictionary<string, int[]>> parsedEventLogs, out int value) {
            if (!parsedEventLogs.ContainsKey(guardId))
                throw new Exception("guard does not have any logs");

            var minuteCounts = new int[60];
            // loop the guard's times
            foreach (var date in parsedEventLogs[guardId]) {
                // check each minute in our hour array
                for (var i = 0; i < date.Value.Length; i++) {
                    if (date.Value[i] > 0) {
                        // they are asleep!
                        minuteCounts[i]++;
                    }
                }
            }

            // return the max value
            value = minuteCounts.Max();
            return minuteCounts.ToList().IndexOf(value);
        }

        private List<Log> ParseInput1() {
            string[] records = null;
            if (_runExample) {
                records = new string[] {
                    "[1518-11-01 00:00] Guard #10 begins shift",
                    "[1518-11-01 00:05] falls asleep",
                    "[1518-11-01 00:25] wakes up",
                    "[1518-11-01 00:30] falls asleep",
                    "[1518-11-01 00:55] wakes up",
                    "[1518-11-01 23:58] Guard #99 begins shift",
                    "[1518-11-02 00:40] falls asleep",
                    "[1518-11-02 00:50] wakes up",
                    "[1518-11-03 00:05] Guard #10 begins shift",
                    "[1518-11-03 00:24] falls asleep",
                    "[1518-11-03 00:29] wakes up",
                    "[1518-11-04 00:02] Guard #99 begins shift",
                    "[1518-11-04 00:36] falls asleep",
                    "[1518-11-04 00:46] wakes up",
                    "[1518-11-05 00:03] Guard #99 begins shift",
                    "[1518-11-05 00:45] falls asleep",
                    "[1518-11-05 00:55] wakes up",
                };
            }
            else {
                records = File.ReadAllLines("Files/day-4-1.txt");
            }

            // parse each record into a readable log object
            var retval = new List<Log>();
            for (var i = 0; i < records.Length; i++) {
                retval.Add(new Log(records[i]));
            }

            // sort that object into the datetime
            retval = retval.OrderBy(x => x.DateTime).ToList();

            // now we need to figure out the guard id's for each action
            // note; I think this is where I fucked up,  I assigned the GuardId before ordering
            int currentGuardId = -1;
            foreach (var log in retval) {
                if (log.GuardId > 0) {
                    currentGuardId = log.GuardId;
                }
                else {
                    if (currentGuardId <= 0)
                        throw new Exception("unknown guard id? what happened?");

                    log.UpdateGuardId(currentGuardId);
                }
            }

            // return our result
            return retval;
        }
        
        private Dictionary<int, Dictionary<string, int[]>> ParseLog(List<Log> logs) {
            var retval = new Dictionary<int, Dictionary<string, int[]>>();

            // loop for each guard
            foreach (var guard in logs.GroupBy(x => x.GuardId)) {
                // loop for each day/month combo
                var dayLogs = new Dictionary<string, int[]>();
                foreach (var day in guard.GroupBy(y => y.Datestamp)) {
                    var hourMinuteLog = new int[60]; // count each minute within the midnight hour
                    // loop the actions for that day
                    int minute = -1;
                    foreach (var action in day) {
                        switch(action.GuardAction) {
                            case Action.beginsShift:
                                break;

                            case Action.fallsAsleep:
                                minute = action.Minute; // the minute he/she fell asleep
                                break;

                            case Action.wakesUp:
                                if (minute < 0)
                                    throw new Exception("unknown sequence of events! wakes up before he falls asleep?");
                                // assign the sleep time
                                for (var i = minute; i < action.Minute; i++) {
                                    hourMinuteLog[i] = 1;
                                }
                                break;
                        }
                    }
                    
                    dayLogs.Add(day.Key, hourMinuteLog);
                }

                retval.Add(guard.Key, dayLogs);
            }

            return retval;
        }

        private class Log {

            public int GuardId { get; private set; }

            public DateTime DateTime { get; private set; }

            public Action GuardAction { get; private set; }

            public int Year {
                get { return DateTime.Year; }
            }

            public int Month {
                get { return DateTime.Month; }
            }

            public int Day {
                get { return DateTime.Day; }
            }

            public int Hour {
                get { return DateTime.Hour; }
            }

            public int Minute {
                get { return DateTime.Minute; }
            }

            public int Second {
                get { return DateTime.Second; }
            }

            public string Datestamp {
                get {
                    return string.Format("{0}-{1}", Month, Day);
                }
            }

            public string Timestamp {
                get {
                    return string.Format("{0}:{1}", Hour, Minute);
                }
            }

            public void UpdateGuardId(int guardId) {
                GuardId = guardId;
            }

            private void ParseRecord(string record) {
                // get the timestamp first
                Match match = Regex.Match(record, @"\[(.*?)\]");
                if (!match.Success)
                    throw new Exception("could not pull the date-time value with regex!");

                // set the date-time
                DateTime = DateTime.Parse(match.Value.Replace("[", "").Replace("]", "").Trim());

                // we want to check for the guard details if they exists!
                // note; we can tell in this puzzle that there is a '#' character within the record
                record = record.Replace(match.Value, "").Replace("Guard", "").Trim();
                string[] aRecord = record.Split(' ');
                if (aRecord[0].Contains('#')) {
                    GuardId = int.Parse(aRecord[0].Replace("#", ""));
                    record = String.Join(" ", aRecord.Skip(1).ToArray()).Trim(); // re-join minus the guard ID
                }
                else {
                    GuardId = -1;
                }

                // now we want the action
                switch (record) {
                    case "begins shift":
                        GuardAction = Action.beginsShift;
                        break;

                    case "falls asleep":
                        GuardAction = Action.fallsAsleep;
                        break;

                    case "wakes up":
                        GuardAction = Action.wakesUp;
                        break;

                    default:
                        throw new Exception("unknown action");
                }
            }

            public Log(string record) {
                ParseRecord(record);
            }
        }
    }
}
