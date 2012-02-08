using System;
using System.Collections.Generic;
using Sudoku.Interfaces;

namespace Sudoku
{

    /// <summary>
    /// The agent will try to solv any problem that implements ISolvableNode, using different tree search algorithms.
    /// </summary>
    class Agent
    {
        private readonly List<ISolvableNode> OPEN;
        private readonly List<ISolvableNode> CLOSED;
        private readonly List<ISolvableNode> NODES;
        private ISolvableNode root;

        public Agent()
        {
            OPEN = new List<ISolvableNode>();
            CLOSED = new List<ISolvableNode>();
            NODES = new List<ISolvableNode>();
        }
        private bool solve(ISolvableProblemItem solvableProblemItem)
        {
            root = solvableProblemItem.getStartNode();
            OPEN.Add(root);
            while (true)
            {
                if (OPEN.Count == 0)
                {
                    Console.WriteLine("Failed after " + CLOSED.Count);
                    return false;
                }

                ISolvableNode N = OPEN[0].createCopy();
                CLOSED.Add(N.createCopy());
                OPEN.RemoveAt(0);

                if (N.isGoal())
                {
                    N.print();
                    return true;
                }

                List<ISolvableNode> tmp = new List<ISolvableNode>();   
                tmp.Clear();
                tmp = N.getChildren();
                NODES.Clear();

                for (short i = 0; i < tmp.Count; i++)
                    NODES.Add(tmp[i]);

                for (int i = 0; i < OPEN.Count; i++)
                {
                    for (int ii = NODES.Count - 1; ii > 0; ii--)
                    {
                        if (OPEN[i].isEqualTo(NODES[ii]))
                        {
                            OPEN[i].print();
                            NODES[ii].print();
                            NODES.RemoveAt(ii);
                        }
                    }
                }
                for (int i = 0; i < CLOSED.Count; i++)
                {
                    for (int ii = NODES.Count - 1; ii > 0; ii--)
                    {
                        if ((CLOSED[i]).isEqualTo(NODES[ii]))
                        {
                            CLOSED[i].print();
                            NODES[ii].print();
                            NODES.RemoveAt(ii);
                        }
                    }
                }

                Expand(NODES);
            }
        }
        private void Expand(IList<ISolvableNode> cNODES)
        {
            for (int i = 0; i < cNODES.Count; i++)
                OPEN.Add(cNODES[i].createCopy());
        }

        public bool solveProblem(ISolvableProblemItem solvableProblemItem)
        {
            return solve(solvableProblemItem);
        }
    }


}
