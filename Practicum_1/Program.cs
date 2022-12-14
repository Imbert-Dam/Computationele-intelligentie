using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace Practicum_1
{
    internal class Experiments
    {
        static int S_parm = 5;
        static int P_parm = 18;
        static void Main(string[] args)
        {

            string sudoku_file = "Sudoku_puzzels_5.txt";
            string[] lines = File.ReadAllLines(sudoku_file);
            for (int i = 1; i<lines.Length;i+=2) //ignore grid lines
            {
                string stripped_line = lines[i];
                if (lines[i][0] == ' ')
                {
                    stripped_line = lines[i].Remove(0,1);
                }
                

                //InitialiseExperiment(stripped_line);
                Sudoku sud = new Sudoku(stripped_line);
                Solver solv = new Solver(sud , S_parm, P_parm);
                solv.iteratedLocalSearch();
                //sud.printBoard();
                
                

            }
        }

        static void InitialiseExperiment(string s_string)
        {
            // Test with the more "Random" initialise:
            Sudoku s = new Sudoku(s_string);
            Solver sol = new Solver(s , S_parm, P_parm);
                
            Stopwatch watch2 = Stopwatch.StartNew(); 
            sol.fillWithRandom();
            watch2.Stop();
            Console.WriteLine("More random:");
            Console.WriteLine("Spend " + watch2.ElapsedTicks + " Ticks");

            // Test with the less "Random" initialise:
            Sudoku sud = new Sudoku(s_string);
            Solver solv = new Solver(sud , S_parm, P_parm);
                
            Stopwatch watch = Stopwatch.StartNew(); 
            solv.fillSudoku();
            watch.Stop();
            Console.WriteLine("Less random:");
            Console.WriteLine("Spend " + watch.ElapsedTicks + " Ticks");

            Console.WriteLine();


        }
    }
}