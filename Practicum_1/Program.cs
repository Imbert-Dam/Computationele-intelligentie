using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace Practicum_1
{
    internal class Experiments
    {
        static int S_parm = 6;
        static int P_parm = 14;
        static int Max_n = 60;

        static void Main(string[] args)
        {
            int t=1;
            int[] S_Array = {1,3,4,5,6,7,8,9,15};
            int[] P_Array = {3,5,10,12,13,14,18,24};
            string sudoku_file = "Sudoku_puzzels_5.txt";
            string[] lines = File.ReadAllLines(sudoku_file);
            
            using (StreamWriter sw = new StreamWriter("Results.txt"))
                {
                    /*Following lines are used for formatting the Result.txt file*/
                        //sw.WriteLine("Sudoku: Experiment: S: P: Ticks:");        //parmtune
                        //sw.WriteLine("FillSudoku: FillRandom:");                 //Initialisetest 
                        //sw.WriteLine("Optimized: Unoptimized:");                 //Optimizationtest
                    for (int i = 1; i<lines.Length;i+=2) //ignore grid lines
                    { 
                        string stripped_line = lines[i];
                        if (lines[i][0] == ' ')
                        {
                            stripped_line = lines[i].Remove(0,1);
                        }
                        /* The following line runs experiment for testing the methods
                            of initialising: */
                        //InitialiseExperiment(stripped_line,sw);

                        /* The following lines tests different combinations of parameters: */
                        //parmTuning(S_Array,P_Array,stripped_line,sw,t);
                        //Console.WriteLine($"Sudoku {t}");
                        //t++;

                        /* The following line tests the speed difference between the optimized
                                and unoptimized version: */
                        //SpeedTest(stripped_line,sw);

                        /* The following lines solves sudokus with the best results: */
                        Sudoku sud = new Sudoku(stripped_line);
                        Solver solv = new Solver(sud , S_parm, P_parm);
                        //sud.printBoard();
                        solv.iteratedLocalSearchOptimized();
                        solv.s.printBoard();
                        Sudoku sud2 = new Sudoku(stripped_line,"CBT");
                        Solver2 solv2 = new Solver2(sud2);
                        solv2.ChronologicalBacktracking();
                        solv2.s.printBoard();

                    }
                

                }
        }
        static void InitialiseExperiment(string s_string,StreamWriter sw)
        {/* InitialiseExperiment reports the difference between two ways of filling in a sudoku. */
            // Test with the more "Random" initialise:
            long ticks_r=0;
            long ticks_f=0;
            for(int i = 0;i<Max_n ;i++)
            {
                // Test with the less "Random" initialise:
                Sudoku sud = new Sudoku(s_string);
                Solver solv = new Solver(sud , S_parm, P_parm);

                Stopwatch watch = Stopwatch.StartNew(); 
                solv.fillSudoku();
                watch.Stop();
                ticks_f+=watch.Elapsed.Ticks;


                // Test with the more "Random" initialise
                Sudoku s = new Sudoku(s_string);
                Solver sol = new Solver(s , S_parm, P_parm);
                
                Stopwatch watch2 = Stopwatch.StartNew(); 
                sol.fillWithRandom();
                watch2.Stop();
                ticks_r+=watch2.Elapsed.Ticks;
            }
            sw.WriteLine($"{ticks_f} {ticks_r}");
        }

        static void SpeedTest(string s, StreamWriter sw)
        { /*SpeedTest tests the speed of the unoptimized and optimized versions of the
                iterated local search, it is done by solving the same sudoku n times and
                then taking the average.*/
            long unopti = 0;
            long opti = 0;
            for (int i = 0; i<Max_n; i++)
            {
                //Test with less optimized:
                Sudoku sud = new Sudoku(s);
                Solver solv = new Solver(sud , S_parm, P_parm);
                unopti += solv.iteratedLocalSearch();

                //Test with more optimized:
                Sudoku su = new Sudoku(s);
                Solver sol = new Solver(su , S_parm, P_parm);
                opti += sol.iteratedLocalSearchOptimized();
            }
            sw.WriteLine($"{opti/Max_n} {unopti/Max_n}");
        }

        static void parmTuning(int[] S_arr, int[] P_arr , string sud_string , StreamWriter sw , int sudoku)
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
                    long ticks = 0;
                    for(int n = 0; n<Max_n; n++)
                    {   

                        Sudoku sud = new Sudoku(sud_string);
                        Solver solv = new Solver(sud , S, P);
                        long t = solv.iteratedLocalSearchOptimized();
                        ticks+=t;
                    }
                    sw.WriteLine($"{sudoku} {counter} {S} {P} {ticks/Max_n}");
                    //Console.WriteLine(counter); //used for checking progress
                    counter++;
                }
            }
        }
        
    }
}