using System;
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

        public Dictionary<string,Dictionary<string, double>> NodesW
        { get { return nodesW; } }

        //пустой конструктор
        public Graph()
        {
            this.type = "nn";
            nodes = new Dictionary<string, List<string>>();
        }


        public Graph(Graph graph)
        {
            if (graph.type.Equals("nv") || graph.type.Equals("ov"))
            {
                foreach (var item in graph.nodesW)
                {
                    Dictionary<string, double> d = new Dictionary<string, double>();
                    foreach (var item2 in item.Value)
                    {
                        d.Add(item2.Key, item2.Value);
                    }
                    nodesW.Add(item.Key, d);
                    this.type = graph.type;
                }
            }
            else if (graph.type.Equals("nn") || graph.type.Equals("on"))
            {
                foreach (var item in graph.nodes)
                {
                    List <string> d = new List<string>();
                    foreach (var item2 in item.Value)
                    {
                        d.Add(item2);
                    }
                    nodes.Add(item.Key, d);
                    this.type = graph.type;
                }
            }

        }

        // конструктор на словаре словарей
        public Graph(Dictionary<string, Dictionary<string, double>> a, string s)
        {
            nodesW = new Dictionary<string, Dictionary<string, double>>(a);
            this.type = string.Copy(s);
        }
        // конструктор на словаре листов
        public Graph(Dictionary<string, List<string>> a, string s)
        {
            this.type = string.Copy(s);
            nodes = new Dictionary<string, List<string>>(a);
        }


        public Graph(string name) //конструктор из файла
        {
            using (StreamReader file = new StreamReader(name))
            {
                string s = file.ReadLine();
                type = string.Copy(s);
                int n = int.Parse(file.ReadLine());
                char[] chars = new char[] { ' ', ':', '(', ')','[',']' };
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
                    nodes = new Dictionary<string, List<string>> (a);
                }
                else if (s.Equals("nv")|| s.Equals("ov"))
                {
                    Dictionary<string,Dictionary<string,double>> d = new Dictionary<string,Dictionary<string,double>>();
                    for (int i = 0; i < n; i++)
                    {
                        Dictionary<string,double> b = new Dictionary<string, double>();
                        string[] mas = file.ReadLine().Split(chars, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 1; j < mas.Length-1; j+=2)
                        {
                            b.Add(mas[j], double.Parse(mas[j+1]));
                        }
                        d.Add(mas[0], b);
                    }
                    nodesW = new Dictionary<string, Dictionary<string, double>> (d);
                }
            }
        }



        // метод, добавляющий вершину
        public void AddVertex(string name)
        {
            if (!nodes.ContainsKey(name) && !nodesW.ContainsKey(name))
            {
                if (nodes != null)
                {
                    List<string> a = new List<string>();
                    nodes.Add(name, a);
                }
                else if (nodesW != null)
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
            else if (nodesW!=null)
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
       

    }

}