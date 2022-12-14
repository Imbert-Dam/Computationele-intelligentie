namespace Practicum_1
{
    public class Sudoku
    {
        public int[,] board;
        public int currentScore;
        public bool[,] unmovable;

        public Sudoku()
        {
            /* Generates Sudoku without given values */
            board = new int[9, 9];
            unmovable = new bool [9,9];
        }

        public Sudoku(string input)
        {
            /* Generates given Sudoku */
            board = new int[9, 9];
            unmovable = new bool [9,9];
            string[] vals = input.Split(' ');
            for (int i = 0; i < vals.Length; i++)
            {
                int temp = int.Parse(vals[i]);
                if (temp != 0)
                {
                    board[i / 9, i % 9] = temp; //swap om het per rij in te lezen en niet per kollom
                    unmovable[i / 9, i % 9] = true;
                }
            }

            
        }

        public bool in_box(int row, int collumn, int v)
        {
            //get row and collumn of the middle:
            int r = (row/3)*3+1;
            int c = (collumn/3)*3+1;

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
        {
            for(int i = 0; i<board.GetLength(0); i++)
            {
                if(board[row,i]==v)
                {
                    return true;
                }
            }
            return false;
        }

        public bool in_collumn(int collumn,int v)
        {
            for(int i = 0; i<board.GetLength(1); i++)
            {
                if(board[i,collumn]==v)
                {
                    return true;
                }
            }
            return false;
        }

        public string Export()
        {
            /* Save Sudoku as string */
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
        {
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

