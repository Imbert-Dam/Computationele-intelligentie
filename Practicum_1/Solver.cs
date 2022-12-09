namespace Practicum_1
{
    public class Solver
    {
        public Sudoku s;
        Random r;

        public Solver(Sudoku sudoku)
        {
            s = sudoku;
            r = new Random();
        }

        public void fillWithRandom()
        {
            for (int qY = 0; qY < 3; qY++)
            {
                for (int qX = 0; qX < 3; qX++)
                {
                    List<int> bag = new List<int>();

                    for (int i = 1; i < 10; i++)
                    {
                        bag.Add(i);
                    }

                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if (s.board[qY*3+y, qX*3+x] != 0)
                            {
                                bag.Remove(s.board[qY * 3 + y, qX * 3 + x]);
                            }
                        }
                    }


                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if (s.board[qY * 3 + y, qX * 3 + x] == 0)
                            {
                                int temp = r.Next(bag.Count);
                                s.board[qY * 3 + y, qX * 3 + x] = bag[temp];
                                bag.RemoveAt(temp);
                            }
                        }
                    }
                }
            }
        }
        // public void sud_initialise (int b , int row, int col)
        // {
        //     for(int hb = 0; hb<b/3;hb++)                            //horizontale boxes 0-2
        //     {
        //         for(int vb = 0; vb<b/3;vb++)                        //verticale boxes 0-2
        //         {
        //             int value = 1;                                  //reset value wnr nieuwe box wordt bezocht
        //             for(int r = 0; r<row/3;r++)                     //visit row 0-2
        //             {               
        //                 for (int c =0; c<col/3;c++)                 //visit collumn 0-2
        //                 {
        //                     if (sudoku[r+hb*3,c+vb*3].value == 0)   //multiply rows and collumns by box number
        //                     {
        //                         while(in_box(sudoku[r+hb*3,c+vb*3],value)) //add until a new not used value is found
        //                         {
        //                             value++;
        //                         }
        //                         sudoku[r+hb*3,c+vb*3].value = value;
        //                         value++; 
        //                     }
        //                 }
        //             }

        //         }

        //     }
        // }

        public void Swap(int x1, int y1, int x2, int y2)
        {
            (s.board[y1,x1], s.board[y2,x2]) = (s.board[y2, x2], s.board[y1, x1]);
        }

        public int getBoardScore() //kunnen we hier niet beter direct currentscore updaten
        {
            int res = 0;
            for (int i =0; i<9; i++)
            {
                res += 9 - valsInColumn(i).Count;
                res += 9 - valsInRow(i).Count;
            }
            s.currentScore = res;
            return res;
        }
        public HashSet<int> valsInColumn(int x, int excludeIndex = 10)
        {
            HashSet<int> res = new HashSet<int>();
            for (int y =0; y<9; y++)
            {
                if (y != excludeIndex)
                {
                    res.Add(s.board[y, x]);
                }
            }
            return res;
        }

        public HashSet<int> valsInRow(int y, int excludeIndex = 10)
        {
            HashSet<int> res = new HashSet<int>();
            for (int x = 0; x < 9; x++)
            {
                if (x != excludeIndex)
                {
                    res.Add(s.board[y, x]);
                }
            }
            return res;
        }

        public void RandomWalkStep()
        {
            //Choose 3x3 section
            int qX = r.Next(3);
            int qY = r.Next(3);

            //Choose squares
            int x1 = r.Next(3);
            int y1 = r.Next(3);
            int x2 = r.Next(3);
            int y2 = r.Next(3);

            Swap(qX * 3 + x1, qY * 3 + y1, qX * 3 + x2, qY * 3 + y2);
        }

        public int HillClimbStep()
        {
            int qX = r.Next(3); //random box_x
            int qY = r.Next(3); //random box_y

            //Console.WriteLine((qX, qY));


            //For each square calculate the score change for removing it and for setting it to a certain value.
            int[,] removalPenalties = new int[3, 3];
            int[,,] addBonus = new int[3, 3, 9];

            for (int y=0; y<3; y++)
            {
                for (int x=0; x<3; x++)
                {
                    //if(!unmovable[qY * 3 + y, qX * 3 + x])
                    //{
                        HashSet<int> col = valsInColumn(qX*3+x, qY * 3 + y);
                        HashSet<int> row = valsInRow(qY * 3 + y, qX * 3 + x);

                        int currentVal = s.board[qY * 3 + y, qX * 3 + x];

                        if (!col.Contains(currentVal))
                        {
                            removalPenalties[y, x] -= 1;
                        }
                        if (!row.Contains(currentVal))
                        {
                            removalPenalties[y, x] -= 1;
                        }
                        for (int i=0; i < 9; i++) // wat als i gelijk is aan current val????
                        {
                            if (!col.Contains(i+1))
                            {
                                addBonus[y, x, i] += 1;
                            }
                            if (!row.Contains(i+1))
                            {
                                addBonus[y, x, i] += 1;
                            }
                        }

                    //}

                }
            }


            int maxScore = 0;
            List<(int, int, int, int)> bestMoves = new List<(int, int, int, int)>();

            for (int x1 = 0; x1 < 3; x1++)
            {
                for (int y1 = 0; y1 < 3; y1++)
                {
                    for (int x2 = 0; x2 < 3; x2++)
                    {
                        for (int y2 = 0; y2 < 3; y2++)
                        {
                            /* loop alle mogelijke vakjes combinaties */
                            if ((x1 != x2 || y1 != y2)&&(!s.unmovable[qY * 3 + y1, qX * 3 + x1])&&(!s.unmovable[qY * 3 + y2, qX * 3 + x2])) //punt x1,y1 != punt x2,y2
                            {
                                    int score = removalPenalties[y1, x1] +
                                    removalPenalties[y2, x2] +
                                    addBonus[y2, x2, s.board[qY * 3 + y1, qX * 3 + x1]-1] +
                                    addBonus[y1, x1, s.board[qY * 3 + y2, qX * 3 + x2]-1];
                                    if (score > maxScore)
                                    {
                                        bestMoves = new List<(int, int, int, int)>();
                                        maxScore = score;
                                        bestMoves.Add((x1, y1, x2, y2));
                                    }
                                    else if (score == maxScore)
                                    {
                                        bestMoves.Add((x1, y1, x2, y2));
                                    }

                                
                            }
                        }
                    }
                }
            }

            if (bestMoves.Count == 0)
            {
                return -1;
            }

            //Console.WriteLine(bestMove);
            //Console.WriteLine(maxScore);

            int temp = r.Next(bestMoves.Count);

            (int x1r, int y1r, int x2r, int y2r) = bestMoves[temp];

            Swap(qX * 3 + x1r, qY * 3 + y1r, qX * 3 + x2r, qY * 3 + y2r);
            s.currentScore-=maxScore;
            return s.currentScore;
        }

        // public int HillClimbStep2()
        // {
        //     int qX = r.Next(3); //random box_x
        //     int qY = r.Next(3); //random box_y
        //     int maxScore = 9;
        //     List<(int, int, int, int)> bestMoves = new List<(int, int, int, int)>();
        //     for (int y1 = 0; y1 < 3; y1++)
        //     {
        //         for (int x1 = 0; x1 < 3; x1++)
        //         {
        //             int currentVal = board[qY * 3 + y1, qX * 3 + x1];
        //             HashSet<int> col1 = valsInColumn(qX*3+x1, qY * 3 + y1);
        //             HashSet<int> row1 = valsInRow(qY * 3 + y1, qX * 3 + x1);
        //             bool c1_old = col1.Contains(currentVal);
        //             bool r1_old = row1.Contains(currentVal);
        //             for (int y2 = y1; y2 < 3; y2++)
        //             {
        //                 for (int x2 = x1; x2 < 3; x2++)
        //                 {
        //                     /* loop alle mogelijke vakjes combinaties */
        //                     if ((x1 != x2 || y1 != y2)&&(!unmovable[qY * 3 + y1, qX * 3 + x1])&&(!unmovable[qY * 3 + y2, qX * 3 + x2])) //punt x1,y1 != punt x2,y2
        //                     {
        //                         int tempVal = board[qY * 3 + y2, qX * 3 + x2];
        //                         int col_score = 0;
        //                         int row_score = 0;
        //                         if(!(x1==x2))
        //                         {
        //                             HashSet<int> col2 = valsInColumn(qX*3+x2, qY * 3 + y2);
                                    
        //                             bool c1_new = col1.Contains(tempVal);
        //                             bool c2_old = col2.Contains(tempVal);
        //                             bool c2_new = col2.Contains(currentVal);
        //                             col_score+=scorecheck(c1_old,c1_new);
        //                             col_score+=scorecheck(c2_old,c2_new);
        //                         }
        //                         if(!(y1==y2))
        //                         {
        //                             HashSet<int> row2 = valsInRow(qY * 3 + y2, qX * 3 + x2);
        //                             bool r1_new = row1.Contains(tempVal);
        //                             bool r2_old = row2.Contains(tempVal);
        //                             bool r2_new = row2.Contains(currentVal); //kan efficienter geplaatst worden
        //                             row_score+=scorecheck(r1_old,r1_new);
        //                             row_score+=scorecheck(r2_old,r2_new);
        //                         }
        //                         int score = col_score+row_score;
        //                         if(score<maxScore)
        //                         {
        //                             bestMoves = new List<(int, int, int, int)>();
        //                             maxScore = score;
        //                             bestMoves.Add((x1, y1, x2, y2));
        //                         }
        //                         else if (score == maxScore)
        //                         {
        //                             bestMoves.Add((x1, y1, x2, y2));
        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //     }
        //     if (bestMoves.Count == 0)
        //     {
        //         return -1;
        //     }
        //     int temp = r.Next(bestMoves.Count);

        //     (int x1r, int y1r, int x2r, int y2r) = bestMoves[temp];

        //     Swap(qX * 3 + x1r, qY * 3 + y1r, qX * 3 + x2r, qY * 3 + y2r);

        //     currentScore+=maxScore;
        //     return currentScore;
        // }

        // private int scorecheck(bool old, bool nieuw)
        // {
        //     if (old && !nieuw)
        //     {
        //         return -1;
        //     }
        //     else if ((old && nieuw)||!old && !nieuw)
        //     {
        //         return 0;
        //     }
        //     return 1;
        // }















    }
}
