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
            int S_parm = 5;
            int P_parm = 18;
            string sudoku_file = "Sudoku_puzzels_5.txt";
            string[] lines = File.ReadAllLines(sudoku_file);
            for (int i = 1; i<lines.Length;i+=2) //ignore grid lines
            {
                string stripped_line = lines[i];
                if (lines[i][0] == ' ')
                {
                    stripped_line = lines[i].Remove(0,1);
                }
                Sudoku sud = new Sudoku(stripped_line);
                Solver solv = new Solver(sud , S_parm, P_parm);
                //solv.sud_initialise();
                //sud.printBoard();
                solv.iteratedLocalSearch();
                //sud.printBoard();
                
                

            }
        }
    }
}