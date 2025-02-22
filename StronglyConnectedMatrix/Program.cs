using StronglyConnectedMatrix;

Matrix matrix = new Matrix(7);
matrix.UserInit();
Matrix strong = MatrixSolver.Strong_Connectivity(matrix);
List<List<int>> scm = MatrixSolver.SCM(strong);