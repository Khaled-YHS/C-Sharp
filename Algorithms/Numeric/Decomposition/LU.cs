using System;

namespace Algorithms.Numeric.Decomposition
{
    /// <summary>
    /// LU-decomposition factors the "source" matrix as the product of lower triangular matrix
    /// and upper triangular matrix.
    /// </summary>
    public static class LU
    {
        /// <summary>
        /// Performs LU-decomposition on "source" matrix.
        /// Lower and upper matrices have same shapes as source matrix.
        /// Note: Decomposition can be applied only to square matrices.
        /// </summary>
        /// <param name="source">Square matrix to decompose.</param>
        /// <returns>Tuple of lower and upper matrix.</returns>
        /// <exception cref="ArgumentException">Source matrix is not square shaped.</exception>
        public static (double[,] L, double[,] U) Decompose(double[,] source)
        {
            if (source.GetLength(0) != source.GetLength(1))
            {
                throw new ArgumentException("Source matrix is not square shaped.");
            }

            var pivot = source.GetLength(0);
            var lower = new double[pivot, source.GetLength(1)];
            var upper = new double[pivot, source.GetLength(1)];

            for (var i = 0; i < pivot; i++)
            {
                for (var k = i; k < pivot; k++)
                {
                    double sum = 0;

                    for (var j = 0; j < i; j++)
                    {
                        sum += lower[i, j] * upper[j, k];
                    }

                    upper[i, k] = source[i, k] - sum;
                }

                for (var k = i; k < pivot; k++)
                {
                    if (i == k)
                    {
                        lower[i, i] = 1;
                    }
                    else
                    {
                        double sum = 0;

                        for (var j = 0; j < i; j++)
                        {
                            sum += lower[k, j] * upper[j, i];
                        }

                        lower[k, i] = (source[k, i] - sum) / upper[i, i];
                    }
                }
            }

            return (L: lower, U: upper);
        }

        /// <summary>
        /// Eliminates linear equations system represented as A*x=b, using LU-decomposition,
        /// where A - matrix of equation coefficients, b - vector of absolute terms of equations.
        /// </summary>
        /// <param name="matrix">Matrix of equation coefficients.</param>
        /// <param name="coefficients">Vector of absolute terms of equations.</param>
        /// <returns>Vector-solution for linear equations system.</returns>
        /// <exception cref="ArgumentException">Matrix of equation coefficients is not square shaped.</exception>
        public static double[,] Eliminate(double[,] matrix, double[,] coefficients)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new ArgumentException("Matrix of equation coefficients is not square shaped.");
            }

            var pivot = matrix.GetLength(0);
            var upperTransform = new double[pivot, 1]; // U * upperTransform = coefficients
            var solution = new double[pivot, 1]; // L * solution = upperTransform
            (double[,] l, double[,] u) = LU.Decompose(matrix);

            for (var i = 0; i < pivot; i++)
            {
                double pivotPointSum = 0;

                for (var j = 0; j < i; j++)
                {
                    pivotPointSum += upperTransform[j, 0] * l[i, j];
                }

                upperTransform[i, 0] = (coefficients[i, 0] - pivotPointSum) / l[i, i];
            }

            for (var i = pivot - 1; i >= 0; i--)
            {
                double constantSum = 0;

                for (var j = i; j < pivot; j++)
                {
                    constantSum += solution[j, 0] * u[i, j];
                }

                solution[i, 0] = (upperTransform[i, 0] - constantSum) / u[i, i];
            }

            return solution;
        }
    }
}
