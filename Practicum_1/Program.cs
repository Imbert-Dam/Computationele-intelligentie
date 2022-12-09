using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace Practicum_1
{
    internal class Sudoku_solver
    {
        static void Main(string[] args)
        {
            int stopval = 10;
            int S_parm = 5;
            string sudoku_file = "Sudoku_puzzels_5.txt";
            string[] lines = File.ReadAllLines(sudoku_file);
            for (int i = 1; i<lines.Length;i+=2) //ignore grid lines
            {
                string stripped_line = lines[i];
                if (lines[i][0] == ' ')
                {
                    stripped_line = lines[i].Remove(0,1);
                }
                Stopwatch watch = Stopwatch.StartNew();
                Sudoku sud = new Sudoku(stripped_line);
                Solver solv = new Solver(sud);
                solv.fillWithRandom();
                //s.printBoard();
                solv.getBoardScore();
                //Console.WriteLine("Score: " + s.currentScore);
                int fails = 0;
                int itterations = 0;
                /*while(s.currentScore!=0)
                {
                    if(itterations>100000)
                    {
                        break;
                    }
                    if (fails == stopval)
                    {
                        fails = 0;
                        for (int j = 0; j<S_parm; j++)
                        {
                            s.RandomWalkStep();
                        }
                        s.updateBoardScore();
                    }
                    else
                    {
                        int value = s.HillClimbStep2();
                        if(value == -1)
                        {
                            fails+=1;
                        }
                        else{fails=0;}

                    }
                    itterations++;
                }*/
                int value =0;
                while(value!=-1){
                value = solv.HillClimbStep();
                solv.getBoardScore();
                Console.WriteLine(solv.s.currentScore);
                Console.WriteLine(value);
                }
                watch.Stop();
                Console.WriteLine("Spend " + watch.ElapsedMilliseconds + " Milliseconds");
                Console.WriteLine("Score: " + solv.s.currentScore);
                sud.printBoard();
                
                

            }
        }
    }
}