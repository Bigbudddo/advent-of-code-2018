W: 108196
W: 42603
W: 3788
R: 76357 - W:64485
R: 76357 - R:41668


private Dictionary<int, Dictionary<string, int[]>> ParseInput1() {
	string[] aRecords = null;
	var eventRecords = new Dictionary<int, Dictionary<string, int[]>>();

	// get our input based on if we are running the example or not
	if (_runExample) {
		aRecords = new string[] {
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
		aRecords = File.ReadAllLines("Files/day-4-1.txt");
	}

	// parse this input into a collection
	foreach (string record in aRecords) {
		// get the datetime
		Match match = Regex.Match(record, @"\[(.*?)\]");
		if (!match.Success)
			throw new Exception("could not pull the date-time value with regex!");

		string timestamp = match.Value.Replace("[", "").Replace("]", "").Trim();
		string[] aTimestamp = timestamp.Split('-'); // should be length of 3

		if (aTimestamp.Length != 3)
			throw new Exception("date-time not what I expected boss!");

		// final part!
		string guardTimestamp = String.Format("{0}-{1}", aTimestamp[1], aTimestamp[2]);

		// get the guard id
		string guardDetails = record.Replace(match.Value, "").Replace("Guard", "").Trim();


	}


	return eventRecords;

}



                //// loop each date that the guard was on duty
                //var minutesAsleepCounter = new int[60];
                //foreach (var date in guardData.Value) {
                //    // loop through all the minutes within the hour and count up
                //    for (var i = 0; i < date.Value.Length; i++) {
                //        if (date.Value[i] > 0) {
                //            // they are asleep
                //            minutesAsleepCounter[i]++;
                //        }
                //    }
                //}

                //// compare with our 'highest score'
                //int maxValue = minutesAsleepCounter.Max();
                //int maxCount = minutesAsleepCounter.Count(x => x == maxValue);
                //int indexMinute = minutesAsleepCounter.ToList().IndexOf(minutesAsleepCounter.Max());
                //if (indexMinute > minuteMostAsleep) {
                //    // assign the data
                //    minuteMostAsleep = indexMinute;
                //    guardId = guardData.Key;
                //}