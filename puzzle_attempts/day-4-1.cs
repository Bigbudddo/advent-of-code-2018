public int Day { get; private set; }

            public int Month { get; private set; }

            public int Year { get; private set; }

            public int Hour { get; private set; }

            public int Minute { get; private set; }

            public int Second { get; private set; }
            
            public string Date {
                get {
                    return string.Format("{0}-{1}", Month, Day);
                }
            }

            public string Timestamp {
                get {
                    return string.Format("{0}:{1}", Hour, Minute);
                }
            }                   


				   public DateTime DateTime {
                get {
                    return new DateTime(Year, Month, Day, Hour, Minute, Second);
                }
            }
				
				string[] aDateTime = match.Value.Replace("[", "").Replace("]", "").Trim().Split(' ');
				string[] aDate = aDateTime[0].Split('-');
                string[] aTime = aDateTime[1].Split(':');

                Day = int.Parse(aDate[2]);
                Month = int.Parse(aDate[1]);
                Year = int.Parse(aDate[0]);
                Hour = int.Parse(aTime[0]);
                Minute = int.Parse(aTime[1]);
                Second = 0;

				
				
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
                Console.WriteLine("Sleep Time: " + minutesOfSleep.Minutes);

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
		
		
        private void DrawLog(List<SleepLog> logs, int? guardId = null) {
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine("Date   ID        Minute                                                        Total");
            Console.WriteLine("                 000000000011111111112222222222333333333344444444445555555555");
            Console.WriteLine("                 012345678901234567890123456789012345678901234567890123456789");

            // loop around the logs
            logs = logs.OrderBy(x => x.Timestamp).ToList();
            foreach (var log in logs) {
                if (guardId.HasValue && guardId.Value != log.GuardId)
                    continue;

                string spacing = "";
                switch (log.GuardId.ToString().Length) {
                    case 2:
                        spacing = "       ";
                        break;

                    case 3:
                        spacing = "      ";
                        break;

                    case 4:
                        spacing = "     ";
                        break;

                    default:
                        spacing = "  ";
                        break;
                }

                Console.Write(String.Format("{0}  #{1}{2}", log.Timestamp, log.GuardId, spacing));
                foreach (char e in log.Log) {
                    Console.Write(e);
                }
                Console.Write("  {0}", log.TotalTimeSlept);
                Console.WriteLine();
            }

            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine();
        }

        private int GetMinuteMostAsleep(List<SleepLog> sleepLogs) {
            int retval = 0;
            int retvalTotal = 0;
            var checkLog = new Dictionary<int, int>();
            // loop each sleep log
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
                        }
                    }
                }
            }

            return retval;
        }
		
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
                            case Action.beginsShift:
                                break;

                            case Action.fallsAsleep:
                                timeOfSleep = action.Timestamp;
                                break;

                            case Action.wakesUp:
                                sleepLog.AddSleep(timeOfSleep, action.Timestamp);
                                break;

                            default:
                                continue;
                        }
                    }

                    sleepLogs.Add(sleepLog);
                }
            }

            DrawLog(sleepLogs);

            // calculate which guard slept the most
            int totalTime = 0;
            int selectedGuard = -1;
            foreach (var guard in sleepLogs.GroupBy(x => x.GuardId)) {
                int guardSleepTime = guard.Sum(y => y.TotalTimeSlept);
                Console.WriteLine(String.Format("Guard #{0} total time spent asleep: {1}", guard.Key, guardSleepTime));
                if (guardSleepTime > totalTime) {
                    totalTime = guardSleepTime;
                    selectedGuard = guard.Key;
                }
            }

            Console.WriteLine();
            Console.WriteLine(String.Format("Guard #{0} slept the most with a total time of: {1}", selectedGuard, totalTime));
            Console.WriteLine();

            // find the minute most alseep
            int minuteMostAsleep = GetMinuteMostAsleep(sleepLogs.Where(x => x.GuardId == selectedGuard).ToList());

            Console.WriteLine(String.Format("Minute most asleep is: {0}", minuteMostAsleep));
            Console.WriteLine();

            //DrawLog(sleepLogs, selectedGuard);

            int answer = selectedGuard * minuteMostAsleep;

            Console.WriteLine(String.Format("Part-1 Solution: {0}", answer));
        }