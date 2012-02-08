using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erostonenes
{
    public class ListErot
    {
        public static void run(int max)
        {
            //const int max = 2000000;
            //const int max = 1000;

            int[] lista = new int[max];
            for (int i = 2; i < max; i++)
            {
                lista[i] = i;
                
            }

            for (int i = 2; i < Math.Sqrt(max); i++)
            {
                lista = Array.FindAll<int>(lista, 
                                            x =>  
                                                x == i || x % i != 0 );
            }

            long sum = 0;
            foreach (var item in lista)
            {
                sum += item;
                Console.WriteLine(item + "\t");
                    
            }
            Console.WriteLine(sum);
        }
    }
}
