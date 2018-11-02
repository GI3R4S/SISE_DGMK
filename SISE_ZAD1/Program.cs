using System;
using System.IO;

namespace SISE_ZAD1
{
    internal class FifteenPuzzle
    {
        private int[,] iNumbers;
        private int iNumberOfMoves;
        private string iMovements;
        private int iNumberOfStates;
        private int iNumberOfProcessedStates;
        private int iNumberOfMaxRecursionDepth;
        private double iComputingTime;

        private void LoadBoard(string aPath)
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

            iNumbers = numbers;
        }

        private void SaveResults()
        {
            using (StreamWriter sw = new StreamWriter("result"))
            {
                sw.WriteLine(iNumberOfMoves.ToString());
                sw.Write(iMovements);
            }
        }

        private void SaveAdditionalData()
        {
            using (StreamWriter sw = new StreamWriter("additionalData"))
            {
                sw.WriteLine(iNumberOfMoves.ToString());
                sw.WriteLine(iNumberOfStates.ToString());
                sw.WriteLine(iNumberOfProcessedStates.ToString());
                sw.WriteLine(iNumberOfMaxRecursionDepth.ToString());
                sw.WriteLine(iComputingTime.ToString().Substring(0, 5));
            }
        }

        private bool CheckSolvability()
        {
            int[] oneDimensionalArray = new int[iNumbers.GetLength(0) * iNumbers.GetLength(1)];
            int zerothDimensionCount = iNumbers.GetLength(0);
            int firstDimensionCount = iNumbers.GetLength(1);

            for (int i = 0; i < zerothDimensionCount; i++)
            {
                for (int j = 0; j < firstDimensionCount; j++)
                {
                    oneDimensionalArray[i * firstDimensionCount + j] = iNumbers[i, j];
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
                    if (iNumbers[i, j] == 0)
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
                if((row % 2 == 0 && numberOfInversions % 2 == 1) || (row % 2 == 1 && numberOfInversions % 2 == 0))
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        private static void Main(string[] args)
        {
            FifteenPuzzle fifteenPuzzle = new FifteenPuzzle();
            fifteenPuzzle.LoadBoard("data");
            fifteenPuzzle.CheckSolvability();
        }
    }
}
