using System;
using Sudoku;

/* Felstavat mm */
namespace Sudoku
{
    class Sudukosolver
    {
        static void Main()
        {
            Agent agent = new Agent();
            
            if (agent.solveProblem(new Classes.SudokuProblemItem().getProblemStart()))
            {
                Console.WriteLine("Yay");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Nay :(");
                Console.ReadKey(); 
            }
            
        }
    }


    
    
}
