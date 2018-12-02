public void SolvePart2() {
	bool found = false;
	string differenceValue = "--",
		differenceA = "--",
		differenceB = "--",
		answerA = "Not solved yet!", 
		answerB = "Not solved yet!";
	string[] boxIds =  this.ParseInput2();

	// loop around the input
	// note; again this is probably a slower solution, but it works!
	for (var i = 0; i < boxIds.Length - 1; i++) {
		string a = boxIds[i];
		// loop around the inputs after the selected one
		for (var j = (i + 1); j < boxIds.Length; j++) {
			string b = boxIds[j];
			// begin compare
			var differences = new List<char>();
			for (var k = 0; k < a.Length; k++) {
				if (a[k] != b[k]) {
					differences.Add(a[k]);
				}
			}
			// check our difference count
			if (differences.Count == 1) {
				found = true;
				differenceValue = differences[0].ToString();
				differenceA = a;
				differenceB = b;
				answerA = a.Replace(differences[0].ToString(), "");
				answerB = b.Replace(differences[0].ToString(), "");
			}

			//if (found)
			//    break;
		}

		//if (found)
		//    break;
	}

	Console.WriteLine(String.Format("Part-2 Solution: {0}", differenceA));
	Console.WriteLine(String.Format("Part-2 Solution: {0}", differenceB));
	Console.WriteLine(String.Format("Part-2 Solution: {0}", differenceValue));
	Console.WriteLine(String.Format("Part-2 Solution: {0}", answerA));
	Console.WriteLine(string.Format("Part-2 Solution: {0}", answerB));
}