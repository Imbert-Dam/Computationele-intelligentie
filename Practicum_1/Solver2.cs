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
        public Solver2(Sudoku sudoku)
        {
            s=sudoku;
        }

        public long ChronologicalBacktracking()
        {
            Stopwatch watch = Stopwatch.StartNew();
            //CBT
            CBT();
            watch.Stop();
            return watch.ElapsedTicks;
        }

        public long Backtracking()
        {
            Stopwatch watch = Stopwatch.StartNew();
            //CBT
            BT();
            watch.Stop();
            return watch.ElapsedTicks;
        }

        private bool BT()
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
                    if (BT())
                    {
                        return true;
                    }
                    //nu is ie incorrect:
                    s.board[y,x] = 0;
                }
            }
            return false;
        }

        private bool CBT()
        {
            (int cx,int cy) = NextSquare();
            if (cx == -1)
            { //base case
                return true;
            }

            for (int i =1; i<10; i++)
            {

                if (s.domain[cy,cx].Remove(i))
                {
                    s.board[cy,cx] = i;
                    st.Push((counter,i,cy,cx));
                    bool empty = false;
                    for(int j = 0; j<9;j++)
                    {
                        int by = cy/3;
                        int bx = cx/3;
                        int y = by*3 + j/3;
                        int x = bx*3 + j%3;

                        
                        if(!(cy==j)&&s.domain[j,cx].Remove(i))
                        {
                            st.Push((counter,i,j,cx));
                        }
                        if(s.domain[j,cx].Count == 0 && s.board[j,cx] == 0)
                        {
                            //counter--;
                            //s.board[cy,cx] = 0;
                            empty = true;
                            break;
                            // return false;
                        }   
                        if(!(cx==j)&&s.domain[cy,j].Remove(i))
                        {
                            st.Push((counter,i,cy,j));
                        }
                        if(s.domain[cy,j].Count == 0 && s.board[cy,j] == 0)
                        {
                            //counter--;
                            //s.board[cy,cx] = 0;
                            empty = true;
                            break;
                            // return false;
                        }  
                        if(!((cy==y)&&(cx==x))&&s.domain[y,x].Remove(i))
                        {
                            st.Push((counter,i,y,x));
                        }
                        if(s.domain[y,x].Count == 0 && s.board[y,x] == 0)
                        {
                            //counter--;
                            //s.board[cy,cx] = 0;
                            empty =true;
                            break;
                            // return false;
                        }

                    }
                    counter++;
                    if(!empty&&CBT())
                    {
                        return true;
                    }
                    counter--;
                    s.board[cy,cx] = 0;
                    if(st.Count!=0)
                    {
                        (int t,_, _, _) = st.Peek();
                        while(t == counter)
                        {
                            (t,int v,int c_y,int c_x)= st.Pop();
                            s.domain[c_y,c_x].Add(v);
                            if(!(st.Count!=0))
                            {
                                break;
                            }
                            (t,_, _, _) = st.Peek();

                            
                        }
                    }
                    
                    
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