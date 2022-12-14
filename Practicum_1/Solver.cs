using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
namespace Practicum_1
{
    public class Solver
    {
        public Sudoku s;
        Random r;
        int s_p;
        int p_p;

        public Solver(Sudoku sudoku , int S_parm , int plateau_parm)
        {/*
        Constructor requires a:
            - sudoku object
            - int S_parm: parameter for the stepsize
            - int plateau_parm: parameter for determining if a plateau is reached
        */
            s = sudoku;
            r = new Random();
            s_p = S_parm;
            p_p = plateau_parm;
        }

        public void iteratedLocalSearch()
        {/* iteratedLocalSearch starts the iterated local search and records
                the amount of time that was required to solve the sudoku.
                It also checks whether a plateau is reached. 
        */
            Stopwatch watch = Stopwatch.StartNew(); 
            fillWithRandom();
            getBoardScore();
            int last_res = s.currentScore;
            int counter = 0;
            while (s.currentScore!=0)
            { /*Stops when the sudoku is solved, because when the sudoku is solved
                    the result of the heuristic function equals zero*/
                if (counter>p_p) 
                { // counter > plateau parameter = randomwalking
                    walking();
                }
                HillClimbing();
                if (s.currentScore == last_res) 
                { // no changes
                    counter++;
                }
                else
                { // changes to the score:
                    counter = 0;
                    last_res = s.currentScore;
                }
            }
            watch.Stop();
            Console.WriteLine("Spend " + watch.ElapsedMilliseconds + " MilliSeconds");
            Console.WriteLine("Score: " + s.currentScore);
            //Debugging:
            //getBoardScore();
            //Console.WriteLine("Echte Score: " + s.currentScore);
            s.printBoard();


        }
        private void HillClimbing() //rename naar FirstImprovement
        {/* HillClimbing this is the First improvement part of the hill-climbing alg
                it goes through all boxes and calls a function that does best-improvement
                if the states generated are not an improvement or equal to the current state
                it check a random next box.
        */
            int res = -1;
            List<(int,int)> boxes = new List<(int,int)>{ (0,0),(0,1),(0,2),(1,0),(1,1),(1,2),(2,0),(2,1),(2,2)};
            while (res == -1)
            { // the result is -1 when no improvement or an equal score has been found in the box
                    //this also means no swap has occured.
                if (boxes.Count!=0)
                { // boxes is a list with unexplored boxes.
                    int box_idx = r.Next(boxes.Count);
                    int qX = boxes[box_idx].Item1; // random box_x
                    int qY = boxes[box_idx].Item2; // random box_y
                    boxes.RemoveAt(box_idx);
                    res = HillClimbStep2(qX,qY);   // best improvement on random box 
                }
                else
                { // local minimum has been found.
                    walking();
                    break;
                }


            }
            
        }
        private void walking()
        {/* walking repeats the walkingstep S times.
        */
            for (int j = 0; j<s_p; j++)
            {
                RandomWalkStep();
            }
            getBoardScore(); //kan dit miss efficiënter???

        }

        public void fillWithRandom()
        {/* fillWithRandom fills a sudoku with randomly generated integers ranging from 1 up untill 9
                but doesn't add numbers that were in the input 
        */
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
        public void fillSudoku ()
        {/* fillSudoku fills a sudoku with the lowest integer that isn't already in the box
        */
            for(int hb = 0; hb<3;hb++)                            //horizontale boxes 0-2
            {
                for(int vb = 0; vb<3;vb++)                        //verticale boxes 0-2
                {
                    int value = 1;                                  //reset value wnr nieuwe box wordt bezocht
                    for(int r = 0; r<3;r++)                     //visit row 0-2
                    {               
                        for (int c =0; c<3;c++)                 //visit collumn 0-2
                        {
                            if (s.board[r+hb*3,c+vb*3] == 0)   //multiply rows and collumns by box number
                            {
                                while(s.in_box(r+hb*3,c+vb*3,value)) //add until a new not used value is found
                                {
                                    value++;
                                }
                                s.board[r+hb*3,c+vb*3] = value;
                                value++; 
                            }
                        }
                    }

                }

            }
        }

        public void Swap(int x1, int y1, int x2, int y2)
        {/* Swap swaps 2 squares in the sudoku
                - int x1: x-index from square 1
                - int y1: y-index from square 1
                - int x2: x-index from square 2
                - int y2: y-index from square 2
        */
            (s.board[y1,x1], s.board[y2,x2]) = (s.board[y2, x2], s.board[y1, x1]);
        }

        public int getBoardScore()
        {/* getBoardScore checks the whole sudoku board and calculates the heuristic values
                for each row and collumn, then it is combined to calculate the score.
        */
            int res = 0;
            for (int i =0; i<9; i++)
            {
                res += 9 - valsInColumn(i).Count;
                res += 9 - valsInRow(i).Count;
            }
            s.currentScore = res; //dit had ik aangepast maar is het ergens nodig om die integer te returnen
            return res;
        }
        public HashSet<int> valsInColumn(int x, int excludeIndex = 10)
        {/* valsInColumn returns a hashtable with all values inside a given collumn 
                - int x: is the index of the collumn
                - in excludeIndex: is the index that you want to exclude default doesn't exclude an index.
        */
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
        {/* valsInColumn returns a hashtable with all values inside a given row 
                - int y: is the index of the collumn
                - in excludeIndex: is the index that you want to exclude default doesn't exclude an index.
        */
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
        {/* RandomWalkStep generates the coördinates of a random square on the sudokuboard
                checks if this a movable square and swaps the square if this is the case.
        */
            //Choose 3x3 section
            int qX = r.Next(3);
            int qY = r.Next(3);

            //Choose squares
            int x1 = r.Next(3);
            int y1 = r.Next(3);
            int x2 = r.Next(3);
            int y2 = r.Next(3);

            
            if (s.unmovable[qY * 3 + y1 ,qX * 3 + x1] || s.unmovable[qY * 3 + y2 , qX * 3 + x2])
            { // the random generated square in the sudokuboard is one of the numbers in the input
                RandomWalkStep();
            }
            else
            { // we are allowed to swap the generated square
                Swap(qX * 3 + x1, qY * 3 + y1, qX * 3 + x2, qY * 3 + y2);
            }
            
        }

        public int HillClimbStep(int qX,int qY)
        {
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
            int t = s.currentScore;

            getBoardScore();
            if(t!=-1 && t != s.currentScore)
            {
                Console.WriteLine(t);                
                Console.WriteLine("Echte Score: " + s.currentScore);
            }
            return s.currentScore;
        }

        public int HillClimbStep2( int qX , int qY) //BestImprovement
        {/* HillClimbStep2 is a less efficient way of doing best-improvement within a box
                it creates a list with the best moves that have either no effect on the score or are an improvement
                    - it itterates over all movable squares in the box and generates hashtables of values in the squares rows and collomns
                        excluding the current square
                            - for each square we compare it to all the other squares
                                we check if when these squares are swapped there is an improvement or no difference
                                then we add this possible swap to a list if it has the same difference or in a new one if it is better.
                then the a random move is selected swapped and the score is updated
                - int qX: x-index of a box
                - int qY: y-index of a box.
        */
            int maxScore = 0;
            List<(int, int, int, int)> bestMoves = new List<(int, int, int, int)>();
            for (int y1 = 0; y1 < 3; y1++)
            {
                for (int x1 = 0; x1 < 3; x1++)
                {
                    if (!s.unmovable[qY * 3 + y1, qX * 3 + x1])
                    {
                        int currentVal = s.board[qY * 3 + y1, qX * 3 + x1];
                        HashSet<int> col1 = valsInColumn(qX*3+x1, qY * 3 + y1);
                        HashSet<int> row1 = valsInRow(qY * 3 + y1, qX * 3 + x1);
                        bool c1_old = col1.Contains(currentVal);
                        bool r1_old = row1.Contains(currentVal);
                    
                        for (int y2 = 0; y2 < 3; y2++)
                        {
                            for (int x2 = 0; x2 < 3; x2++)
                            {
                                /* loop alle mogelijke vakjes combinaties */
                                if ((x1 != x2 || y1 != y2)&&(!s.unmovable[qY * 3 + y2, qX * 3 + x2])) //punt x1,y1 != punt x2,y2
                                {
                                    int tempVal = s.board[qY * 3 + y2, qX * 3 + x2];
                                    int col_score = 0;
                                    int row_score = 0;
                                    if(!(x1==x2))
                                    {
                                        HashSet<int> col2 = valsInColumn(qX*3+x2, qY * 3 + y2);
                                    
                                        bool c1_new = col1.Contains(tempVal);
                                        bool c2_old = col2.Contains(tempVal);
                                        bool c2_new = col2.Contains(currentVal);
                                        col_score+=scorecheck(c1_old,c1_new);
                                        col_score+=scorecheck(c2_old,c2_new);
                                    }
                                    if(!(y1==y2))
                                    {
                                        HashSet<int> row2 = valsInRow(qY * 3 + y2, qX * 3 + x2);
                                        bool r1_new = row1.Contains(tempVal);
                                        bool r2_old = row2.Contains(tempVal);
                                        bool r2_new = row2.Contains(currentVal); //kan efficienter geplaatst worden
                                        row_score+=scorecheck(r1_old,r1_new);
                                        row_score+=scorecheck(r2_old,r2_new);
                                    }
                                    int score = col_score+row_score;
                                    if(score<maxScore)
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
            }
            if (bestMoves.Count == 0)
            { // no improvement has been found
                return -1;
            }
            int temp = r.Next(bestMoves.Count);
            (int x1r, int y1r, int x2r, int y2r) = bestMoves[temp];                 //chooses a random swap from the bestMoves list
            Swap(qX * 3 + x1r, qY * 3 + y1r, qX * 3 + x2r, qY * 3 + y2r);
            s.currentScore+=maxScore;
            return s.currentScore;
        }

        private int scorecheck(bool old, bool nieuw)
        {/* scorecheck checks if the score improves or stays the same or gets worse when a certain value is inserted
            in a certain square. It returns a -1 for improvement, 0 when it's equal and a +1 when it makes the score higher
                - bool old: boolean if the old value is in a row or collumn
                - bool new: boolean if the new value is in a row or collumn
        */
            if (old && !nieuw)
            {
                return -1;
            }
            else if ((old && nieuw)||!old && !nieuw)
            {
                return 0;
            }
            return 1;
        }
    }
}
