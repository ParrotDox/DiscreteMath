using StronglyConnectedMatrix;

Matrix matrix_5_5 = new Matrix(7);
matrix_5_5.UserInit();
Matrix strong = MatrixSolver.Strong_Connectivity(matrix_5_5);
List<List<int>> scc = MatrixSolver.SCC(strong);