namespace Practicum_1
{
    public class Sudoku
    {
        public int[,] board;
        public int currentScore;
        public bool[,] unmovable;

        public Sudoku()
        { /* Generates the board of the Sudoku without given values (without input sudoku) 
                and a bool array to check whether a value in the sudoku is fixed or not. */
            board = new int[9, 9];
            unmovable = new bool [9,9];
        }

        public Sudoku(string input)
        {/* The following code converts the input string into a correct sudoku representation. 
                and a bool array to check whether a value in the sudoku is fixed or not. */
        
            board = new int[9, 9];
            unmovable = new bool [9,9];
            string[] vals = input.Split(' ');
            for (int i = 0; i < vals.Length; i++)
            {
                int temp = int.Parse(vals[i]);
                if (temp != 0)
                {
                    board[i / 9, i % 9] = temp; 
                    unmovable[i / 9, i % 9] = true;
                }
            }

            
        }

        public bool in_box(int row, int column, int v)
        {/* in_box checks whether a certain value is inside the box or not. 
                - int row: index of the row of a certain square
                - int column: index of the column of a certain square
                - int v: value of a certain square
            returns true if value is inside the same box.
        */
            // get row_idx and column_idx of the middle:
            int r = (row/3)*3+1;
            int c = (column/3)*3+1;

            for (int i = -1; i<2; i++)
            {
                for(int j = -1; j<2; j++)
                {
                    if(board[r+j,c+i]==v)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool in_row(int row,int v)
        {/* in_row checks whether a certain value is inside a row or not.  
                - int row: index of the row of a certain square
                - int v: value of a certain square
            returns true if value is inside the same row.
        */
            for(int i = 0; i<board.GetLength(0); i++)
            {
                if(board[row,i]==v)
                {
                    return true;
                }
            }
            return false;
        }

        public bool in_column(int column,int v)
        {/* in_column checks whether a certain value is inside a column or not.  
                - int column: index of the column of a certain square
                - int v: value of a certain square
            returns true if value is inside the same column.
        */
            for(int i = 0; i<board.GetLength(1); i++)
            {
                if(board[i,column]==v)
                {
                    return true;
                }
            }
            return false;
        }

        public string Export()
        {/* Export saves the Sudoku as string , can be used for debugging */
            string res = "";
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    res += board[y, x].ToString();
                    res += " ";
                }
            }
            return res;
        }

        public void printBoard()
        {/* printBoard prints the sudoku in  a easily readable way  */
            for (int y = 0; y < 9; y++)
            {
                string temp = "";
                for (int x = 0; x < 9; x++)
                {
                    temp += board[y, x].ToString();
                    if (x % 3 == 2 && x < 8)
                    {
                        temp += "|";
                    }
                }
                Console.WriteLine(temp);
                if (y % 3 == 2 && y < 8)
                {
                    Console.WriteLine("---+---+---");
                }
            }
            Console.WriteLine();
        }
    }
}

