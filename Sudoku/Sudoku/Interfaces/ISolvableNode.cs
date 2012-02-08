using System.Collections.Generic;

namespace Sudoku.Interfaces
{
    /// <summary>
    /// ISolvableNode is a state that can be transformed towards a goal. It is transformed by calling getChildren.
    /// </summary>
    interface ISolvableNode
    {

        /// <summary>
        /// Creates a copy of This Solvable Node
        /// </summary>
        /// <returns></returns>
        ISolvableNode createCopy();

        /// <summary>
        /// If needed to initialize the SolvableNode
        /// </summary>
        void init();

        /// <summary>
        /// Indicates if this Solvable Node is the solution to the problem. The Solvable Node need to define it's own goal.
        /// </summary>
        /// <returns>Should return true if it is the goal, else false</returns>
        bool isGoal();

        /// <summary>
        /// Prints the current state of the Solvable Node
        /// </summary>
        void print();

        /// <summary>
        ///  Get all possible transformations of the Solvable Node
        /// </summary>
        /// <returns>A list of all possible solvable Nodes that can be transformed from This </returns>
        List<ISolvableNode> getChildren();

        /// <summary>
        /// Indicates that This have the same state as f
        /// </summary>
        /// <param name="f">The other Solvale Node to compare to</param>
        /// <returns></returns>
        bool isEqualTo(ISolvableNode f);

        /// <summary>
        ///  Some value that indicated uniqeness of the Solvable Node
        /// </summary>
        /// <returns>A long value</returns>
        long getChecksum();


        ISolvableProblemItem SolvableProblemItem
        {
            get;
            set;
        }

    }
}
