using System;
using System.Collections.Generic;
using Sudoku.Interfaces;
using Sudoku.Classes;

namespace BeeHive
{
    /*
    * Class Node
    *
    * Use as a node in the search tree with lots of fun stuff
    *
    *
    */
    class BeeHiveNode : ISolvableNode
    {
        private long checksum;
        private SudokuProblemItem problemItem;



        public BeeHiveNode(ISolvableProblemItem problemItem)
        {
            SolvableProblemItem = new SudokuProblemItem();

            short[,] cf = ((SudokuProblemItem)problemItem).Field;
            // Set the values of things
            ((SudokuProblemItem)SolvableProblemItem).Field = new short[9, 9];
            for (int i = 0; i < 9; i++)
                for (int ii = 0; ii < 9; ii++)
                    ((SudokuProblemItem)SolvableProblemItem)[i, ii] = cf[i, ii];
            init();
        }

        public ISolvableProblemItem SolvableProblemItem
        {
            get { return problemItem; }
            set
            {
                if(problemItem == null)
                    problemItem = new SudokuProblemItem();
                problemItem = (SudokuProblemItem)value;
            }
        }

        public ISolvableNode createCopy()
        {
            return new BeeHiveNode(SolvableProblemItem);
        }

        public void init()
        {
            calculateChecksum();
        }
        public bool isGoal()   // Indicates if this Node is the solution to the problem
        {
            //for (int i = 0; i < 9; i++)
            //    for (int j = 0; j < 9; j++)
            //        if (((SudokuProblemItem)SolvableProblemItem)[j, i] == 0)
            //            return false;

            //return true;
            return SolvableProblemItem.isGoal();
        }
        public void print()
        {
            printShort();
        }
        private void printShort()
        {
            Console.WriteLine("Checksum is: " + checksum);
            for (short i = 0; i < 9; i++)
            {
                for (short ii = 0; ii < 9; ii++)
                    Console.Write(" " + ((SudokuProblemItem)SolvableProblemItem)[i, ii]);
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
        private void calculateChecksum()
        {
            // Calculate the checksum of the Field.
            checksum = 0;
            checksum = SolvableProblemItem.GetHashCode();
        }

        /// <summary>
        /// Get all children that can be transformed from this node.
        /// </summary>
        /// <returns></returns>
        public List<ISolvableNode> getChildren()
        {
            // Return all posible children for the 'next' position in the grid
            int[] next = getNextFreeSpot();

            if (next == null)
                return null; // We could not find a free spot. Thus, we cannot return any children

            List<ISolvableNode> possible = new List<ISolvableNode>();
            int counter = 0;
            for (short i = 1; i < 10; i++)
            {
                if (!(isInCol(i, next) || isInRow(i, next) || isInSquare(i, next)))
                {
                    possible.Add(new BeeHiveNode(SolvableProblemItem));
                    ((SudokuProblemItem)((BeeHiveNode)possible[counter]).SolvableProblemItem)[next[0], next[1]] = i;
                    possible[counter].init();
                    counter++;
                }
            }
            return possible;
        }

        /// <summary>
        /// Is this node equal to f?
        /// </summary>
        /// <param name="f">The other node to compare to</param>
        /// <returns></returns>
        public bool isEqualTo(ISolvableNode f)
        {
            calculateChecksum();
            f.init();
            if (checksum == f.getChecksum())
                return true;
            return false;
        }

        /// <summary>
        /// Returns the checksum, some value that is unique to the current state
        /// </summary>
        /// <returns></returns>
        public long getChecksum()
        { return checksum; }

        /// <summary>
        /// Gets the location of the next free spot in the 9*9 (the big) grid
        /// </summary>
        /// <returns></returns>
        private int[] getNextFreeSpot()
        {
            // Finds the next spot in field in wich we can put a number
            // If this is used those spots who are assigned a number att start will not be evaluated with the getchildren, its ok since it would only have one children.
            // Save some memory....
            // Can this function be altered in a way so that the next spot will be picked at random? And would that increase the overall performance of the solution?
            for (int i = 0; i < 9; i++)
            {
                for (int ii = 0; ii < 9; ii++)
                {
                    if (((SudokuProblemItem)SolvableProblemItem)[i, ii] == 0)
                    {
                        int[] r = new int[] { i, ii };
                        return r;
                    }
                }
            }
            Console.WriteLine("Found no spot to put the next number in. Is the playfield full?");
            return null; // Found no spot, is it full?
        }

        /// <summary>
        /// Checks if n is in the current Row
        /// </summary>
        /// <param name="n">The number to look for</param>
        /// <param name="pos">The row to search in</param>
        /// <returns></returns>
        private bool isInRow(short n, int[] pos)
        {
            // true om finns i raden
            for (int i = 0; i < 9; i++)
                if (((SudokuProblemItem)SolvableProblemItem)[pos[0], i] == n)
                    return true;

            return false;

        }
        
        /// <summary>
        /// Checks if n is in the current column
        /// </summary>
        /// <param name="n">The number to look for</param>
        /// <param name="pos">The row to search in</param>
        /// <returns></returns>
        private bool isInCol(short n, int[] pos)
        {
            // true om finns i columnen
            for (int i = 0; i < 9; i++)
                if (((SudokuProblemItem)SolvableProblemItem)[i, pos[1]] == n)
                    return true;
            return false;
        }
    
        /// <summary>
        /// Checks if n is in the current 3*3 square
        /// </summary>
        /// <param name="n">The number to search for</param>
        /// <param name="pos">The square</param>
        /// <returns></returns>
        private bool isInSquare(short n, int[] pos)
        {
            // true om finns i raden
            short h = Convert.ToInt16(pos[0] / 3);
            short b = Convert.ToInt16(pos[1] / 3);

            for (short x = Convert.ToInt16(3 * b); x < Convert.ToInt16(3 * (b + 1)); x++)
                for (short y = Convert.ToInt16(3 * h); y < Convert.ToInt16(3 * (h + 1)); y++)
                    if (((SudokuProblemItem)SolvableProblemItem)[y, x] == n)
                        return true;
            return false;
        }

  
    }
}
