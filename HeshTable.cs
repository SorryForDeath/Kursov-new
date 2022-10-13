using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Hesh.HeshTable;




namespace Hesh
{

    public class HeshTable
    {

        public static int N;
        public int N1;
        public struct Game
        {
            public string name;
            public string genre;
            public int year;
            public string developer;
            public int sales;

            public Game(string name, string genre, int year, string developer, int sales)
            {
                this.name = name;
                this.genre = genre;
                this.year = year;
                this.developer = developer;
                this.sales = sales;
            }
            public void Print()
            {
                Console.WriteLine(name + " " + genre + " " + year + " " + developer + " " + sales);
            }
            public void Set(string name, string genre, int year, string developer, int sales)
            {
                this.name = name;
                this.genre = genre;
                this.year = year;
                this.developer = developer;
                this.sales = sales;
            }
            public static bool operator ==(Game w, Game w2)
            {
                return w.Equals(w2);
            }
            public static bool operator !=(Game w, Game w2)
            {
                return !(w.Equals(w2));
            }
        }
        public enum Status
        {
            Empty = 0,
            Is = 1,
            Deleted = 2,
        }
        public struct Hesh
        {
            public Status status;
            public Game value;
            public int key;
        }
        public Game[] wow2; //дополнительный массив, считываю в него информацию из текстового файла
        public Hesh[] wow;  //основной массив
        public void Read()
        {
            string[] file_a = File.ReadAllLines(@"C:\Game.txt");
            int count = 0;
            foreach (string i in file_a)
            {
                int ind = i.IndexOf("/");
                wow2[count].name = i.Substring(0, ind);
                string k = i.Remove(0, ind + 1);

                ind = k.IndexOf("/");
                wow2[count].genre = k.Substring(0, ind);
                k = k.Remove(0, ind + 1);

                ind = k.IndexOf("/");
                string str = k.Substring(0, ind);
                wow2[count].year = int.Parse(str);
                k = k.Remove(0, ind + 1);

                ind = k.IndexOf("/");
                wow2[count].developer = k.Substring(0, ind);
                k = k.Remove(0, ind + 1);

                ind = k.IndexOf("/");
                str = k.Substring(0, ind);
                wow2[count].sales = int.Parse(str);
                k = k.Remove(0, ind + 1);
                count++;
            }
        }
        public void PrintHesh()

        {
            for (int i = 0; i < N; i++)
            {
                if ((wow[i].status == Status.Empty) | (wow[i].status == Status.Deleted))
                {
                    Console.Write("This line is empty or deleted\n");
                }
                else
                {
                    Console.Write(wow[i].value.name + " ");
                    Console.Write(wow[i].value.genre + " ");
                    Console.Write(wow[i].value.year + " ");
                    Console.Write(wow[i].value.developer + " ");
                    Console.WriteLine(wow[i].value.sales + " ");
                }
            }
        }
        public HeshTable(int s)
        {
            N = s;
            N1 = s;
            wow2 = new Game[N];
            wow = new Hesh[N];
            for (int i = 0; i < N; i++)
            {
                wow[i].status = Status.Empty;
            }
            Read();
        }
        public static int Svertka(Game w)
        {
            int b = w.sales;
            int key = 0;
            int t;
            while (b > 0)
            {
                t = b % 100;
                key += t;
                b /= 100;
            }
            while (key >= N)
            {
                key %= N;
            }
            return key;
        }
        public int Size()
        {
            int k = 0;
            for (int i = 0; i < N; i++)
            {
                if (wow[i].status == Status.Is) k++;
            }
            return k;
        }
        public int Find(Game w)
        {
            int max = Size();
            int i = 0;
            int k = Svertka(w);
            while (wow[k].status == Status.Is & i < max)
            {
                if (wow[k].value == w & wow[k].status == Status.Is) return k; //перегрузку надо реализовать
                else
                {
                    k++;
                    k %= N;
                    i++;
                }
            }
            Console.WriteLine("There isn't this string");
            return -1;
        }
        public void Add(Game w)
        {
            int k = Svertka(w);
            int s = k;
            int q = 0;
            while (wow[k].status == Status.Is)
            {
                k++;
                k %= N;
                q++;
                if (q > N)
                {
                    Console.WriteLine("Table is full");
                    return;
                }
            }
            wow[k].key = s;
            wow[k].status = Status.Is;
            wow[k].value = w;
        }
        public void Delete(Game w)
        {
            int k = Find(w);
            if (k == -1)
            {
                Console.WriteLine("There isn't this line"); return;
            }
            else
            {
                wow[k].status = Status.Deleted;
            }
        }
        ~HeshTable()
        {
            for (int i = 0; i < N; i++)
            {
                wow[i].status = Status.Empty;
            }
        }
    }
}
