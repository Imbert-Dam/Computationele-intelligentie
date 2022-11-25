using System;

namespace Practicum_1
{
    internal class Sudoku_solver
    {
        static void Main(string[] args)
        {
            string sudoku_file = "Sudoku_puzzels_5.txt";
            int rows = 9;
            int collumns = 9;
            int boxes = 9;
            string[] lines = File.ReadAllLines(sudoku_file);
            for (int i = 1; i<lines.Length;i+=2)
            {
                string stripped_line = lines[i];
                if (lines[i][0] == ' ')
                {
                    stripped_line = lines[i].Remove(0,1);
                }
                string [] input = stripped_line.Split(" ");
                int teller = 0;
                Vakje[,] s = new Vakje[rows,collumns];
                for(int r = 0; r<rows;r++)
                {
                    for(int c = 0; c<collumns;c++)
                    {
                        s[r,c] = new Vakje (int.Parse(input[teller]),r,c);
                        teller++;
                    }
                }
                Sudoku sud = new Sudoku(s);
                sud.sud_initialise(boxes,rows,collumns);
                print_sudoku(sud);
                Console.WriteLine();
            }

        }

        static void print_sudoku(Sudoku sud)
        {
            for(int r = 0; r<sud.sudoku.GetLength(0);r++)
            {
                for(int c = 0; c< sud.sudoku.GetLength(1);c++)
                {
                    Console.Write(sud.sudoku[r,c].value);
                }
                Console.WriteLine();
            }
        }
    }

    class Sudoku
    {
        public Vakje [,] sudoku;
        public Sudoku(Vakje [,] s)
        {
            sudoku = s;
        }
        bool in_box(Vakje n, int v)
        {
            //get row and collumn of the middle:
            int r = (n.row/3)*3+1;
            int c = (n.collumn/3)*3+1;

            for (int i = -1; i<2; i++)
            {
                for(int j = -1; j<2; j++)
                {
                    if(sudoku[r+j,c+i].value==v)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool in_row(Vakje n,int v)
        {
            for(int i = 0; i<sudoku.GetLength(0); i++)
            {
                if(sudoku[n.row,i].value==v)
                {
                    return true;
                }
            }
            return false;
        }

        bool in_collumn(Vakje n,int v)
        {
            for(int i = 0; i<sudoku.GetLength(1); i++)
            {
                if(sudoku[i,n.collumn].value==v)
                {
                    return true;
                }
            }
            return false;
        }

        public void sud_initialise (int b , int row, int col)
        {
            for(int hb = 0; hb<b/3;hb++)                            //horizontale boxes 0-2
            {
                for(int vb = 0; vb<b/3;vb++)                        //verticale boxes 0-2
                {
                    int value = 1;                                  //reset value wnr nieuwe box wordt bezocht
                    for(int r = 0; r<row/3;r++)                     //visit row 0-2
                    {               
                        for (int c =0; c<col/3;c++)                 //visit collumn 0-2
                        {
                            if (sudoku[r+hb*3,c+vb*3].value == 0)   //multiply rows and collumns by box number
                            {
                                while(in_box(sudoku[r+hb*3,c+vb*3],value)) //add until a new not used value is found
                                {
                                    value++;
                                }
                                sudoku[r+hb*3,c+vb*3].value = value;
                                value++; 
                            }
                        }
                    }

                }

            }
        }

    }

    class Vakje
    {
        public int value;
        public int row;
        public int collumn;
        public bool beweegbaar;


        public Vakje(int v, int r , int c)
        {
            value = v;
            row = r;
            collumn = c;
            if (v==0)
            {
                beweegbaar = true;
            }

        }
    }
}