using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        public static void ShowList(List<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        public static void ShowList(List<double>list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }



        public static Graph CopyGraph_withoutHangVertex(Graph g)
        {
            Graph copyGraph = new Graph(g);
            if (g.Type.Equals("nn"))
            {
                foreach (var item in g.Nodes)
                {
                    if (item.Value.Count == 1)
                    {
                        copyGraph.DeleteVertex(item.Key);
                    }
                }
                return copyGraph;
            }
            else if (g.Type.Equals("nv"))
            {
                foreach (var item in g.NodesW)
                {
                    if (item.Value.Count == 1)
                    {
                        copyGraph.DeleteVertex(item.Key);
                    }
                }
                return copyGraph;
            }
            else if (g.Type.Equals("on"))
            {
                foreach (var item in g.Nodes)
                {
                    int count = 0;
                    if (item.Value.Count == 0)
                    {
                        foreach (var item2 in g.Nodes.Values)
                        {
                            foreach (var item3 in item2)
                            {
                                if (item3.Equals(item.Key))
                                    count++;
                            }
                        }
                    }
                    if (count==1)
                        copyGraph.DeleteVertex(item.Key);
                }
                return copyGraph;
            }
            else if (g.Type.Equals("ov"))
            {
                foreach (var item in g.NodesW)
                {
                    int count = 0;
                    if (item.Value.Count == 0)
                    {
                        foreach (var item2 in g.Nodes.Values)
                        {
                            foreach (var item3 in item2)
                            {
                                if (item3.Equals(item.Key))
                                    count++;
                            }
                        }
                    }
                    if (count == 1)
                        copyGraph.DeleteVertex(item.Key);
                }
                return copyGraph;
            }
            else
            {
                throw new ArgumentException("нет такого типа графа");
            }
        }

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
                    "r - задание 12: построить граф, полученный из исходного удалением висячих вершин \n"+
                    "t - задание 7: Корнем ацикличного орграфа называется такая вершина u, что из неё существуют пути в каждую \n"+
                    " из остальных вершин орграфа. Определить, имеет ли данный ацикличный орграф корень. \n"+
                    "y - задание 27: Найти длины кратчайших (по числу дуг) путей из вершины u во все остальные \n" +
                    "u - Краскал: дан взвешенный неориентированный граф из N вершин и M ребер. Требуется найти в нем каркас минимального веса\n" +
                    "i - задание 16: Вывести кратчайшие пути до вершины u из всех остальных вершин. \n" +
                    "o - задание 4: Вывести длины кратчайших путей от u до v1 и v2 \n" +
                    "p - задание 1: Определить, существует ли путь длиной не более L между двумя заданными вершинами графа. \n" +
                    "[ - Потоки: Решить задачу на нахождение максимального потока любым алгоритмом \n" +
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
                    case 'r':
                        {
                            try
                            {
                                Graph graph1 = CopyGraph_withoutHangVertex(graph);
                                Console.WriteLine("Измененный граф:");
                                Console.WriteLine(graph1.ShowNode());

                                Console.WriteLine("Исходный граф:");
                                Console.WriteLine(graph.ShowNode());
                            }
                            catch (NullReferenceException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch(ArgumentException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    case 't': 
                        {
                            try
                            {
                                ///
                                if (graph!=null)
                                {
                                        if (graph.Nodes != null)
                                        {
                                            foreach (var item in graph.Nodes)
                                            {
                                                graph.NovSet();
                                                bool IsD = true;
                                                graph.Dfs(item.Key);
                                                
                                                if (graph.nov.ContainsValue('w'))
                                                {
                                                    IsD = false;
                                                }
                                                if (IsD)
                                                {
                                                    Console.WriteLine("Корень найден");
                                                    Console.WriteLine(item.Key);
                                                    break;
                                                }

                                            }
                                            

                                        }
                                        else if (graph.NodesW != null)
                                        {
                                            foreach (var item in graph.NodesW)
                                            {
                                                graph.NovSet();
                                                bool IsD = true;
                                                graph.Dfs(item.Key);
                                                
                                                foreach (var item2 in graph.nov)
                                                {
                                                    if (item2.Value == 'w')
                                                    {
                                                        IsD = false;
                                                        break;
                                                    }
                                                }
                                                if (IsD)
                                                {
                                                    Console.WriteLine("Корень найден");
                                                    Console.WriteLine(item.Key);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                /////
                            }
                            catch (NullReferenceException e) 
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (Exception e)
                            { Console.WriteLine(e.Message); }
                            break;
                        }
                    case 'y':
                        {
                            graph.NovSetForUsed();
                            string s = Console.ReadLine();
                            graph.Bfs(s);
                            break;
                        }
                    case 'u':
                        {
                            graph.Krascal();
                            break;
                        }
                    case 'i':
                        {
                            string s = Console.ReadLine();
                            Stack<string> items = new Stack<string>();
                            Console.WriteLine("Расстояние от вершин до вершины {0}", s);
                            foreach (var item in graph.NodesW)
                            {
                                if (!item.Key.Equals(s))
                                {
                                    Console.WriteLine("от {0} ", item.Key);
                                    List<double> d2 = graph.Dijkstra(item.Key, s, out List<string> p);
                                    if (d2[graph.verts.IndexOf(s)] != double.MaxValue)
                                    {
                                        graph.WayDijkstr(item.Key, s, p, ref items);
                                        foreach (var item2 in items)
                                        {
                                            Console.Write(item2 + "\n");
                                        }
                                    }
                                    else
                                        Console.WriteLine("Нет пути");
                                }
                                items.Clear();
                            }
                            break;
                        }
                    case 'o':
                        {
                            Console.WriteLine("От какой вершины");
                            string u = Console.ReadLine();
                            Console.WriteLine("до каких 2 вершин");
                            string v1 = Console.ReadLine();
                            string v2 = Console.ReadLine();
                            graph.BellmanFord(u,v1,v2);
                            break;
                        }
                    case 'p':
                        {
                            Console.WriteLine("между какими 2 вершинами?");
                            string u = Console.ReadLine();
                            string v = Console.ReadLine();
                            Console.WriteLine("Введите L");
                            int l = int.Parse(Console.ReadLine());

                            Dictionary<string, Dictionary<string, string>> p = new Dictionary<string, Dictionary<string, string>>();
                            Dictionary<(string, string), double> distance = graph.Floyd(p);



                            if (distance.Count > 0)
                            {
                                bool flag2 = true;
                                foreach (var item in distance.Keys)
                                {
                                    if (item.Item1.Equals(u) && item.Item2.Equals(v) && distance[(u, v)] <= l)
                                    {
                                        graph.Floyd_Warshall_Way(u,v);
                                        flag2 = false;
                                    }
                                    else if (item.Item1.Equals(v) && item.Item2.Equals(u) && distance[(v, u)] <= l)
                                    {
                                        graph.Floyd_Warshall_Way(v, u);
                                        flag2 = false;
                                    }
                                }
                                if (flag2)
                                {
                                    Console.WriteLine("Нет пути");
                                }
                            }
                          

                            break;
                        }
                    case '[':
                        {
                            Console.WriteLine("введите 2 вершины");
                            string u = Console.ReadLine();
                            string v = Console.ReadLine();
                            Console.WriteLine(graph.FordFulkerson(u, v,graph));
                            break;
                        }
                }
            }
        }
    }
}
