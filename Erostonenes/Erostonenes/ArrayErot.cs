using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erostonenes
{
    public class ArrayErot
    {
        static void run(int max)
        {
            //const int max = 2000000;
            //const int max = 1000;

            int[] lista = new int[max];
            for (int i = 0; i < max; i++)
            {
                lista[i] = i;
            }

            for (int i = 2; i < Math.Sqrt(max); i++)
            {
                for (int j = i + 1; j < max; j++)
                {
                    if (lista[j] % i == 0)
                        lista[j] = 0;
                }
            }

            foreach (var item in lista)
            {
                if (item > 1)
                    Console.Write(item + "\t");
            }
        }
    }
}
