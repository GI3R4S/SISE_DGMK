using System;
using System.Collections.Generic;
using System.IO;

namespace SISE_ZAD1
{

    internal class FifteenPuzzle
    {
        public static Dictionary<int, KeyValuePair<int, int>> TargetState;
        public static int[,] rows;
        public static int[,] collumns;

        public static int[,] LoadBoard(string aPath)
        {

            string[] allLines = File.ReadAllLines(aPath);
            int numberOfColumns = Convert.ToInt32(allLines[0].Split(' ')[0]);
            int numberOfRows = Convert.ToInt32(allLines[0].Split(' ')[1]);
            int[,] numbers = new int[numberOfColumns, numberOfRows];

            for (int i = 1; i <= numberOfColumns; i++)
            {
                string[] lineItems = allLines[i].Split(' ');
                for (int j = 0; j < lineItems.Length; j++)
                {
                    numbers[i - 1, j] = Convert.ToInt32(lineItems[j]);
                }
            }

            return numbers;
        }



        public static bool IsSolvable(int[,] aNumbers)
        {
            int[] oneDimensionalArray = new int[aNumbers.GetLength(0) * aNumbers.GetLength(1)];
            int zerothDimensionCount = aNumbers.GetLength(0);
            int firstDimensionCount = aNumbers.GetLength(1);

            for (int i = 0; i < zerothDimensionCount; i++)
            {
                for (int j = 0; j < firstDimensionCount; j++)
                {
                    oneDimensionalArray[i * firstDimensionCount + j] = aNumbers[i, j];
                }
            }

            int numberOfInversions = 0;

            for (int i = 0; i < oneDimensionalArray.Length - 1; i++)
            {
                for (int j = i + 1; j < oneDimensionalArray.Length; j++)
                {
                    if (oneDimensionalArray[i] > oneDimensionalArray[j] && oneDimensionalArray[j] != 0)
                    {
                        numberOfInversions++;
                    }
                }
            }
            int row = 0;

            for (int i = 0; i < zerothDimensionCount; i++)
            {
                for (int j = 0; j < firstDimensionCount; j++)
                {
                    if (aNumbers[i, j] == 0)
                    {
                        row = i;
                    }
                }
            }

            row = zerothDimensionCount - row;

            if (zerothDimensionCount % 2 == 1 && numberOfInversions % 2 == 0)
            {
                return true;
            }

            if (zerothDimensionCount % 2 == 0)
            {
                if ((row % 2 == 0 && numberOfInversions % 2 == 1) || (row % 2 == 1 && numberOfInversions % 2 == 0))
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        private static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                return;
            }

            Solver solver = null;


            if (!File.Exists(args[2]))
            {
                return;
            }

            int[,] initialBoard = LoadBoard(args[2]);
            if (initialBoard == null)
                return;

            switch (args[0])
            {
                case "bfs":
                    {
                        solver = new BFSSolver(new State(initialBoard, "L", 1), args[1]);
                        break;
                    }
                case "dfs":
                    {
                        solver = new DFSSolver(new State(initialBoard, "L", 1), args[1]);
                        break;
                    }
                case "astr":
                    {
                        solver = new AStarSolver(new State(initialBoard, "L", 1), args[1]);
                        break;
                    }
                default:
                    {
                        return;
                    }
            }


            TargetState = new Dictionary<int, KeyValuePair<int, int>>();
            for (int i = 0; i < initialBoard.GetLength(0); i++)
            {
                for (int j = 0; j < initialBoard.GetLength(1); j++)
                {
                    int value = i * initialBoard.GetLength(0) + j;
                    if (i * initialBoard.GetLength(0) + j < initialBoard.GetLength(0) * initialBoard.GetLength(1))
                    {
                        TargetState.Add(value + 1, new KeyValuePair<int, int>(i, j));
                    }
                }
            }

            solver.Solve();
            solver.PrintData(args[3], args[4]);
        }
    }
}
