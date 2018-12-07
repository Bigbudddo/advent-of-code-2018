918 : too high
917 : correct

public void SolvePart2() {
	// get the inputs
	Dictionary<char, List<char>> input = this.ParseInput1();
	int baseWorkTime = (_runExample) ? 0 : 60;
	int numberOfWorkers = (_runExample) ? 2 : 5;

	int timeTaken = 0;
	int stepsToComplete = input.Keys.Count;
	var stepsCompleted = new List<char>();

	do {
		// loop each worker and give them a job to do!
		var listOfJobs = new List<char>();
		for (var i = 0; i < numberOfWorkers; i++) {
			// find the next step
			char id = input.OrderBy(x => x.Key).Where(y => y.Value.Count == 0 && !listOfJobs.Contains(y.Key)).Select(z => z.Key).FirstOrDefault();
			// check to see we have an actual job for that worker to do?
			if (id != '\0') {
				listOfJobs.Add(id);
			}
		}

		Console.WriteLine("We have {0} jobs this time", listOfJobs.Count());

		// loop each job that we can do at this point
		foreach (var job in listOfJobs) {
			stepsCompleted.Add(job);

			foreach (var i in input) {
				i.Value.Remove(job);
			}

			input.Remove(job);
			timeTaken += (this.GetStepWorkTime(job) + baseWorkTime);
		}
	} while (stepsCompleted.Count() < stepsToComplete);

	string answer = new string(stepsCompleted.ToArray());
	Console.WriteLine(String.Format("Part-2 Solution: {0}. Time Taken: {1}", answer, timeTaken));
}