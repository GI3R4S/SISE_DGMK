using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISE_ZAD1
{
    public class State
    {
        public string iDecisions;
        private int[,] iBoard;

        public State(int[,] aBoard, string aPreviousDecisions)
        {
            iBoard = aBoard;
            iDecisions = aPreviousDecisions;
        }

        public Tuple<int, int> FindEmptyCell()
        {
            Tuple<int, int> result = new Tuple<int, int>(0, 0);

            for (int i = 0; i < iBoard.GetLength(0); i++)
            {
                for (int j = 0; j < iBoard.GetLength(1); j++)
                {
                    if (iBoard[i, j] == 0)
                    {
                        result = new Tuple<int, int>(i, j);
                    }
                }
            }

            return result;
        }

        public State GetOptionalState(char direction)
        {
            Tuple<int, int> zeroCoordinates = FindEmptyCell();
            int[,] toReturn = (int[,])iBoard.Clone();
            string decision = (string)iDecisions.Clone();

            switch (direction)
            {
                case 'L':
                    {
                        if (zeroCoordinates.Item2 != 0)
                        {
                            toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2] = toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2 - 1];
                            toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2 - 1] = 0;
                            decision += "L";
                            break;
                        }
                        toReturn = null;
                        break;
                    }
                case 'R':
                    {
                        if (zeroCoordinates.Item2 != iBoard.GetLength(1) - 1)
                        {
                            toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2] = toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2 + 1];
                            toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2 + 1] = 0;
                            decision += "R";
                            break;
                        }
                        toReturn = null;
                        break;
                    }
                case 'U':
                    {
                        if (zeroCoordinates.Item1 != 0)
                        {
                            toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2] = toReturn[zeroCoordinates.Item1 - 1, zeroCoordinates.Item2];
                            toReturn[zeroCoordinates.Item1 - 1, zeroCoordinates.Item2] = 0;
                            decision += "U";
                            break;
                        }
                        toReturn = null;
                        break;
                    }
                case 'D':
                    {
                        if (zeroCoordinates.Item1 != iBoard.GetLength(0) - 1)
                        {
                            toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2] = toReturn[zeroCoordinates.Item1 + 1, zeroCoordinates.Item2];
                            toReturn[zeroCoordinates.Item1 + 1, zeroCoordinates.Item2] = 0;
                            decision += "D";
                            break;
                        }
                        toReturn = null;
                        break;
                    }
                default:
                    {
                        toReturn = null;
                        break;
                    }
            }

            if (toReturn == null)
            {
                return null;
            }

            return new State(toReturn, decision);
        }

        public int GetNumberOfIncorrect()
        {
            int zerothDimensionLength = iBoard.GetLength(0);
            int firstDimensionLength = iBoard.GetLength(1);
            int[] oneDimensionalArray = new int[zerothDimensionLength * firstDimensionLength];
            int numberOfIncorrectlyPlaced = 0;

            for (int j = 0; j < zerothDimensionLength; j++)
            {
                for (int k = 0; k < firstDimensionLength; k++)
                {
                    oneDimensionalArray[j * firstDimensionLength + k] = iBoard[j, k];
                }
            }

            for (int j = 0; j < zerothDimensionLength * firstDimensionLength; j++)
            {
                if (oneDimensionalArray[j] != j + 1)
                {
                    numberOfIncorrectlyPlaced++;
                }
            }

            if (oneDimensionalArray[oneDimensionalArray.Length - 1] == 0)
            {
                numberOfIncorrectlyPlaced--;
            }

            return numberOfIncorrectlyPlaced;
        }


        public override bool Equals(object obj)
        {
            int[,] toCompare = ((State)obj).iBoard;
            bool isSame = true;

            if (toCompare == null)
            {
                return false;
            }

            for (int j = 0; j < toCompare.GetLength(0); j++)
            {
                for (int k = 0; k < toCompare.GetLength(1); k++)
                {
                    if (toCompare[j, k] != iBoard[j, k])
                    {
                        isSame = false;
                    }
                }
            }

            return isSame;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    };
}
