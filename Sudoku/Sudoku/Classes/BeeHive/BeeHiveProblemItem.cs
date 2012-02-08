using Sudoku.Interfaces;

namespace BeeHive.Classes
{
    class BeeHiveProblemItem : ISolvableProblemItem
    {
        private short[,] field;

        public BeeHiveProblemItem()
        {
            field = new short[9,9];
        }

        public short this[int a, int b]
        {
            get { return Field[a, b]; }
            set
            {
                if(Field == null)
                    Field = new short[9,9];
                Field[a, b] = value;
            }
        }

        public short[,] Field
        {
            get { return field; }
            set { field = value; }
        }

        public bool isGoal()
        {           
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (Field[j, i] == 0)
                        return false;

            return true;
        }

        public ISolvableProblemItem getProblemStart()
        {
            Field = new short[9, 9]{
                 {0, 9, 0, 0, 0, 6, 1, 0, 0}, 
                 {0, 0, 0, 5, 0, 0, 0, 3, 2}, 
                 {3, 0, 4, 0, 1, 0, 0, 0, 0}, 

                 {0, 0, 0, 8, 5, 0, 0, 1, 0}, 
                 {9, 5, 0, 6, 0, 0, 0, 8, 0}, 
                 {0, 0, 8, 0, 0, 9, 2, 0, 5}, 

                 {0, 0, 0, 0, 9, 0, 3, 0, 7}, 
                 {4, 0, 3, 2, 0, 0, 0, 0, 0}, 
                 {0, 6, 0, 0, 3, 5, 8, 0, 0} 
             };
            return this;
        }

        public ISolvableNode getStartNode()
        {
            return new BeeHiveNode(getProblemStart());
        }
    }
}
