using System;
using System.Collections.Generic;
using System.IO;

namespace SISE_ZAD1
{

    internal class FifteenPuzzle
    {
        private int iNumberOfMoves;
        private string iMovements;
        private int iNumberOfStates;
        private int iNumberOfProcessedStates;
        private int iNumberOfMaxRecursionDepth;
        private double iComputingTime;


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
            string[] files = Directory.GetFiles("input");

            foreach(string name in files)
            {
                BFSSolver bFSSolver = new BFSSolver(new State(LoadBoard(name), ""), "LRUD");
                bFSSolver.Solve();

                string resultPath = name.Insert(name.Length - 16, "..\\solutions\\");
                using (StreamWriter stream = new StreamWriter(resultPath))
                {
                    stream.WriteLine(bFSSolver.iSolution.iDecisions.Length);
                    stream.WriteLine(bFSSolver.iSolution.iDecisions);
                }

                string additionalDataPath = name.Insert(name.Length - 16, "..\\additionalData\\");
                using (StreamWriter stream = new StreamWriter(additionalDataPath))
                {
                    stream.WriteLine(bFSSolver.iSolutionLength);
                    stream.WriteLine(bFSSolver.iVisitedStates);
                    stream.WriteLine(bFSSolver.iProcessedStates);
                    stream.WriteLine(bFSSolver.iRecursionDepth);
                    stream.WriteLine(bFSSolver.iComputingTime);
                }
            }

            

        }
    }
}
