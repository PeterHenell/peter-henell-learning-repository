namespace Sudoku.Interfaces
{
    interface ISolvableProblemItem
    {
        /// <summary>
        /// Indicates if this Item is the goal of the problem
        /// </summary>
        /// <returns></returns>
        bool isGoal();

        /// <summary>
        /// Should set itself to be the startingpoint of the problem and then return itself or return
        /// </summary>
        /// <returns></returns>
        ISolvableProblemItem getProblemStart();

        ISolvableNode getStartNode();

       

    }
}
