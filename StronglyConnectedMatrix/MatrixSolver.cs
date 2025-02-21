using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StronglyConnectedMatrix
{
    //Class for containing data
    public class Matrix 
    {
        public int[,] matrix;
        private int _size;
        public int Size 
        {
            get { return _size; }
            set { 
                if(value > 0) 
                    _size = value; 
            }
        }
        //Constructors
        public Matrix(int size) 
        {
            Size = size;
            matrix = new int[Size, Size];
        }
        public Matrix(Matrix sample) 
        {
            this.Size = sample.Size;
            matrix = new int[Size, Size];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = sample.matrix[i, j];
                }
            }
        }
        //Methods
        public void RandomInit() 
        {
            Random connection = new Random();
            for(int i = 0; i < matrix.GetLength(0); i++) 
            {
                for(int j = 0; j < matrix.GetLength(1); j++) 
                {
                    matrix[i, j] = connection.Next(0, 2);
                }
            }
        }
        public void UserInit() 
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    int val = -1;
                    bool is_correct = false;
                    do
                    {
                        is_correct = false ;
                        Console.Write($"[{i}][{j}]: ");
                        string user_input = Console.ReadLine();
                        if(user_input.Length == 0) 
                        {
                            Console.WriteLine("Value is null!");
                            continue;
                        }
                            
                        is_correct = int.TryParse(user_input, out val);
                        if (!is_correct)
                        {
                            Console.WriteLine("Value has wrong type of data!");
                            continue;
                        }
                        if (val != 0 && val != 1)
                        {
                            Console.WriteLine("Value can be only 0 or 1!");
                            continue;
                        }
                    } while ((val != 0 && val != 1) || !is_correct);
                    matrix[i, j] = val;
                }
            }
        }
    }
    //Class for process Matrix data
    public static class MatrixSolver
    {
        //Find reachability matrix
        //Uses raw matrix as sample
        public static Matrix Warshall(Matrix sample) 
        {
            Matrix warshall_matrix = new Matrix(sample);
            //Filling diagonal, point can be reached from itself
            for(int i = 0; i < warshall_matrix.matrix.GetLength(0); i++) 
            {
                warshall_matrix.matrix[i, i] = 1;
            }
            //Transitivity
            for (int k = 0; k < warshall_matrix.Size; k++)
                for (int i = 0; i < warshall_matrix.Size; i++)
                    for (int j = 0; j < warshall_matrix.Size; j++) 
                    {
                        warshall_matrix.matrix[i, j] =
                            warshall_matrix.matrix[i, j] | (warshall_matrix.matrix[i, k] & warshall_matrix.matrix[k, j]);
                    }
            return warshall_matrix;
        }
        //Uses warshall matrix as sample
        public static Matrix Transposition(Matrix sample) 
        {
            Matrix transpositioned_matrix = new Matrix(sample.Size);
            for (int i = 0; i < sample.Size; i++)
                for (int j = 0; j < sample.Size; j++)
                    transpositioned_matrix.matrix[i,j] = sample.matrix[j,i];
            return transpositioned_matrix;
        }
        //Uses raw matrix as sample
        public static Matrix Strong_Connectivity(Matrix sample) 
        {
            Matrix raw = new Matrix(sample);
            Matrix warshall = Warshall(raw);
            Matrix transpositioned_warshall = Transposition(warshall);
            Matrix strong_connectivity_matrix = new Matrix(sample.Size);
            for(int i = 0; i < sample.Size; i++)
                for (int j = 0; j < sample.Size; j++)
                    strong_connectivity_matrix.matrix[i, j] = warshall.matrix[i,j] & transpositioned_warshall.matrix[i, j];
            return strong_connectivity_matrix;
        }

        //Uses Strong Connectivity matrix as sample
        public static List<List<int>> SCC(Matrix sample) 
        {
            Matrix strong_connectivity_matrix = new Matrix(sample);
            bool[] isCellVisited = new bool[sample.Size];

            List<List<int>> components = new List<List<int>>();

            for (int i = 0; i < sample.Size; i++)
            {
                //If vertex is not visited, then looking for strong connectivity component
                if (!isCellVisited[i])
                {
                    List<int> component = new List<int>();

                    //If vertex can be reached, adding it to component
                    DFS(strong_connectivity_matrix, i, isCellVisited, component);

                    //Adding result component to component list
                    components.Add(component);
                }
            }

            return components;
        }
        private static void DFS(Matrix graph, int vertex, bool[] visited, List<int> component)
        {
            visited[vertex] = true; //Marking vertex as visited
            component.Add(vertex);  //Adding vertex to component

            //Iterating through neighbours of vertex
            for (int i = 0; i < graph.Size; i++)
            {
                if (graph.matrix[vertex, i] == 1 && !visited[i]) //If connection exists and it's not marked as visited
                {
                    DFS(graph, i, visited, component); //Continue recursion
                }
            }
        }
    }
}
