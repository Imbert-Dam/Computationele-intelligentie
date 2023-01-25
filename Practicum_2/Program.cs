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
        static int Max_n = 50;

        static void Main(string[] args)
        {
            string sudoku_file = "Sudoku_puzzels_.txt";
            string[] lines = File.ReadAllLines(sudoku_file);
            
            using (StreamWriter sw = new StreamWriter("Results.txt"))
                {
                    /*Following lines are used for formatting the Result.txt file*/
                        sw.WriteLine("Optimized: Unoptimized:");
                        //sw.WriteLine("CBT: BT:");
                        //sw.WriteLine("CBT: ILS:");
                    for (int i = 1; i<lines.Length;i+=2) //ignore grid lines
                    { 
                        string stripped_line = lines[i];
                        if (lines[i][0] == ' ')
                        {
                            stripped_line = lines[i].Remove(0,1);
                        }
                        /* The following line tests the speed difference between the optimized
                                and unoptimized version: */
                        SpeedTest(stripped_line,sw);
                        
                        /* The following line tests the speed difference between backtracking
                                and chronological backtracking */
                        //BTvsCBT(stripped_line,sw);
                        
                        /* The following line tests the speed difference between iterated local search
                                and chronological backtracking */
                        //CBTvsILS(stripped_line,sw);

                        /* The following lines solves sudokus with the best version: */
                        Sudoku sud = new Sudoku(stripped_line,"CBT",true);
                        Solver2 solv = new Solver2(sud);
                        solv.ChronologicalBacktrackingOptimized();
                        solv.s.printBoard();
                        // Sudoku sud = new Sudoku(stripped_line);
                        // Solver solv = new Solver(sud , S_parm, P_parm);
                        // sud.printBoard();
                        // solv.iteratedLocalSearchOptimized();
                        // solv.s.printBoard();
                        // Sudoku sud = new Sudoku(stripped_line,"CBT", true);
                        // Solver2 solv = new Solver2(sud);
                        // solv.ChronologicalBacktrackingOptimized();
                        // solv.s.printBoard();
                        // Sudoku sud2 = new Sudoku(stripped_line);
                        // Solver2 solv2 = new Solver2(sud2);
                        // solv2.BacktrackingOptimized();
                        // solv2.s.printBoard();
                        // if(solv.s.Export()!=solv2.s.Export())
                        // {
                        //     continue;
                        // }
                    }
                }
        }
        static void SpeedTest(string s, StreamWriter sw)
        {/*SpeedTest tests the speed of the unoptimized and optimized versions of the
                chronological backtracking.*/
            
            //Test with less optimized:
            Sudoku sud = new Sudoku(s,"CBT");
            Solver2 solv = new Solver2(sud);
            long unopti = solv.ChronologicalBacktracking();               
            //Test with more optimized:
            Sudoku su = new Sudoku(s,"CBT",true);
            Solver2 sol = new Solver2(su);
            long opti = sol.ChronologicalBacktrackingOptimized();

            sw.WriteLine($"{opti} {unopti}");
            

        }

        static void BTvsCBT(string s, StreamWriter sw)
        {/*SpeedTest tests the speed of the backtracking and chronologicalbacktracking */

            //Test with Chronological Backtracking:
            Sudoku sud = new Sudoku(s,"CBT",true);
            Solver2 solv = new Solver2(sud);
            long cbt = solv.ChronologicalBacktrackingOptimized();
            
            
            //Test with Backtracking:
            Sudoku su = new Sudoku(s);
            Solver2 sol = new Solver2(su);
            long bt = sol.BacktrackingOptimized();                
            

            sw.WriteLine($"{cbt} {bt} {solv.backwards} {sol.backwards}");
        }          

        static void CBTvsILS(string s, StreamWriter sw)
        { /*SpeedTest tests the speed of the unoptimized and optimized versions of the
                chronological backtracking, it is done by solving the same sudoku n times and
                then taking the average.*/
            long ils = 0;
            for (int i = 0; i<Max_n; i++)
            {
                //Test with ILS
                Stopwatch w1 = Stopwatch.StartNew();
                Sudoku sudo = new Sudoku(s);
                Solver so = new Solver(sudo,S_parm,P_parm);
                so.iteratedLocalSearchOptimized();
                w1.Stop();
                ils+=w1.ElapsedTicks;

            }

                //Test with ChronologicalBacktracking:
                Stopwatch w2 = Stopwatch.StartNew();
                Sudoku su = new Sudoku(s,"CBT",true);
                Solver2 sol = new Solver2(su);
                sol.ChronologicalBacktrackingOptimized();
                w2.Stop();
                long cbt = w2.ElapsedTicks;

            
            sw.WriteLine($"{cbt} {ils/Max_n}");
        }
        
    }
}