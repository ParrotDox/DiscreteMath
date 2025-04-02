using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Lab4
{
    //This class is for processing matrixes
    public class MatrixSolver
    {
        public List<List<int>> matrix;
        public List<List<int>> complementGraph = new List<List<int>>();
        public string[] routes;
        public List<int> ostov;
        public List<int> clique;

        public MatrixSolver() 
        {
            matrix = InitMatrix();
        }
        public MatrixSolver(List<List<int>> m)
        {
            matrix = InitMatrix(m);
        }

        public void PrintMatrix() 
        {
            int n = matrix.Count;
            //MATRIX
            Console.WriteLine("Your matrix:");
            Console.Write(" ".PadLeft(5));
            for (int i = 0; i < n; ++i)
            {
                Console.Write($"{((char)('a' + i)).ToString().PadLeft(5)}");
            }
            Console.Write('\n');
            for (int i = 0; i < n; ++i)
            {
                Console.Write($"{((char)('a' + i)).ToString().PadLeft(5)}");
                foreach (int col in matrix[i])
                {
                    Console.Write(col.ToString().PadLeft(5));
                }
                Console.WriteLine();
            }
        }
        public void PrintInfo() 
        {
            //MATRIX
            PrintMatrix();

            //DIJKSTRA
            if (routes != null) 
            {
                Console.WriteLine("Dijkstra Routes:");
                for (int i = 0; i < routes.Length; i++)
                {
                    Console.Write($"{(char)('a' + i)}) ");
                    string[] splitted = routes[i].Split(' ');
                    for (int j = 0; j < splitted.Length; j++) 
                    {
                        int vertex = int.Parse(splitted[j]);
                        Console.Write($" {(char)('a' + vertex)} ");
                    }
                    Console.Write('\n');
                }
            }
            else 
            {
                Console.WriteLine("Dijkstra Routes: not initialized");
            }


            //PRIMA
            if (ostov != null) 
            {
                Console.WriteLine("Ostov (1 - route | 2 - length):");
                Console.Write("1)");
                for (int i = 0; i < ostov.Count; i++)
                {
                    if (i == ostov.Count - 1)
                    {
                        Console.Write($"{(char)('a' + ostov[i])}");
                    }
                    else 
                    {
                        Console.Write($"{(char)('a' + ostov[i])} ->");
                    }
                }
                Console.Write('\n');
                Console.Write("2)");
                int len = 0;
                for (int i = 0; i < ostov.Count - 1; i++)
                {
                    len += matrix[ostov[i]][ostov[i + 1]];
                }
                Console.WriteLine($"{len}");
            }
            else 
            {
                Console.WriteLine("Ostov: not initialized");
            }
            

            //CLIQUE
            if(clique != null) 
            {
                Console.WriteLine("Max Clique:");
                for (int i = 0; i < clique.Count; i++)
                {
                    if (i == clique.Count - 1)
                    {
                        Console.Write($"{(char)('a' + clique[i])}");
                    }
                    else 
                    {
                        Console.Write($"{(char)('a' + clique[i])} ->");
                    }
                }
                Console.Write('\n');
            }
            else 
            {
                Console.WriteLine("Max Clique: not initialized");
            }
        }
        public List<List<int>> InitMatrix() 
        {
            Console.WriteLine("Input matrix size: ");
            int size = int.Parse(Console.ReadLine());
            List<List<int>> matrixTemp = new List<List<int>>();
            for (int i = 0; i < size; i++) 
            {
                Console.WriteLine("Input matrix values in line {example: 1 12 3}:\n ");
                string[] vals = Console.ReadLine().Split(' ');
                List<int> row = new List<int>();
                foreach (string val in vals) 
                {
                    row.Add(int.Parse(val));
                }
                matrixTemp.Add(row);
            }
            return matrixTemp;
        }
        public List<List<int>> InitMatrix(List<List<int>> sample)
        {
            List<List<int>> matrixTemp = new List<List<int>>();
            for(int i = 0; i < sample.Count; i++) 
            {
                List<int> row = new();
                for(int j = 0; j < sample[i].Count; j++) 
                {
                    row.Add(sample[i][j]);
                }
                matrixTemp.Add(row);
            }
            return matrixTemp;
        }
        public string[] Dijkstra() 
        {
            int n = matrix.Count;
            bool[] visited = new bool[n];
            double[] distances = new double[n];
            string[] routes = new string[n];

            for (int i = 0; i < n; i++) { distances[i] = double.PositiveInfinity; }

            Console.WriteLine("Dijkstra: Input start vertex: ");
            int startVertex = Console.ReadLine().ToLower()[0] - 'a';
            //Console.WriteLine("Dijkstra: Input end vertex: ");
            //int endVertex = Console.ReadLine().ToLower()[0] - 'a';

            //First initialization
            int currentVertex = startVertex;
            distances[currentVertex] = 0;
            routes[currentVertex] = currentVertex.ToString();

            while (!visited.All(x => x == true)) 
            {
                //Choose not visited min.vertex
                double minDistance = double.PositiveInfinity;
                for (int i = 0; i < n; i++)
                {
                    if (visited[i] == false && distances[i] < minDistance)
                    {
                        minDistance = distances[i];
                        currentVertex = i;
                    }
                }
                visited[currentVertex] = true;
                for (int i = 0; i < n; i++)
                {
                    //Checking connections from chosen vertex to other vertexes
                    if (matrix[currentVertex][i] != 0)
                    {
                        //If the path length of other vertex is higher than path from cur.vertex
                        //then change the path to the second one

                        if (distances[i] > distances[currentVertex] + matrix[currentVertex][i])
                        {
                            distances[i] = distances[currentVertex] + matrix[currentVertex][i];
                            routes[i] = routes[currentVertex] + ' ' + i;
                        }
                    }
                }
            }
            return routes;
        }
        public List<int> Prima()
        {
            int n = matrix.Count;
            //Checking if prima can be used for this graph
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i][j] != matrix[j][i])
                        return null;
                }
            }

            bool[] visited = new bool[n];
            List<int> ostov = new();

            Console.WriteLine("Prima: Input start vertex: ");
            int startVertex = Console.ReadLine().ToLower()[0] - 'a';

            //First initialization
            visited[startVertex] = true;
            ostov.Add(startVertex);
            while (!visited.All(x => x == true)) 
            {
                double minDistance = double.PositiveInfinity;
                int to = -1;
                for(int i = 0; i < n; i++) 
                {
                    if (visited[i])
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (!visited[j] && matrix[i][j] > 0 && matrix[i][j] < minDistance)
                            {
                                minDistance = matrix[i][j];
                                to = j;
                            }
                        }
                    }
                }
                visited[to] = true;
                ostov.Add(to);
            }
            return ostov;
        }
        public List<int> Clique() 
        {
            int n = matrix.Count;

            //Creating complementation of the original graph
            for (int i = 0; i < n; i++) 
            {
                complementGraph.Add(new List<int>());
                for(int j = 0; j < n; j++) 
                {
                    //If is not looping and no edge has found
                    if(i!=j && matrix[i][j] == 0) 
                    {
                        complementGraph[i].Add(j);
                    }
                }
            }
            // 5 6 0    null null 0
            // 3 0 1 -> null 0    null
            // 3 6 8    null null nullА
            List<int> maxClique = new List<int>();
            List<int> current = new List<int>();
            BackTrack(0, current, maxClique);
            return maxClique;
        }
        public void BackTrack(int start, List<int> current, List<int> maxClique) 
        {
            int n = matrix.Count;
            if(current.Count > maxClique.Count) 
            {
                maxClique.Clear();
                maxClique.AddRange(current);
            }

            //Checking the availability of adding vertex to clique.
            //For example: I can't add vertex that is in the complementation
            for(int i = start; i < n; i++) 
            {
                bool canAdd = true;
                //Is in complementation
                foreach (int v in current)
                {
                    if (complementGraph[i].Contains(v)) 
                    {
                        canAdd = false;
                        break;
                    }
                }
                if (canAdd) 
                {
                    current.Add(i);
                    BackTrack(i+1, current, maxClique);
                    //Removing current vertex to check other variants
                    current.RemoveAt(current.Count - 1);
                }
            }
        }
    }

    //This class is for user
    public class MatrixUI 
    {
        static public List<List<List<int>>> matrixes = new List<List<List<int>>>()
        {
            new List<List<int>>() { new List<int> { 0, 4, 0, 0, 1}, new List<int> { 4, 0, 3, 0, 2}, new List<int> { 0, 3, 0, 5, 0}, new List<int> { 0, 0, 5, 0, 3}, new List<int> { 1, 2, 0, 3, 0} },
            new List<List<int>>() { new List<int> { 0, 5, 0, 0 }, new List<int> { 0, 0, 3, 2 }, new List<int> { 0, 0, 0, 4 }, new List<int> { 0, 0, 0, 0 } },
            new List<List<int>>() { new List<int> { 0, 2, 3}, new List<int> { 2, 0, 1 }, new List<int> { 3, 1, 0 } }
        };
        public List<MatrixSolver> solvers = new List<MatrixSolver>();
        public MatrixSolver currentSolver = null;
        public MatrixUI()
        {
            foreach (var m in matrixes) 
            {
                Console.WriteLine($"Template Init, Matrix scale {m.Count}");
                solvers.Add(new MatrixSolver(m));
            }
        }
        public void Menu() 
        {
            menuStart:
            try 
            {
                int option = -1;
                bool run = true;
                while (run) 
                {
                    Console.WriteLine("0 - Choose matrix");
                    Console.WriteLine("1 - Init Dijkstra");
                    Console.WriteLine("2 - Init Prima");
                    Console.WriteLine("3 - Init Clique");
                    Console.WriteLine("4 - Init New matrix");
                    Console.WriteLine("5 - Print results");
                    Console.WriteLine("6 - Exit");
                    Console.Write("Input: "); option = int.Parse(Console.ReadLine());
                    switch (option) 
                    {
                        case 0:
                            {
                                for(int i = 0; i < solvers.Count; i++) 
                                {
                                    Console.WriteLine($"Index: {i}");
                                    solvers[i].PrintMatrix();
                                }
                                Console.Write("Input index: ");
                                int chosenIndex = int.Parse(Console.ReadLine());
                                currentSolver = solvers[chosenIndex];
                                break;
                            }
                        case 1:
                            {
                                if (currentSolver == null)
                                {
                                    throw new Exception("Current matrix is null. Choose matrix or create a new one!");
                                }
                                currentSolver.routes = currentSolver.Dijkstra();
                                break;
                            }
                        case 2:
                            {
                                if (currentSolver == null)
                                {
                                    throw new Exception("Current matrix is null. Choose matrix or create a new one!");
                                }
                                currentSolver.ostov = currentSolver.Prima();
                                break;
                            }
                        case 3:
                            {
                                if (currentSolver == null)
                                {
                                    throw new Exception("Current matrix is null. Choose matrix or create a new one!");
                                }
                                currentSolver.clique = currentSolver.Clique();
                                break;
                            }
                        case 4:
                            {
                                MatrixSolver tempSolver = new MatrixSolver();
                                solvers.Add(tempSolver);
                                break;
                            }
                        case 5:
                            {
                                currentSolver.PrintInfo();
                                break;
                            }
                        case 6:
                            {
                                run = false;
                                break;
                            }
                        default: 
                            {
                                throw new Exception("Unknown operation");
                            }
                    }
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                goto menuStart;
            }
        }
    }
}
