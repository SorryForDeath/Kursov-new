using System;
using Hesh;
using RedBlackTree;
namespace Work
{
    class Program
    {
        static void Main()
        {

            HeshTable W = new(11);
            //HeshTable.Game G = new ("Name", "Genre", 2000, "Rockstar", 200000);
            HeshTable.Game R = new("Name", "Genre", 2000, "Rockstar", 452742);
            for (int i = 0; i < W.N1; i++)
            {
                W.Add(W.wow2[i]);
            } 
            W.PrintHesh();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Tree T = new();
            Dev dateToIncert = new();
            for (int sales = 1; sales <= 9; sales++)
            {
                    dateToIncert.Set("R", "W", sales);
                    T.Add(dateToIncert);      
            }
            T.PrintTree();
        }
    }
}
