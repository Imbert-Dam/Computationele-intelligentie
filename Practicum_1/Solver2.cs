using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
namespace Practicum_1
{
    public class Solver2
    {
        public Sudoku s;
        private Stack<(int,int,int,int)> st = new Stack<(int, int, int, int)>();
        private int counter = 0;
        public int backwards = 0;
        public Solver2(Sudoku sudoku)
        {/*
        Constructor requires a:
            - sudoku object
        */
            s=sudoku;
        }

        public long ChronologicalBacktracking()
        {/*
        ChronologicalBacktracking() solves a sudoku using chronological backtracking and returns 
            the amount of ticks it took.
        */
            Stopwatch watch = Stopwatch.StartNew();
            //CBT
            CBT();
            watch.Stop();
            return watch.ElapsedTicks;
        }

        public long Backtracking()
        {/*
        Backtracking() solves a sudoku using backtracking and returns 
            the amount of ticks it took.
        */
            Stopwatch watch = Stopwatch.StartNew();
            //CBT
            BT();
            watch.Stop();
            return watch.ElapsedTicks;
        }

        private bool BT()
        {/*
        BT() gets the next empty square and fills in a number ranging from 1 up untill 9 if it is
            legal in the same: row , column and box 
            through recursion it checks if this number is possible, if not it raises the number.
        */
            (int x,int y) = NextSquare(); 
            if (x == -1)
            { // base case: no square is empty
                return true;
            }              
            for (int i=1; i<10; i++)
            {
                if (!s.in_box(y,x,i)&&!s.in_column(x,i)&&!s.in_row(y,i))
                {
                    s.board[y,x] = i;
                    if (BT())
                    {
                        return true;
                    }
                    //if this line is reached there was an incorrect number filled in
                    backwards++;
                    s.board[y,x] = 0;
                }
            }
            return false;
        }

        private bool CBT()
        {/*
        CBT() gets the next empty square and fills in a number ranging from 1 up untill 9 if it is
            in the domain
            through recursion and forward checking it checks if this number is possible, 
            if not it returns all elements of the domain and it raises the number.
        */
            (int cx,int cy) = NextSquare();
            if (cx == -1)
            { // base case: no square is empty
                return true;
            }

            for (int i =1; i<10; i++)
            { //ipv 1-9 loop domein hashset/list

                if (s.domain[cy,cx].Remove(i))
                {
                    s.board[cy,cx] = i;
                    st.Push((counter,i,cy,cx));
                    if(!forwardCheck(i,cy,cx)&&CBT())
                    {
                        return true;
                    }
                    counter--;
                    backwards++;
                    s.board[cy,cx] = 0;
                    while(st.Count!=0 && st.Peek().Item1 == counter)
                    {
                        (int t,int v,int c_y,int c_x)= st.Pop();
                        s.domain[c_y,c_x].Add(v);
                    }
                    
                }
            }
            return false;

        }
        private bool forwardCheck(int i , int cy, int cx)
        { /*
        Removes i from all the domains that square cy,cx has effect on if is a different square.
        Returns false if it is still a partial solution.
        Returns true if a domain of a empty square is empty.
        */
            for(int j = 0; j<9;j++)
            {
                (int x,int y) = s.boxCoordinates(cx,cy,j);

                if(!(cy==j)&&s.domain[j,cx].Remove(i))
                {
                    st.Push((counter,i,j,cx));
                }
                if(s.domain[j,cx].Count == 0 && s.board[j,cx] == 0)
                {
                    counter++;
                    return true;
                }   
                if(!(cx==j)&&s.domain[cy,j].Remove(i))
                {
                    st.Push((counter,i,cy,j));
                }
                if(s.domain[cy,j].Count == 0 && s.board[cy,j] == 0)
                {
                    counter++;
                    return true;
                }  
                if(!((cy==y)&&(cx==x))&&s.domain[y,x].Remove(i))
                {
                    st.Push((counter,i,y,x));
                }
                if(s.domain[y,x].Count == 0 && s.board[y,x] == 0)
                {
                    counter++;
                    return true;
                }

            }
            counter++;
            return false;
            
        }

        private (int,int) NextSquare()
        {/*
        NextSquare() returns the next empty square if the sudoku is finished (-1,-1) is returned.
        */
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

        private (int,int) NextSquare2(int c_y, int c_x)
        {/*
        NextSquare() returns the next empty square if the sudoku is finished (-1,-1) is returned.
        */
            for(int x = c_x; x<9; x++)
            {   
                if(s.board[c_y,x]==0)
                    {
                        return((x,c_y));
                    }
            }
            for (int y = c_y+1; y<9; y++)
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