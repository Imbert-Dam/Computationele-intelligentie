using System.Collections;

namespace Practicum_1
{
    public class Sudoku
    {
        public int[,] board;
        public int currentScore;
        public bool[,] unmovable;
        public HashSet<int>[,] domain;

        public uint[,] domainBA;

        public (int,int) nextEmpty;
        public Sudoku()
        { /* Generates the board of the Sudoku without given values (without input sudoku) 
                and a bool array to check whether a value in the sudoku is fixed or not. */
            board = new int[9, 9];
            unmovable = new bool [9,9];
        }

        public Sudoku(string input, string solver = "ILS", bool useIntRepr = false)
        {/* The following code converts the input string into a correct sudoku representation. 
                and a bool array to check whether a value in the sudoku is fixed or not. 
                
                if useIntRepr is true the sudoke will be initialized using uints to represent domain instead of HashSets
                where the last 9 bits of the uint are used to store whether a number in in the domain.
                so the domain [3,4,9] would be
                1_0000_1100
                */
            board = new int[9, 9];
            if(solver == "ILS")
            {
                unmovable = new bool [9,9];
            }
            else if(solver == "CBT")
            {
                if (useIntRepr) {
                    domainBA = new uint[9,9];
                }
                else {
                    domain = new HashSet<int>[9,9];
                }
            }
            string[] vals = input.Split(' ');
            for (int i = 0; i < vals.Length; i++)
            {
                int value = int.Parse(vals[i]);
                int row = i / 9;
                int column = i%9;

                if (value != 0)
                {
                    board[row, column] = value; 
                    if (solver == "ILS")
                    {
                        unmovable[row , column] = true;
                    }
                    else if(solver == "CBT")
                    {
                        if (useIntRepr) {
                            nodeConsistencyIntRepr(row, column, value);
                        }
                        else {
                            nodeConsistency(row , column ,value);
                        }
                    }
                }
                else if (solver =="CBT")
                {
                    if (useIntRepr) {
                        if (domainBA[row,column] == 0) {
                            domainBA[row,column] = 0b1_1111_1111;
                        }
                    }
                    else {
                        if (domain[row,column] == null) {
                             domain[row,column] = new HashSet<int>{1,2,3,4,5,6,7,8,9};
                        }
                    }
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

        private void nodeConsistency(int row, int column, int value)
        {/*
        nodeConsistency(int row, int column, int value) takes as input the coördinates of a square 
            and its containing value. It loops over all other squares in its row column 
            and box and if the domain is empty of a neighbor a new domain is created. 
            Then the value of the original square is removed from the domains.
        */
            for(int j = 0; j<9;j++)
            {
                (int x,int y) = boxCoordinates(column,row,j);
                if(domain[row,j] == null )
                {
                    domain[row,j] = new HashSet<int>{1,2,3,4,5,6,7,8,9};
                }
                if(domain[j,column] == null )
                {
                    domain[j,column] = new HashSet<int>{1,2,3,4,5,6,7,8,9};   
                }
                if (domain[y,x] == null )
                {
                    domain[y,x] = new HashSet<int>{1,2,3,4,5,6,7,8,9};
                }
                domain[j,column].Remove(value);
                domain[row,j].Remove(value);
                domain[y,x].Remove(value);
            }
        }

        private void nodeConsistencyIntRepr(int row, int column, int value)
        {/*
        nodeConsistency(int row, int column, int value) takes as input the coördinates of a square 
            and its containing value. It loops over all other squares in its row column 
            and box and if the domain is empty of a neighbor a new domain is created. 
            Then the value of the original square is removed from the domains.

            This version stores domains as uints
        */
            for(int j = 0; j<9;j++)
            {
                (int x,int y) = boxCoordinates(column,row,j);
                if(domainBA[row,j] == 0 )
                {
                    domainBA[row,j] = 0b1_1111_1111;
                }
                if(domainBA[j,column] == 0 )
                {
                    domainBA[j,column] = 0b1_1111_1111;   
                }
                if (domainBA[y,x] == 0 )
                {
                    domainBA[y,x] = 0b1_1111_1111;
                }
                RemoveIntRepr(j,column,value);
                RemoveIntRepr(row,j,value);
                RemoveIntRepr(y,x,value);
            }
        }

        public (int,int) boxCoordinates(int cx, int cy , int j)
        {/* 
        boxCoordinates(int cx, int cy , int j) maps the integer j (ranging from 0-8)
            and the coordinates of the original square to a location in its box.
            0|1|2
            -----
            3|4|5
            -----
            6|7|8
            It returns the coördinates of the square in comparison to the board. 
         */
            int y = (cy/3)*3 + j/3;
            int x = (cx/3)*3 + j%3;
            return(x,y);
        } 
        public void setToNonZero(int cy, int cx, int j){
            /*
                sets a value to a nonzero value, then updates the nextEmpty variable.
            */

            //This check was always true therefor not needed
            //bool temp = false;
            //if (board[cy,cx] == 0){
            //    temp = true;
            //}
            board[cy,cx] = j;
            //if (temp) {
            findNextEmpty(cy, cx+1);
            //}
        }

        public void findNextEmpty(int cy, int cx){
            /*
                finds the next empty slot starting the search from cy,cx. And stores the resulting value in the nextEmpty variable
            */
            if (cx > 8) {
                cx = 0;
                cy++;
            }
            if (cy > 8) {
                nextEmpty = (-1,-1);
                return;
            }
            if (board[cy,cx] == 0){
                nextEmpty = (cx,cy);
            }
            else{
                findNextEmpty(cy, cx+1);
            }
        }

        public void setToZero(int cy, int cx){
            /*
                sets a location on the board to 0, then checks if it needs to update nextEmpty
            */
            board[cy,cx] = 0;
            if (cy < nextEmpty.Item1) {
                nextEmpty = (cy,cx);
            }
            else if (cy == nextEmpty.Item1 && cx < nextEmpty.Item2) {
                nextEmpty = (cy,cx);
            }
        }

        public bool RemoveIntRepr(int cy, int cx, int i){
            /*
                removes a value from the domain of the int representation, return whether that value was there before
            */

            // (uint) 1 << i-1) gets a number with only the bit of the number we're removing set to 1.
            uint bitToRemove = (uint) 1 << i-1;
            // domainBA[cy,cx] & that number will have all 0's besides the number we're removing, which will be 1 if it was in the domain.

            if ((domainBA[cy,cx] & bitToRemove) == 0){
                //number was not present in domain
                return false;
            }
            //since the bit of the the number we're removing is always 1 here, xor'ing it will set it to 0.
            domainBA[cy,cx] = domainBA[cy,cx] ^ bitToRemove;
            return true;
        }

        public void AddIntRepr(int cy, int cx, int i){
            /*
                adds a value to the domain of the int representation.
            */

            // (uint) 1 << i-1) gets a number with only the bit of the number we're adding set to 1.
            // or'ing this with the domain will set that bit to 1.
            domainBA[cy,cx] = domainBA[cy,cx] | ((uint)1 << i-1);
        }

        public bool IsEmptyIntRepr(int cy, int cx){
            /*
                checks if the domain of a space is empty
            */

            //the domain is empty if all the bits are 0, which means the number is 0.
            return domainBA[cy,cx] == 0;
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

