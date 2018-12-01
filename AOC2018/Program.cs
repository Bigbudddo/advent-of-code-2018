﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2018 {

    class Program {

        static void Main(string[] args) {
            DateTime today = DateTime.Now;
            if (today.Month != 12) {
                Console.WriteLine("It's not December yet! Come back when it's time and the snow is falling!");
                Console.WriteLine();
            }
            else {
                Console.WriteLine(string.Format("Looks like today is the {0}{1}.", today.Day, ProgramHelpers.GetDateEnd(today.Day)));
                Console.Write("Checking to see if we have this puzzle implemented yet... ");

                IPuzzle puzzle = GetPuzzle(today.Day);
                if (puzzle == null) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("NOT FOUND!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Looks like you've not solved that puzzle yet. Get cracking!");
                    Console.WriteLine();
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("FOUND!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Executing puzzle...");
                    Console.WriteLine();

                    #region Puzzle 1

                    try {
                        puzzle.SolvePart1();
                    }
                    catch {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Oops! I don't think you've solved Part-1 correctly yet!");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    #endregion

                    #region Puzzle 2

                    try {
                        puzzle.SolvePart2();
                    }
                    catch {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Oops! I don't think you've solved Part-2 correctly yet!");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    #endregion

                    Console.WriteLine();
                }
            }

            Console.WriteLine("Finished.. Press return to close! ");
            Console.ReadLine();
        }

        static IPuzzle GetPuzzle(int day) {
            switch (day) {
                case 1:
                    return new Day1();
                default:
                    return null;
            }
        }
    }

    public static class ProgramHelpers {

        public static string GetDateEnd(int day) {
            if (day == 1) {
                return "st";
            }
            else if (day == 2 || day == 22) {
                return "nd";
            }
            else if (day == 3) {
                return "rd";
            }
            else {
                return "th";
            }
        }
    }
}