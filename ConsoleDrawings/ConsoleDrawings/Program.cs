using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDrawings
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 25;
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    Console.Write(
                        (j) % ((i + j+1)+10)*2 < (i) % (i + 1 * j + 1)+15 ? "# " : ". "
                        //(i)%(i+j+1)>(j)%(i+1 * j+1) ? "# " : ". "
                        //(Math.E + i*j) % 3 > 2 ? "# " : ". "
                        //Math.Pow(i, i * j) % 3 > 1 ? "# " : ". "
                        //i * j % 4 > 1 ? "# " : ". "
                        //i * j % 3 > 1 ? "# " : ". "
                        //i * j % 4 > 2 ? "# " : ". "
                        //i * j % 3 > 0 ? "# " : ". "
                        //i*j % 2 > 0 ? "# " : ". "
                        //Math.Pow(i, j)%2 == 1 ? "# " : ". "
                        //i/2 > j ? "# " : ". "
                        //i > j ? "# " : ". "
                        //i % (j + 1) > 5 ? "# " : ". "
                        //i * j % 7 == 2 ? "# " : ". "
                        );

                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
