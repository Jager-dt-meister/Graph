using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class Program
    {


        static void Main(string[] args)
        {
            Graph graph = null;
            bool flag = true;
            char[] seps = new char[6] { ' ', ':','(',')','[',']'};
            while (flag)
            {
                Console.WriteLine("0 - считать граф из файла \n " +
                    "1 - создать граф \n" +
                    "2 - добавить вершину \n" +
                    "3 - удалить вершину \n" +
                    "4 - добавить ребро \n" +
                    "5 - удалить ребро \n" +
                    "6 - вывести весь граф \n" +
                    "7 - вывести граф в файл \n" +
                    "w - задание 3: для каждой вершины графа вывести ее степень \n"+
                    "e - задание 11: для данной вершины орграфа вывести все заходящие вершины \n"+
                    "q - выйти");
                char letter = char.Parse(Console.ReadLine());
                switch (letter)
                {
                    case '0':
                        {
                            Console.WriteLine("Введите название файла");
                            string s = Console.ReadLine();
                            graph = new Graph(s);
                            break;
                        }
                    case '1':
                        {
                            try
                            {
                                Console.WriteLine("Тип графа: \n" +
                                    "nn - неор невзв \n" +
                                    "nv - неор взв \n" +
                                    "ov - ор взв \n" +
                                    "on - ор невзв");
                                string s = Console.ReadLine();
                                Console.WriteLine("Количество вершин в графе:");
                                int n = int.Parse(Console.ReadLine());
                                if (n <= 0)
                                {
                                    throw new ArgumentOutOfRangeException("Некорректное значение!");
                                }
                                else
                                {
                                    if (s.Equals("nn") || s.Equals("on"))
                                    {
                                        Dictionary<string,List<string>> d = new Dictionary<string,List<string>>();
                                        while (n > 0)
                                        {
                                            Console.WriteLine("Введите  название вершины");
                                            string name = Console.ReadLine();
                                            Console.WriteLine("Введите  названия вершин, с которыми она будет соединена");
                                            string[] mas = Console.ReadLine().Split(seps, StringSplitOptions.RemoveEmptyEntries);

                                            List<string> l = new List<string>();
                                            for (int i = 0; i < mas.Length; i++)
                                            {
                                                l.Add(mas[i]);
                                            }
                                            //if (l.Count == 0)
                                            //    l.Add(name);
                                            d.Add(name,l);
                                            n--;
                                        }
                                        foreach (var item in d.Values)
                                        {
                                            foreach (var item2 in item)
                                            {
                                                if (!d.ContainsKey(item2))
                                                {
                                                    throw new ArgumentException("Введенные вершины не существует в данном графе!");
                                                }
                                                else
                                                {
                                                    graph = new Graph(d,s);
                                                }
                                            }
                                        }
                                    }
                                    else if (s.Equals("nv")|| s.Equals("ov"))
                                    {
                                        Dictionary<string, Dictionary<string, double>> d = new Dictionary<string, Dictionary<string, double>>();
                                        while (n > 0)
                                        {
                                            Console.WriteLine("Введите  название вершины");
                                            string name = Console.ReadLine();
                                            Console.WriteLine("Введите  названия вершин, с которыми она будет соединена");
                                            string[] mas = Console.ReadLine().Split(seps, StringSplitOptions.RemoveEmptyEntries);
                                            Dictionary<string,double> l = new Dictionary<string, double>();
                                            for (int i = 0; i < mas.Length-1; i+=2)
                                            {
                                                l.Add(mas[i], double.Parse(mas[i+1]));
                                            }
                                            //if (l.Count == 0)
                                            //    l.Add(name, double.MaxValue);
                                            d.Add(name, l);
                                            n--;
                                        }
                                        foreach (var item in d.Values)
                                        {
                                            foreach (var item2 in item)
                                            {
                                                if (!d.ContainsKey(item2.Key))
                                                {
                                                    throw new ArgumentException("некорректный ввод данных");
                                                }
                                                else
                                                {
                                                    graph = new Graph(d, s);
                                                }
                                            }
                                        }

                                    }

                                }
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;

                        }
                    case '2':
                        {
                            try
                            {
                                if (graph != null)
                                {
                                    Console.WriteLine("Введите название новой вершины");
                                    string vertex = Console.ReadLine();
                                    if (graph.Contains(vertex))
                                    {
                                        throw new ArgumentException("Такая вершина уже есть \n");
                                    }
                                    else
                                    {
                                        graph.AddVertex(vertex);
                                    }
                                }
                                else
                                    throw new NullReferenceException("Графа не существует! \n");
                            }
                            catch (ArgumentException e)
                            {
                                Console.Write(e.Message);
                            }
                            catch (NullReferenceException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    case '3':
                        {
                            try
                            {
                                if (graph != null)
                                {
                                    Console.WriteLine("введите название вершины, которую нужно удалить");
                                    string vertex = Console.ReadLine();
                                    if (!graph.Contains(vertex))
                                    {
                                        throw new ArgumentException("Такой вершины нет  графе!");
                                    }
                                    else
                                    {
                                        graph.DeleteVertex(vertex);
                                    }
                                }
                                else
                                    throw new NullReferenceException("Графа не существует!");

                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (NullReferenceException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    case '4':
                        {
                            try
                            {
                                if (graph != null)
                                {
                                    Console.WriteLine("введите две вершины, которые нужно соединить");
                                    string vertex1 = Console.ReadLine();
                                    string vertex2 = Console.ReadLine();
                                    if (!graph.Contains(vertex1) || !graph.Contains(vertex2))
                                    {
                                        throw new ArgumentException("Такой вершины не существует!");
                                    }
                                    else
                                    {
                                        if (graph.Type.Equals("nn")|| graph.Type.Equals("on"))
                                            graph.AddEdge(vertex1, vertex2);
                                        else
                                        {
                                            Console.WriteLine("введите вес ребра");
                                            double w = double.Parse(Console.ReadLine());
                                            graph.AddEdge(vertex1,vertex2,w);
                                        }
                                    }
                                }
                                else
                                    throw new NullReferenceException("Графа не существует!");
                            }
                            catch (NullReferenceException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    case '5':
                        {
                            try
                            {
                                if (graph != null)
                                {
                                    Console.WriteLine("введите 2 вершины, ребро между которыми надо удалить");
                                    string vertex1 = Console.ReadLine();
                                    string vertex2 = Console.ReadLine();
                                    if (!graph.Contains(vertex1) || !graph.Contains(vertex2))
                                    {
                                        throw new ArgumentException("Такой вершины не существует!");
                                    }
                                    else
                                    {
                                        graph.DeleteEdge(vertex1, vertex2);
                                    }
                                }
                                else
                                    throw new NullReferenceException("Графа не существует!");
                            }
                            catch (NullReferenceException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    
                    case '6':
                        {
                            try
                            {
                                if (graph != null)
                                {
                                    Console.WriteLine(graph.ShowNode());
                                }
                                else
                                    throw new NullReferenceException("Графа не существует!");
                            }
                            catch (NullReferenceException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    case '7':
                        {
                            using (StreamWriter file = new StreamWriter("output.txt"))
                            {
                                file.WriteLine(graph.ShowNode());
                            }
                            break;
                        }
                    case 'q':
                        {
                            flag = false;
                            break;
                        }
                    case 'w':
                        {
                            try
                            {

                                if (graph.Type.Equals("nn"))
                                {
                                    foreach (var item in graph.Nodes)
                                    {
                                        Console.Write(item.Key + ":");
                                        Console.WriteLine(item.Value.Count());
                                    }
                                }
                                else if (graph.Type.Equals("nv"))
                                {
                                    foreach (var item in graph.NodesW)
                                    {
                                        Console.Write(item.Key + ":");
                                        Console.WriteLine(item.Value.Count());
                                    }
                                }
                                else throw new ArgumentException("невозможно выполнить с данным объектом");
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine(e.Message);  
                            }
                            break;
                        }
                    case 'e':
                        {
                            try
                            {

                                if (graph.Type.Equals("nn")|| graph.Type.Equals("nv"))
                                {
                                    throw new NullReferenceException("невозможно выполнить с данным объектом");
                                }
                                else
                                {
                                    Console.WriteLine("введите название вершины");
                                    string name = Console.ReadLine();
                                    string s = "";
                                    if (graph.Nodes.ContainsKey(name) || graph.NodesW.ContainsKey(name))
                                    {
                                        if (graph.NodesW != null)
                                        {
                                            if (graph.NodesW.ContainsKey(name))
                                            {
                                                foreach (var item in graph.NodesW)
                                                {
                                                    if (item.Value.ContainsKey(name))
                                                    {
                                                        s += item.Key + " ";
                                                    }
                                                }
                                            }
                                            Console.WriteLine(s);
                                        }
                                        else if (graph.Nodes != null)
                                        {
                                            if (graph.Nodes.ContainsKey(name))
                                            {
                                                foreach (var item in graph.Nodes)
                                                {
                                                    if (item.Value.Contains(name))
                                                    {
                                                        s += item.Key + " ";
                                                    }
                                                }
                                            }
                                            Console.WriteLine(s);
                                        }
                                    }
                                    else
                                        throw new ArgumentException("Нет вершины");
                                }
                            }
                            catch (NullReferenceException ex) 
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }

                }
            }
        }
    }
}
