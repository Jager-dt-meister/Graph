using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class Graph
    {

        Dictionary<string, Dictionary<string, double>> nodesW;
        Dictionary<string, List<string>> nodes;
        string type;

        public string Type
        { get { return type; } }

        public Dictionary<string, List<string>> Nodes
        { get { return nodes; } }

        public Dictionary<string, Dictionary<string, double>> NodesW
        { get { return nodesW; } }


        // индексатор для невзвешенного графа
        //public KeyValuePair<string, List<string>> this[int i]
        //{
        //    get
        //    {
        //        int i1 = 0;
        //        foreach (var item in nodes)
        //        {
        //            if ( i1==i)
        //            {
        //                return item;
        //            }
        //            i1++;
        //        }
        //        KeyValuePair<string, List<string>> a = new KeyValuePair<string, List<string>>(null,null);
        //        return a;
        //    }
        //    set
        //    {

        //    }
        //}



        //пустой конструктор
        public Graph()
        {
            this.type = "nn";
            this.nodes = new Dictionary<string, List<string>>();

            this.nodesW = new Dictionary<string, Dictionary<string, double>>();

            this.NovSet();
        }


        public Graph(string s1, string s)
        {
            this.type = string.Copy(s);
            if (this.type.Equals("nn") || this.type.Equals("on"))
                this.nodes = new Dictionary<string, List<string>>();
            else if (this.type.Equals("nv") || this.type.Equals("ov"))
                this.nodesW = new Dictionary<string, Dictionary<string, double>>();

            this.NovSet();
        }


        public Graph(Graph graph)
        {
            Graph graph1 = new Graph();
            if (graph.type.Equals("nv") || graph.type.Equals("ov"))
            {
                foreach (var item in graph.nodesW)
                {
                    Dictionary<string, double> d = new Dictionary<string, double>();
                    foreach (var item2 in item.Value)
                    {
                        d.Add(item2.Key, item2.Value);
                    }
                    graph1.nodesW.Add(item.Key, d);
                    graph1.type = string.Copy(graph.type);

                }
            }
            else if (graph.type.Equals("nn") || graph.type.Equals("on"))
            {
                foreach (var item in graph.nodes)
                {
                    List<string> d = new List<string>();
                    foreach (var item2 in item.Value)
                    {
                        d.Add(item2);
                    }
                    graph1.nodes.Add(item.Key, d);
                    graph1.type = string.Copy(graph.type);
                }
            }
            this.nodes = graph1.nodes;
            this.nodesW = graph1.nodesW;
            this.type = graph1.type;
        }

        // конструктор на словаре словарей
        public Graph(Dictionary<string, Dictionary<string, double>> a, string s)
        {
            nodesW = new Dictionary<string, Dictionary<string, double>>(a);
            this.type = string.Copy(s);
            this.NovSet();
        }
        // конструктор на словаре листов
        public Graph(Dictionary<string, List<string>> a, string s)
        {
            this.type = string.Copy(s);
            nodes = new Dictionary<string, List<string>>(a);
            this.NovSet();
        }


        public Graph(string name) //конструктор из файла
        {
            using (StreamReader file = new StreamReader(name))
            {
                string s = file.ReadLine();
                type = string.Copy(s);
                int n = int.Parse(file.ReadLine());
                char[] chars = new char[] { ' ', ':', '(', ')', '[', ']' };
                if (s.Equals("nn") || s.Equals("on"))
                {
                    Dictionary<string, List<string>> a = new Dictionary<string, List<string>>();
                    for (int i = 0; i < n; i++)
                    {
                        List<string> b = new List<string>();
                        string[] mas = file.ReadLine().Split(chars, StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 1; j < mas.Length; j++)
                        {
                            b.Add(mas[j]);
                        }
                        a.Add(mas[0], b);
                    }
                    nodes = new Dictionary<string, List<string>>(a);
                }
                else if (s.Equals("nv") || s.Equals("ov"))
                {
                    Dictionary<string, Dictionary<string, double>> d = new Dictionary<string, Dictionary<string, double>>();
                    for (int i = 0; i < n; i++)
                    {
                        Dictionary<string, double> b = new Dictionary<string, double>();
                        string[] mas = file.ReadLine().Split(chars, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 1; j < mas.Length - 1; j += 2)
                        {
                            b.Add(mas[j], double.Parse(mas[j + 1]));
                        }
                        d.Add(mas[0], b);
                    }
                    nodesW = new Dictionary<string, Dictionary<string, double>>(d);
                }
                this.NovSet();
            }
        }



        // метод, добавляющий вершину
        public void AddVertex(string name)
        {
            if (nodes != null)
            {
                if (!nodes.ContainsKey(name))
                {
                    List<string> a = new List<string>();
                    nodes.Add(name, a);
                }
            }
            else if (nodesW != null)
            {
                if (!nodesW.ContainsKey(name))
                {
                    Dictionary<string, double> a = new Dictionary<string, double>();
                    nodesW.Add(name, a);
                }
            }
            else
                throw new ArgumentException("такая вершина уже существует");
        }

        // метод, удаляющий вершину
        public void DeleteVertex(string vertex)
        {
            if (nodes.ContainsKey(vertex) || nodesW.ContainsKey(vertex))
            {
                if (nodes != null)
                {
                    nodes.Remove(vertex);
                    foreach (var item in nodes.Values)
                    {
                        item.Remove(vertex);
                    }
                }
                else if (nodesW != null)
                {
                    nodesW.Remove(vertex);

                    foreach (var item in nodesW.Values)
                    {
                        item.Remove(vertex);
                    }
                }
            }
            else
                throw new ArgumentException("такая вершина не существует");
        }


        // метод, добавляющий ребро
        public void AddEdge(string vertex, string vertex2)
        {
            if (nodes.ContainsKey(vertex) && nodes.ContainsKey(vertex2))
            {
                if (this.type == "nn")
                {
                    nodes[vertex].Add(vertex2);
                    nodes[vertex2].Add(vertex);
                }
                else
                {
                    nodes[vertex].Add(vertex2);
                }
            }
            else
                throw new ArgumentException("Такой вершины не существует!");
        }

        public void AddEdge(string vertex, string vertex2, double w)
        {
            if (nodesW.ContainsKey(vertex) && nodesW.ContainsKey(vertex2))
            {
                if (this.type == "nv")
                {
                    nodesW[vertex].Add(vertex2, w);
                    nodesW[vertex2].Add(vertex, w);
                }
                else
                {
                    nodesW[vertex].Add(vertex2, w);
                }
            }
            else
                throw new ArgumentException("Такой вершины не существует!");
        }

        // удаление ребра
        public void DeleteEdge(string vertex, string vertex2)
        {
            if (nodes != null)
            {
                if (nodes.ContainsKey(vertex) && nodes.ContainsKey(vertex2))
                {
                    if (this.type.Equals("nn"))
                    {
                        nodes[vertex].Remove(vertex2);
                        nodes[vertex2].Remove(vertex);
                    }
                    else
                    {
                        nodes[vertex].Remove(vertex2);
                    }
                }
                else
                    throw new ArgumentException("такой вершины не существует");
            }
            else if (nodesW != null)
            {
                if (nodesW.ContainsKey(vertex) && nodesW.ContainsKey(vertex2))
                {
                    if (this.type.Equals("nv"))
                    {
                        nodesW[vertex].Remove(vertex2);
                        nodesW[vertex2].Remove(vertex);
                    }
                    else
                    {
                        nodesW[vertex].Remove(vertex2);
                    }
                }
                else
                    throw new ArgumentException("такой вершины не существует");
            }
            else
                throw new ArgumentException("Ничего нет");
        }

        public string ShowNode()
        {
            string s = "";
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    s += item.Key + ":";
                    foreach (var item2 in item.Value)
                    {
                        if (!item2.Equals(item.Key))
                            s += item2 + " ";
                    }
                    s += "\n";
                }
            }
            else
            {
                foreach (var item in nodesW)
                {
                    s += item.Key + ":";
                    foreach (var item2 in item.Value)
                    {
                        s += item2.ToString() + " ";
                    }
                    s += "\n";
                }
            }
            return s;
        }

        public bool Contains(string vertex)
        {
            if (nodes != null)
            {
                if (nodes.ContainsKey(vertex))
                    return true;
                else
                    return false;
            }
            else if (nodesW != null)
            {
                if (nodesW.ContainsKey(vertex))
                    return true;
                else
                    return false;
            }
            return false;
        }


        public Dictionary<string, char> nov;
       
        ///
        public void NovSet()
        {
            nov = new Dictionary<string, char>();
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    nov.Add(item.Key, 'w');
                }
            }
            else
            {
                foreach (var item in nodesW)
                {
                    nov.Add(item.Key, 'w');
                }
            }
        }
        // обход в глубину
        public void Dfs(string vertex)
        {
            if (type.Equals("on") || type.Equals("ov"))
            {
                if (nodes != null)
                {
                    nov[vertex] = 'g';
                    foreach (var item in nodes[vertex])
                    {
                        if (nov[item] == 'w')
                        {
                            Dfs(item);
                        }
                        else if (nov[item] == 'g')
                        {
                            throw new Exception("Не ацикличен");
                        }
                    }
                    nov[vertex] = 'b';
                }
                else if (nodesW != null)
                {
                    nov[vertex] = 'g';
                    foreach (var item in nodesW[vertex])
                    {
                        if (nov[item.Key] == 'w')
                        {
                            Dfs(item.Key);
                        }
                        else if (nov[item.Key] == 'g')
                        {
                            throw new Exception("Не ацикличен");
                        }
                    }
                    nov[vertex] = 'b';
                }
            }
            else
            {
                string prev = string.Copy(vertex);
                if (nodes != null)
                {
                    nov[vertex] = 'b';
                    foreach (var item in nodes[vertex])
                    {
                        if (nov[item] == 'w')
                        {
                            Dfs(item);
                        }
                        else if (!item.Equals(prev))
                        {
                            throw new Exception("Не ацикличен");
                        }
                    }
                }
                else if (nodesW != null)
                {
                    nov[vertex] = 'b';
                    foreach (var item in nodesW[vertex])
                    {
                        if (nov[item.Key] == 'w')
                        {
                            Dfs(item.Key);
                        }
                        else if (!item.Key.Equals(prev))
                        {
                            throw new Exception("Не ацикличен");
                        }
                    }
                }
            }
        }


        public List<bool> notused;
        public void NovSetForUsed()
        {
            notused = new List<bool>();
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    notused.Add(true);
                }
            }
            else
            {
                foreach (var item in nodesW)
                {
                    notused.Add(true);
                }
            }
        }
        public void Bfs(string v)
        {
            string start = v;
            if (nodes != null)
            {
                int[] dist = new int[nodes.Count];
                for (int j = 0; j < nodes.Count; j++)
                {
                    dist[j] = 0;
                }

                int i = nodes.Keys.ToList<string>().IndexOf(v);
                Queue<string> q = new Queue<string>();
                q.Enqueue(v);
                notused[i] = false;
                while (q.Count != 0)
                {
                    v = q.Dequeue();
                    foreach (string item in nodes[v])
                    {
                        if (notused[nodes.Keys.ToList<string>().IndexOf(item)])
                        {
                            dist[nodes.Keys.ToList<string>().IndexOf(item)] += dist[nodes.Keys.ToList<string>().IndexOf(v)] + 1;

                            q.Enqueue(item);
                            Console.WriteLine("Вершину {0} и вершину {1} разделяют рёбер: {2}", start, item, dist[nodes.Keys.ToList<string>().IndexOf(item)]);
                            notused[nodes.Keys.ToList<string>().IndexOf(item)] = false;
                        }
                    }
                }
                foreach (var item2 in nodes)
                {
                    if ((item2.Key != start) && notused[nodes.Keys.ToList<string>().IndexOf(item2.Key)])
                    {
                        Console.WriteLine("Из вершины {0} в вершину {1} нет пути", start, item2.Key);
                    }
                }

            }
            else if (nodesW != null)
            {
                int[] dist = new int[nodesW.Count];
                for (int j = 0; j < nodesW.Count; j++)
                {
                    dist[j] = 0;
                }

                int i = nodesW.Keys.ToList<string>().IndexOf(v);
                Queue<string> q = new Queue<string>();
                q.Enqueue(v);
                notused[i] = false;
                while (q.Count != 0)
                {
                    v = q.Dequeue();
                    foreach (var item in nodesW[v])
                    {
                        if (notused[nodesW.Keys.ToList<string>().IndexOf(item.Key)])
                        {
                            dist[nodesW.Keys.ToList<string>().IndexOf(item.Key)] += dist[nodesW.Keys.ToList<string>().IndexOf(v)] + 1;

                            q.Enqueue(item.Key);
                            Console.WriteLine("Вершину {0} и вершину {1} разделяют рёбер: {2}", start, item.Key, dist[nodesW.Keys.ToList<string>().IndexOf(item.Key)]);
                            notused[nodesW.Keys.ToList<string>().IndexOf(item.Key)] = false;
                        }
                    }
                }
            }
        }

        // краскал  нет в отчете
        public List<(double weight, string a, string b)> edges = new List<(double, string, string)>();

        public void Krascal()
        {
            if (type.Equals("nv"))
            {
                foreach (var item in nodesW)
                {
                    string a = item.Key;
                    foreach (var item2 in nodesW[a])
                    {
                        string b = item2.Key;
                        if (!(edges.Contains((nodesW[a][b], a, b)) || edges.Contains((nodesW[a][b], b, a))))
                            edges.Add((nodesW[a][b], a, b));
                    }
                }
                edges.Sort(); // сортировка по весу

                Graph g_min = new Graph("тип", "nv");

                foreach (var item in nodesW)
                {
                    g_min.AddVertex(item.Key);
                }
                g_min.NovSet();

                foreach ((double weight, string a, string b) item in edges)
                {

                    g_min.AddEdge(item.a, item.b, item.weight);

                    if (g_min.hasCycles(item.a, " "))
                    {
                        g_min.DeleteEdge(item.a, item.b);
                    }
                    g_min.NovSet();

                }
                Console.WriteLine(g_min.ShowNode());
            }
            else
                Console.WriteLine("Граф не удовл. условиям неориентированности и взвешенности");
        }

        public bool hasCycles(string s, string s2)
        {
            nov[s] = 'b';
            foreach (var item in nodesW[s])
            {
                if (nov[item.Key] == 'w')
                {
                    hasCycles(item.Key, s);
                }
                else if (item.Key != s2)
                {
                    return true;
                }
            }
            return false;
        }



        public List<string> verts = new List<string>();
        public List<double> Dijkstra(string v, string stop, out List<string> p)
        {
            List<double> dist = new List<double>();
            foreach (var item in nodesW)
            {
                verts.Add(item.Key);
            }
            for (int i = 0; i < nodesW.Count; i++)
            {
                if (i != verts.IndexOf(v))
                    dist.Add(double.MaxValue);
                else
                    dist.Add(0);
            }
            NovSetForUsed();
            p = new List<string>();
            for (int i = 0; i < verts.Count; i++)
            {
                if (i != verts.IndexOf(v))
                    p.Add(v);
                else
                    p.Add("0");
            }
            for (int i = 0; i < nodesW.Count; i++)
            {
                string w = "";
                double min = double.MaxValue;
                for (int j = 0; j < nodesW.Count; j++)
                {
                    if (notused[j] && min > dist[j])
                    {
                        min = dist[j];
                        w = verts[j];
                    }
                }
                if (w == "")
                {
                    continue;
                }
                notused[verts.IndexOf(w)] = false;

                for (int j = 0; j < nodesW.Count; j++)
                {
                    double distance;
                    if (nodesW[w].ContainsKey(verts[j]))
                    {
                        distance = dist[verts.IndexOf(w)] + nodesW[w][verts[j]];
                        if (notused[j] && dist[j] > distance)
                        {
                            dist[j] = distance;
                            p[j] = w;
                        }
                    }
                }
            }

            return dist;
        } // алгоритм Дейкстры




        public void WayDijkstr(string a, string b, List<string> p, ref Stack<string> items)
        {
            items.Push(b);
            if (a == p[verts.IndexOf(b)])
            {
                items.Push(a);
            }
            else
            {
                WayDijkstr(a, p[verts.IndexOf(b)], p, ref items);
            }
        }








        public List<double> BellmanFord(string v, string u1, string u2)
        {
            foreach (var item in nodesW)
            {
                verts.Add(item.Key);
            }


            List<double> dist = new List<double>();
            string cur, next;

            for (int i = 0; i < nodesW.Count; i++)
            {
                if (i != verts.IndexOf(v))
                    dist.Add(double.MaxValue);
                else
                    dist.Add(0);
            }

            for (int k = 0; k < nodesW.Count - 1; k++)
            {
                for (int i = 0; i < nodesW.Count; i++)
                {
                    cur = verts[i];
                    for (int j = 0; j < nodesW.Count; j++)
                    {
                        next = verts[j];
                        if (i != j && nodesW[cur].ContainsKey(next))
                        {
                            if (dist[i] != double.MaxValue && dist[j] > dist[i] + nodesW[cur][next])
                            {
                                dist[j] = dist[i] + nodesW[cur][next];
                            }
                        }
                    }
                }
            }

            foreach (var item in nodesW)
            {
                foreach (var item2 in item.Value)
                {
                    if (dist[verts.IndexOf(item.Key)] + item2.Value < dist[verts.IndexOf(item2.Key)])
                    {
                        Console.WriteLine("Есть отрицательный цикл");
                        return null;
                    }
                }
            }



            for (int i = 0; i < dist.Count; i++)
            {
                if (i == verts.IndexOf(u1))
                    Console.WriteLine("расстояние до {0} = {1} ", u1, dist[i]);
                if (i == verts.IndexOf(u2))
                    Console.WriteLine("расстояние до {0} = {1} ", u2, dist[i]);
            }
            return dist;
        } // алгоритм Беллмана-Форда





        public Dictionary<(string, string), double> Floyd(Dictionary<string, Dictionary<string, string>> p)
        {
            foreach (var element in NodesW.Keys)
            {
                p.Add(element, new Dictionary<string, string>());
                foreach (var second_element in NodesW.Keys)
                {
                    if (NodesW[element].ContainsKey(second_element))
                    {
                        p[element].Add(second_element, second_element);
                    }
                    else
                    {
                        p[element].Add(second_element, null);
                    }
                }
            }
            Dictionary<(string, string), double> distance = new Dictionary<(string, string), double>();
            foreach (var item in nodesW)
            {
                foreach (var item2 in nodesW)
                {
                    if (item.Key.Equals(item2.Key))
                        distance[(item.Key, item2.Key)] = 0;
                    else if (nodesW.ContainsKey(item.Key) && nodesW[item.Key].ContainsKey(item2.Key))
                        distance[(item.Key, item2.Key)] = nodesW[item.Key][item2.Key];
                    else
                        distance[(item.Key, item2.Key)] = double.MaxValue;
                }
            }

            foreach (var k in nodesW)
            {
                foreach (var item in nodesW)
                {
                    foreach (var item2 in nodesW)
                    {
                        if (distance.ContainsKey((k.Key, item.Key)) && distance.ContainsKey((k.Key, item2.Key)) && (distance.ContainsKey((item.Key, item2.Key)) || item.Key == item2.Key)
                            && distance[(item.Key, k.Key)] != double.MaxValue && distance[(k.Key, item2.Key)] != double.MaxValue && distance[(item.Key, k.Key)] + distance[(k.Key, item2.Key)] < distance[(item.Key, item2.Key)])
                        {
                            distance[(item.Key, item2.Key)] = distance[(item.Key, k.Key)] + distance[(k.Key, item2.Key)];
                            p[item.Key][item2.Key] = p[item.Key][k.Key];
                        }
                    }
                }
            }

            foreach (var item in distance.Keys)
            {
                string i = item.Item1;
                string j = item.Item2;
                if (distance[(i, j)] != int.MaxValue && distance[(j, j)] < 0 && distance[(j, i)] != int.MaxValue)
                {
                    
                    break;
                }
            }
            return distance;
        }

        public void Floyd_Warshall_Way(string u, string v)
        {
            Dictionary<string, Dictionary<string, string>> p = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<(string, string), double> distance = Floyd(p);
            Dictionary<string,int> count = new Dictionary<string, int>();
            if (distance[(u,v)] == double.MaxValue)
            {
                Console.WriteLine("No way");
                return;
            }
            string c = u;
            count.Add(c, 0);
            count.Add(v, 0);
            Console.WriteLine("путь из {0} в {1}:",u,v);
            while (c != v || (count.ContainsKey(c) && count[c]<=1))
            {
                if (c==v && count[c]==1)
                {
                    break;
                }
                if (count.ContainsKey(c))
                {
                    if (count[c] <= 1)
                    {
                        count[c] += 1;
                        Console.WriteLine(c);
                        c = p[c][v];
                    }
                    else
                        break;
                }
                else
                {
                    count.Add(c, 1);
                    Console.WriteLine(c);
                    c = p[c][v];
                }
            }
            Console.WriteLine(v);
        }


        public double FordFulkerson(string source, string sink, Graph g)
        {
            // остаточная сеть
            Graph residualGraph = new Graph(g) ;

            // Словарь для хранения предков
            Dictionary<string, string> p = new Dictionary<string, string>();

            double maxFlow = 0;

            // Пока существует путь из источника в сток
            while (Bfs(residualGraph, source, sink, p))
            {
                double pathFlow = double.MaxValue;

                // Находим минимальную пропускную способность в пути
                string current = sink;
                while (current != source)
                {
                    string prev = p[current];
                    pathFlow = Math.Min(pathFlow, residualGraph.nodesW[prev][current]);
                    current = prev;
                }

                // Обновляем остаточную сеть и увеличиваем поток
                current = sink;
                while (current != source)
                {
                    string prev = p[current];
                    residualGraph.nodesW[prev][current] -= pathFlow;
                    if (!residualGraph.nodesW[current].ContainsKey(prev))
                        residualGraph.AddEdge(current, prev,0);
                    residualGraph.nodesW[current][prev] += pathFlow;
                    current = prev;
                }

                maxFlow += pathFlow;
            }

            return maxFlow;
        }

        // поиск пути по в ширину
        private bool Bfs(Graph residualGraph, string source, string sink, Dictionary<string, string> p)
        {
            List<string> visited = new List<string>();
            Queue<string> queue = new Queue<string>();

            queue.Enqueue(source);
            visited.Add(source);

            while (queue.Count > 0)
            {
                string u = queue.Dequeue();

                foreach (var item in residualGraph.nodesW[u])
                {
                    if (!visited.Contains(item.Key) && item.Value > 0)
                    {
                        queue.Enqueue(item.Key);
                        visited.Add(item.Key);
                        p[item.Key] = u;

                        if (item.Key == sink)
                            return true;
                    }
                }
            }

            return false;
        }


    }
}