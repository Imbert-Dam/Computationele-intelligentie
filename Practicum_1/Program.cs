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

        //static string temp1 = "";
        //static string temp2 = "";
        static void Main(string[] args)
        {
            int[] S_Array = {1,2,3,4,5,6};
            int[] P_Array = {18};
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
                parmTuning(S_Array,P_Array,stripped_line);
                // Sudoku sud = new Sudoku(stripped_line);
                // Solver solv = new Solver(sud , S_parm, P_parm);
                // solv.iteratedLocalSearch();
                //sud.printBoard();
                
                

            }
            //Console.WriteLine(temp1);
            //Console.WriteLine(temp2);
        }

        static void InitialiseExperiment(string s_string)
        {/* InitialiseExperiment reports the difference between two ways of filling in a sudoku. */
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

        static void parmTuning(int[] S_arr, int[] P_arr , string sud_string)
        {/* parmTuning takes 2 arrays of values of parameters and a sudoku as a string 
                and tests all value combinations.
                - int[] S_arr: array with values of S
                - int[] P_arr: array with values of P
                - string sud_string: string which represents a sudoku.
        */  
        
            int counter = 1;
            foreach (int P in P_arr)
            {
                foreach(int S in S_arr)
                {
                    Sudoku sud = new Sudoku(sud_string);

                    Sudoku sud2 = new Sudoku(sud);
            
                    Solver solv = new Solver(sud , S, P);
                    Console.WriteLine($"Experiment {counter} with S: {S} and P: {P}");

                    Console.WriteLine($"Optimized");
                    Solver solv2 = new Solver(sud2, S, P);

                    solv2.iteratedLocalSearchOptimized();
                    //temp1 += solv2.iteratedLocalSearchOptimized().ToString();
                    //temp1 += ",";

                    Console.WriteLine($"Pre-optimization");
                    solv.iteratedLocalSearch();
                    //temp2 += solv.iteratedLocalSearch().ToString();
                    //temp2 += ",";

                    Console.WriteLine();
                    
                    counter++;
                }
            }
            //temp1 += "\n";
            //temp2 += "\n";
        }
        
    }
}