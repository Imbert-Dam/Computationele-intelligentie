using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
namespace Practicum_1
{
    public class Solver2
    {
        public Sudoku s;
        public Solver2(Sudoku sudoku)
        {
            s=sudoku;
        }

        public long ChronologicalBacktracking()
        {
            Stopwatch watch = Stopwatch.StartNew();
            //Reductie Knoopconsitentie 1x of bij elke itteratie??

            //CBT
            CBT();
            watch.Stop();
            return watch.ElapsedTicks;
        }

        private bool CBT()
        {
            (int x,int y) = NextSquare(); 
            if (x == -1)
            { //base case
                return true;
            }              
            for (int i=1; i<10; i++)
            {
                if (!s.in_box(y,x,i)&&!s.in_column(x,i)&&!s.in_row(y,i))
                {
                    s.board[y,x] = i;
                    if (CBT())
                    {
                        return true;
                    }
                    //nu is ie incorrect:
                    s.board[y,x] = 0;
                }
            }
            return false;
        }

        private (int,int) NextSquare()
        {
            for (int y = 0; y<9; y++)
            {
                for (int x = 0; x<9; x++)
                {
                    if(s.board[y,x]==0)
                    {
                        return((x,y));
                    }
                }
            }
            return (-1,-1); //complete sudoku

            
        }
    }
}