            // look for 'finite' points
            var retval = new Dictionary<char, int>();
            var test = new List<Tuple<int, int, char, bool>>();
            for (int i = 0; i < rowLength; i++) {
                for (int j = 0; j < colLength; j++) {
                    if (board[i, j] == '.')
                        continue;
                    try {
                        bool isFinite = true;
                        // check for outside the walls of our array
                        // automatic is set to infinity
                        if (i < 1 || i > rowLength || j < 1 || j > colLength) {
                            test.Add(new Tuple<int, int, char, bool>(i, j, board[i, j], false));
                            continue;
                        }

                        // check north
                        if (board[i, j] == board[i, j - 1] || board[i, j - 1] == '.') {
                            isFinite = false;
                        }
                        else {
                            isFinite = true;
                        }

                        // check south
                        if (board[i, j] == board[i, j + 1] || board[i, j + 1] == '.') {
                            isFinite = false;
                        }
                        else {
                            isFinite = true;
                        }

                        // check east
                        if (board[i, j] == board[i + 1, j] || board[i + 1, j] == '.') {
                            isFinite = false;
                        }
                        else {
                            isFinite = true;
                        }

                        // check west
                        if (board[i, j] == board[i - 1, j] || board[i - 1, j] == '.') {
                            isFinite = false;
                        }
                        else {
                            isFinite = true;
                        }

                        test.Add(new Tuple<int, int, char, bool>(i, j, board[i, j], isFinite));
                    }
                    catch {
                        test.Add(new Tuple<int, int, char, bool>(i, j, board[i, j], false));
                    }
                }
            }

            var test2 = test.Where(x => x.Item4 == true).GroupBy(y => y.Item3);
            var test3 = new Dictionary<char, int>();
            foreach (var x in test.GroupBy(y => y.Item3)) {
                Console.WriteLine(x.Key);
            }

            var test4 = test.GroupBy(x => x.Item3).Select(y => new { Id = y.Key, Values = y.Select(z => z.Item4) }).ToList();
			
			
			
			
			        private Dictionary<char, int> GetFiniteClaims(char[,] board) {
            var retval = new Dictionary<char, int>();

            int rowLength = board.GetLength(0);
            int colLength = board.GetLength(1);

            for (int i = 0; i < rowLength; i++) {
                for (int j = 0; j < colLength; j++) {
                    // check if the current character is finite
                    if (IsFinite(board, i, j)) {
                        char id = board[i, j];
                        if (!retval.ContainsKey(id)) {
                            retval.Add(id, 1);
                        }
                        else {
                            retval[id]++;
                        }
                    }
                }
            }

            return retval;
        }
		
		
		        private bool IsFinite(char[,] board, int x, int y) {
            if (x < 1 || x >= board.GetLength(0))
                return false;
            if (y < 1 || y >= board.GetLength(1))
                return false;

            // check north

            // check south

            // check west

            // check east

            return true;
        }
