using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class MatrixSolver
    {
        public List<List<int>> matrix;
        List<List<int>> complementGraph = new List<List<int>>();
        string[] routes;
        List<int> ostov;
        List<int> clique;

        public MatrixSolver()
        {
            List<List<int>> m = new List<List<int>>() 
            {
                new List<int> {0, 3, 5, 0, 0},
                new List<int> {3, 0, 2, 0, 8},
                new List<int> {5, 2, 0, 3, 0},
                new List<int> {0, 0, 3, 0, 3},
                new List<int> {0, 8, 0, 3, 0},
            };
            matrix = InitMatrix(m);
            PrintMatrix();
            routes = Dijkstra();
            ostov = Prima();
            clique = Clique();
            Console.ReadLine();
        }

        public void PrintMatrix() 
        {
            foreach (List<int> row in matrix) 
            {
                foreach (int col in row) 
                {
                    Console.Write(col.ToString().PadLeft(5));
                }
                Console.WriteLine();
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

            Console.WriteLine("Input start vertex: ");
            int startVertex = Console.ReadLine().ToLower()[0] - 'a';
            Console.WriteLine("Input end vertex: ");
            int endVertex = Console.ReadLine().ToLower()[0] - 'a';

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

            Console.WriteLine("Input start vertex: ");
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
                    canAdd = false;
                    break;
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
}
