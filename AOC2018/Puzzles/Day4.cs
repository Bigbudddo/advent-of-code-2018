using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2018 {

    public class Day4 : IPuzzle {

        private readonly bool _runExample = false;

        public void SolvePart1() {
            List<Log> logs = this.ParseInput1();
            var sleepLogs = new List<SleepLog>();

            // search logs based on the date
            foreach (var day in logs.GroupBy(x => x.Timestamp.ToString("MM-dd"))) {
                // loop the guards within that day
                foreach (var guard in day.GroupBy(y => y.GuardId)) {
                    // now loop their actions
                    DateTime timeOfSleep = DateTime.Now;
                    var sleepLog = new SleepLog(guard.Key, day.Key);

                    foreach (var action in guard) {
                        switch(action.GuardAction) {
                            case Log.Action.beginsShift:
                                break;

                            case Log.Action.fallsAsleep:
                                timeOfSleep = action.Timestamp;
                                break;

                            case Log.Action.wakesUp:
                                sleepLog.AddSleep(timeOfSleep, action.Timestamp);
                                break;

                            default:
                                continue;
                        }
                    }

                    sleepLogs.Add(sleepLog);
                }
            }

            if (_runExample) {
                
            }

            DrawLog(sleepLogs);

            // calculate which guard slept the most
            int totalTime = 0;
            int selectedGuard = -1;
            foreach (var guard in sleepLogs.GroupBy(x => x.GuardId)) {
                int guardSleepTime = guard.Sum(y => y.TotalTimeSlept);
                if (guardSleepTime > totalTime) {
                    totalTime = guardSleepTime;
                    selectedGuard = guard.Key;
                }
            }
            
            // find the minute 
            int answer = GetMinuteMostAsleep(sleepLogs.Where(x => x.GuardId == selectedGuard).ToList(), out int guardId);
            answer = guardId * answer;

            Console.WriteLine(String.Format("Part-1 Solution: {0}", answer));
        }

        public void SolvePart2() {
            throw new NotImplementedException();
        }

        private void DrawLog(List<SleepLog> logs) {
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("Date   ID   Minute                                                        Total");
            Console.WriteLine("            000000000011111111112222222222333333333344444444445555555555");
            Console.WriteLine("            012345678901234567890123456789012345678901234567890123456789");

            // loop around the logs
            logs = logs.OrderBy(x => x.Timestamp).ToList();
            foreach (var log in logs) {
                Console.Write(String.Format("{0}  #{1}  ", log.Timestamp, log.GuardId));
                foreach (char e in log.Log) {
                    Console.Write(e);
                }
                Console.Write("  {0}", log.TotalTimeSlept);
                Console.WriteLine();
            }

            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine();
        }

        private int GetMinuteMostAsleep(List<SleepLog> sleepLogs, out int guardId) {
            int retval = 0;
            int retvalTotal = 0;
            var checkLog = new Dictionary<int, int>();
            // loop each sleep log
            guardId = -1;
            foreach (var log in sleepLogs) {
                // get the
                for (var i = 0; i < log.Log.Length; i++) {
                    if (!checkLog.ContainsKey(i)) {
                        checkLog.Add(i, 0);
                    }

                    if (log.Log[i] == SleepLog._sleepChar) {
                        checkLog[i]++;
                        if (checkLog[i] > retvalTotal) {
                            retval = i;
                            retvalTotal = checkLog[i];

                            guardId = log.GuardId; // assign the guard-id
                        }
                    }
                }
            }

            return retval;
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

            var retval = new List<Log>();
            for (var i = 0; i < records.Length; i++) {
                // get the previous guard id
                Log previousLog = (retval.Count > 0) ? retval[i - 1] : null;
                int previousGuardId = (previousLog != null) ? previousLog.GuardId : -1;

                // add & parse the record into a log
                retval.Add(new Log(records[i], previousGuardId));
            }
            
            return retval.OrderBy(x => x.Timestamp).ToList();
        }

        private class Log {

            public enum Action {
                beginsShift,
                fallsAsleep,
                wakesUp
            };

            public int GuardId { get; set; }

            public DateTime Timestamp { get; set; }

            public Action GuardAction { get; set; }

            private void ParseRecord(string record, int previousGuardId) {
                // get the timestamp first
                Match match = Regex.Match(record, @"\[(.*?)\]");
                if (!match.Success)
                    throw new Exception("could not pull the date-time value with regext!");

                // set the timestamp
                Timestamp = DateTime.Parse(match.Value.Replace("[", "").Replace("]", ""));

                // check for a time just before midnight, and assign it to the next day!
                if (Timestamp.Hour == 23 && (Timestamp.Minute >= 55 && Timestamp.Minute < 60)) {
                    Timestamp.AddDays(1);
                    Timestamp = new DateTime(Timestamp.Year, Timestamp.Month, Timestamp.Day, 0, 0, 0);
                    //Timestamp = new DateTime(Timestamp.Year, Timestamp.Month, (Timestamp.Day + 1), 0, 0, 0);
                }

                // we want to check for the guard details if they exists!
                // note; we can tell in this puzzle that there is a '#' character within the record
                record = record.Replace(match.Value, "").Replace("Guard", "").Trim();
                string[] aRecord = record.Split(' ');
                if (aRecord[0].Contains('#')) {
                    GuardId = int.Parse(aRecord[0].Replace("#", ""));
                    record = String.Join(" ", aRecord.Skip(1).ToArray()).Trim(); // re-join minus the guard ID
                }
                else {
                    GuardId = previousGuardId;
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

            public Log(string record, int previousGuardId) {
                ParseRecord(record, previousGuardId);
            }
        }

        private class SleepLog {

            public static readonly char _awakeChar = '.';
            public static readonly char _sleepChar = '#';

            public int GuardId { get; set; }

            public string Timestamp { get; set; }

            public char[] Log { get; set; }

            public int TotalTimeSlept {
                get {
                    int count = 0;

                    foreach (char x in Log) {
                        if (x == _sleepChar)
                            count++;
                    }

                    return count;
                }
            }

            public void AddSleep(DateTime start, DateTime end) {
                TimeSpan minutesOfSleep = end - start;
                //Console.WriteLine("Sleep Time: " + minutesOfSleep.Minutes);

                for (var i = start.Minute; i < end.Minute; i++) {
                    Log[i] = _sleepChar;
                }
            }

            public SleepLog(int guardId, string timestamp) {
                GuardId = guardId;
                Timestamp = timestamp;
                // sort-out our log
                Log = new char[60];
                for (var i = 0; i < Log.Length; i++) {
                    Log[i] = '.';
                }
            }
        }
    }
}
