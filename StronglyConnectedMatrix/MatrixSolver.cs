using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace StronglyConnectedMatrix
{
    //Class for containing data
    public class Matrix 
    {
        //Variables
        public int[,] matrix;
        private string json_values; //String variable for saving matrix data
        public string Json_values 
        {
            get { return json_values; }
            set { json_values = value; }
        }
        private int _size;
        public int Size 
        {
            get { return _size; }
            set { 
                if(value > 0) 
                    _size = value; 
            }
        }
        //Events
        public delegate void MatrixHandler();
        public event MatrixHandler matrixChanged;
        //Checker
        public void OnMatrixChanged() 
        {
            if(matrixChanged != null) 
            {
                matrixChanged();
            }
        }
        //Handler
        public void HandleMatrixChanged() 
        {
            FillJSValues();
        }
        //Constructors
        public Matrix() { } //Constructor for json format
        public Matrix(int size) 
        {
            Size = size;
            matrix = new int[size, size];
            Json_values = "";
            matrixChanged += HandleMatrixChanged;
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
            Json_values = "";
            matrixChanged += HandleMatrixChanged;
        }
        public Matrix(Matrix sample, int size)
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
            Json_values = "";
            matrixChanged += HandleMatrixChanged;
        }
        //Methods
        public void Print() 
        {
            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    Console.Write($"{matrix[i, j]} ");
                }
                Console.Write("\n");
            }
        }
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
            OnMatrixChanged();
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
            OnMatrixChanged();
        }
        public void FillJSValues() 
        {
            this.Json_values = "";
            for(int i = 0; i < matrix.GetLength(0); i++) 
            {
                for(int j = 0; j< matrix.GetLength(1); j++) 
                {
                    this.Json_values += matrix[i, j];
                }
            }
        }
        public void UseJSValuesToFillMatrix() 
        {
            if(json_values == null) 
            {
                Console.WriteLine("UseJSValuesToFillMatrix: no data stored");
                return;
            }
            matrix = new int[Size, Size];

            int ctr = 0;
            for (int i = 0; i < Size; i++) 
            {
                for (int j = 0; j < Size; j++) 
                {
                    matrix[i, j] = int.Parse(json_values[ctr].ToString());
                    ++ctr;
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
        //Uses raw matrix as sample
        public static Matrix Transposition(Matrix sample) 
        {
            Matrix transpositioned_matrix = new Matrix(sample.Size);
            for (int i = 0; i < sample.Size; i++)
                for (int j = 0; j < sample.Size; j++)
                    transpositioned_matrix.matrix[i,j] = sample.matrix[j,i];
            return transpositioned_matrix;
        }
        //Uses raw matrix as sample
        public static Matrix StrongConnectivity(Matrix sample) 
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
        public static List<List<int>> SCM(Matrix sample) 
        {
            Matrix strong_connectivity_matrix = new Matrix(sample);
            bool[] isCellVisited = new bool[sample.Size];

            List<List<int>> components = new List<List<int>>();

            for(int i = 0; i < sample.Size; ++i) 
            {
                //If vertex is not visited, then looking for strong connectivity component
                if (!isCellVisited[i])
                {
                    List<int> component = new List<int>();

                    for(int j = 0; j < sample.Size; ++j) 
                    {
                        //If cell isn't visited and equal 1
                        if (!isCellVisited[j] && strong_connectivity_matrix.matrix[i, j] == 1) 
                        {
                            component.Add(j);
                            isCellVisited[j] = true;
                        }    
                    }
                    components.Add(component);
                }
            }
            return components;
        }
    }
    //Class for manipulating with file data
    public static class MatrixFiler 
    {
        public static List<Matrix> matrixes;
        //Events
        public delegate void MatrixFilerHandler();
        public static event MatrixFilerHandler gotMatrixes = HandleGotMatrixes;
        //Checker
        public static void OnGotMatrixes() 
        {
            if(gotMatrixes != null)
                gotMatrixes();
        }
        //Handler
        public static void HandleGotMatrixes()
        {
            foreach(Matrix matrix in matrixes) 
            {
                matrix.UseJSValuesToFillMatrix();
            }
        }
        public static void PrintMatrixes() 
        {
            if(matrixes == null) 
            {
                Console.WriteLine("PrintMatrixes: List is null");
                return;
            }
            int ctr = 0;
            foreach(Matrix matrix_sample in matrixes) 
            {
                Console.WriteLine($"Num.{ctr}");
                ++ctr;
                matrix_sample.Print();
            }
        }
        public static void SetMatrixFile() 
        {
            //If list is null, creating new empty list and writing down it into a file
            if(matrixes == null) 
            {
                Console.WriteLine("SetMatrixFile: List is null. Creating new empty list.");
                matrixes = new List<Matrix>();
                using (FileStream fs = new FileStream("User.json", FileMode.Create))
                {
                    JsonSerializer.Serialize<List<Matrix>>(fs, matrixes);
                }
                return;
            }
            //Writing down info about current list
            using(FileStream fs = new FileStream("User.json", FileMode.Create)) 
            {
                JsonSerializer.Serialize<List<Matrix>>(fs, matrixes);
            }
        }
        public static void GetMatrixFile() 
        {
            using (FileStream fs = new FileStream("User.json", FileMode.OpenOrCreate))
            {
                try 
                {
                    //Getting list from json file even if it is empty (empty != null)
                    matrixes = JsonSerializer.Deserialize<List<Matrix>>(fs);
                }
                catch (JsonException e) 
                {
                    //Creating new list if file contains null value
                    Console.WriteLine("GetMatrixFile: List is null. Calling SetMatrixFile.");
                    //Closing current file stream
                    fs.Dispose();
                    //Calling set method to create and write down empty list
                    SetMatrixFile();
                    using(FileStream fs2 = new FileStream("User.json", FileMode.OpenOrCreate))
                        matrixes = JsonSerializer.Deserialize<List<Matrix>>(fs2);
                    //Filling every matrix int[,] with values using string field json_values
                    OnGotMatrixes();
                }
            }
            //Filling every matrix int[,] with values using string field json_values
            OnGotMatrixes();
        }
        public static void AddMatrix(Matrix sample) 
        {
            //Current method also writing down info into a file. No need to use SetMatrixFile after using Add method
            if (matrixes == null)
            {
                Console.WriteLine("AddMatrix: List is null.");
                GetMatrixFile();
            }
            matrixes.Add(sample);
            SetMatrixFile();
        }
    }

    enum InputCommands 
    {
        choose_matrix = 0,
        create_matrix = 1,
        find_SCM = 2,
        exit = 3
    }
    //Class for user interface
    public class MatrixUI 
    {
        public Matrix current_matrix;
        public int count;
        //Constructor (Used to init list of matrixes from the file)
        public MatrixUI() 
        {
            MatrixFiler.GetMatrixFile();
            count = MatrixFiler.matrixes.Count;
            if (count == 0)
            {
                Console.WriteLine("MatrixUI: List is empty. Add matrix first before using other functions.");
            }
            else 
            {
                Console.WriteLine("MatrixUI: List has values. Index[0] matrix set as current.");
                current_matrix = MatrixFiler.matrixes[0];
            }
        }
        public void Menu() 
        {
            int query = -1;
            do
            {
                try
                {
                    bool is_correct = false;
                    do
                    {
                        Console.WriteLine("Choose option:");
                        Console.WriteLine("choose_matrix = 0");
                        Console.WriteLine("create_matrix = 1");
                        Console.WriteLine("find_SCM = 2");
                        Console.WriteLine("exit = 3");
                        is_correct = false;
                        Console.Write($"Menu: Option:");
                        string user_input = Console.ReadLine();
                        if (user_input.Length == 0)
                        {
                            Console.WriteLine("Menu: Value is null!");
                            continue;
                        }

                        is_correct = int.TryParse(user_input, out query);
                        if (!is_correct)
                        {
                            Console.WriteLine("Menu: Value has wrong type of data!");
                            continue;
                        }
                        if (query < 0 && query > 3)
                        {
                            Console.WriteLine("Menu: Wrong option!");
                            continue;
                        }
                    } while ((query < 0 && query > 3) || !is_correct);

                    switch (query) 
                    {
                        case 0: 
                            {
                                Console.WriteLine("Menu:Case 0: Printing list of matrixes");
                                MatrixFiler.PrintMatrixes();
                                if(MatrixFiler.matrixes.Count == 0) 
                                {
                                    Console.WriteLine("Menu:Case 0: No matrixes have found. Create new one using option 1.");
                                    break;
                                }
                                int query_case0 = -1;
                                Console.WriteLine("Choose matrix.");
                                do
                                {
                                    is_correct = false;
                                    Console.Write($"Menu:Case 0: Option:");
                                    string user_input = Console.ReadLine();
                                    if (user_input.Length == 0)
                                    {
                                        Console.WriteLine("Menu:Case 0:Value is null!");
                                        continue;
                                    }

                                    is_correct = int.TryParse(user_input, out query_case0);
                                    if (!is_correct)
                                    {
                                        Console.WriteLine("Menu:Case 0: Value has wrong type of data!");
                                        continue;
                                    }
                                    if (query_case0 < 0 || query_case0 >= MatrixFiler.matrixes.Count)
                                    {
                                        Console.WriteLine($"Menu:Case 0: Value should be > 0 and < {MatrixFiler.matrixes.Count}.");
                                        continue;
                                    }
                                } while (query_case0 < 0 || query_case0 >= MatrixFiler.matrixes.Count || !is_correct);

                                current_matrix = MatrixFiler.matrixes[query_case0];
                                Console.WriteLine("Menu:Case 0:Matrix has been set.");
                                break;
                            }
                        case 1: 
                            {
                                int query_case1 = -1;
                                int mode = -1;
                                int size = -1;
                                Console.WriteLine("User input - 0 | Random input - 1.");
                                do
                                {
                                    is_correct = false;
                                    Console.Write($"Menu:Case 1:Mode: Option:");
                                    string user_input = Console.ReadLine();
                                    if (user_input.Length == 0)
                                    {
                                        Console.WriteLine("Menu:Case 1:Mode: Value is null!");
                                        continue;
                                    }

                                    is_correct = int.TryParse(user_input, out query_case1);
                                    if (!is_correct)
                                    {
                                        Console.WriteLine("Menu:Case 1:Mode: Value has wrong type of data!");
                                        continue;
                                    }
                                    if (query_case1 != 0 && query_case1 != 1)
                                    {
                                        Console.WriteLine("Menu:Case 1:Mode: Value should be 0 or 1");
                                        continue;
                                    }
                                } while ((query_case1 != 0 && query_case1 != 1) || !is_correct);

                                mode = query_case1;

                                query_case1 = -1;
                                Console.WriteLine("Choose size of matrix.");
                                do
                                {
                                    is_correct = false;
                                    Console.Write($"Menu:Case 1:Size: Option:");
                                    string user_input = Console.ReadLine();
                                    if (user_input.Length == 0)
                                    {
                                        Console.WriteLine("Menu:Case 1:Size: Value is null!");
                                        continue;
                                    }

                                    is_correct = int.TryParse(user_input, out query_case1);
                                    if (!is_correct)
                                    {
                                        Console.WriteLine("Menu:Case 1:Size: Value has wrong type of data!");
                                        continue;
                                    }
                                    if (query_case1 < 0)
                                    {
                                        Console.WriteLine("Menu:Case 1:Size: Value should be > 0.");
                                        continue;
                                    }
                                } while (query_case1 < 0 || !is_correct);
                                size = query_case1;

                                Matrix temp_matrix = new Matrix(size);

                                if (mode == 0) 
                                {
                                    temp_matrix.UserInit();
                                }
                                else 
                                {
                                    temp_matrix.RandomInit();
                                }
                                Console.WriteLine($"Menu:Case 1: Adding matrix:Parameters [Size = {size} Mode = {mode}]");
                                MatrixFiler.AddMatrix(temp_matrix);
                                Console.WriteLine("Menu:Case 1: Matrix has been added.");
                                break;
                            }
                        case 2:
                            {
                                if(current_matrix == null) 
                                {
                                    Console.WriteLine("Menu:Case 2: Current matrix has not been set. Choose matrix using option 0. ");
                                    break;
                                }
                                Console.WriteLine("Menu:Case 2: Raw matrix:");
                                current_matrix.Print();
                                Matrix warshall = MatrixSolver.Warshall(current_matrix);
                                Console.WriteLine("Menu:Case 2: Warshall form:");
                                warshall.Print();
                                Matrix transpositioned = MatrixSolver.Transposition(current_matrix);
                                Console.WriteLine("Menu:Case 2: Transpositioned form:");
                                transpositioned.Print();
                                Matrix strong_connectivity_matrix = MatrixSolver.StrongConnectivity(current_matrix);
                                Console.WriteLine("Menu:Case 2: Strong connectivity form:");
                                strong_connectivity_matrix.Print();
                                List<List<int>> scm = MatrixSolver.SCM(strong_connectivity_matrix);
                                Console.WriteLine("Menu:Case 2: SCM:");
                                for(int i = 0; i < scm.Count; i++) 
                                {
                                    //Printing components
                                    Console.Write($"{i + 1}:");
                                    for(int j = 0; j < scm[i].Count; j++) 
                                    {
                                        Console.Write($" {(char)('a' + scm[i][j])}");
                                    }
                                    Console.Write("\n");
                                }
                                Console.WriteLine("Menu:Case 2: All components have been found.");
                                break;
                            }
                        case 3:
                            {
                                Console.WriteLine("Menu:Case 3: Exit in progress...");
                                break;
                            }
                    }
                }
                catch (Exception e) 
                {
                    Console.WriteLine($"Menu: {e}");
                }
            } while (query != 3);
        }
    }
}
