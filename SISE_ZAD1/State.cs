using System;
using System.Collections.Generic;

namespace SISE_ZAD1
{
    public class State
    {
        private int[,] iBoard;
        int zerothDimensionLength;
        int firstDimensionLength;

        public int iCurrentDepth = 1;
        public string iDecisions;
        public char GetLastLetter
        {
            get
            {
                return iDecisions[iDecisions.Length - 1];
            }
        }

        public int HammingHeurestic
        {
            get
            {
                int[] oneDimensionalArray = new int[zerothDimensionLength * firstDimensionLength];
                int numberOfIncorrectlyPlaced = 0;

                for (int j = 0; j < zerothDimensionLength; j++)
                {
                    for (int k = 0; k < firstDimensionLength; k++)
                    {
                        oneDimensionalArray[j * firstDimensionLength + k] = iBoard[j, k];
                    }
                }

                for (int j = 0; j < zerothDimensionLength * firstDimensionLength - 1; j++)
                {
                    if (oneDimensionalArray[j] != j + 1 && oneDimensionalArray[j] != 0)
                    {
                        numberOfIncorrectlyPlaced++;
                    }
                }

                if(oneDimensionalArray[zerothDimensionLength * firstDimensionLength - 1] != 0)
                {
                    numberOfIncorrectlyPlaced++;
                }

                return numberOfIncorrectlyPlaced + iCurrentDepth;
            }
        }
        public int Dijkstra
        {
            get
            {
                int distanceValue = 0;
                for (int i = 0; i < zerothDimensionLength; i++)
                {
                    for (int j = 0; j < firstDimensionLength; j++)
                    {
                        if (iBoard[i, j] != TargetBoard[i, j] && iBoard[i, j] != 0)
                        {
                            int coordinateX = FifteenPuzzle.TargetState[iBoard[i, j]].Key;
                            int coordinateY = FifteenPuzzle.TargetState[iBoard[i, j]].Value;

                            distanceValue += Math.Abs(i - coordinateX);
                            distanceValue += Math.Abs(j - coordinateY);
                        }
                    }
                }

                return distanceValue;
            }
        }
        public int ManhattanHeurestic
        {
            get
            {
                return Dijkstra + iCurrentDepth;
            }
        }

        public int MixedHeurestic
        {
            get
            {
                return HammingHeurestic > ManhattanHeurestic ? HammingHeurestic : ManhattanHeurestic;
            }
        }

        int[,] TargetBoard;

        public State(int[,] aBoard, string aPreviousDecisions, int aNewDepth)
        {
            iBoard = aBoard;
            zerothDimensionLength = iBoard.GetLength(0);
            firstDimensionLength = iBoard.GetLength(1);

            TargetBoard = new int[zerothDimensionLength, firstDimensionLength];
            for (int i = 0; i < zerothDimensionLength; i++)
            {
                for(int j = 0; j < firstDimensionLength; j++)
                {
                    TargetBoard[i, j] = i * zerothDimensionLength + j + 1;
                }
            }
            TargetBoard[zerothDimensionLength - 1, firstDimensionLength - 1] = 0;

            iDecisions = aPreviousDecisions;
            iCurrentDepth = aNewDepth;

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
            int currentDepth = iCurrentDepth;

            switch (direction)
            {
                case 'L':
                    {
                        if (zeroCoordinates.Item2 != 0)
                        {
                            toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2] = toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2 - 1];
                            toReturn[zeroCoordinates.Item1, zeroCoordinates.Item2 - 1] = 0;
                            decision += "L";
                            currentDepth += 1;
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
                            currentDepth += 1;
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
                            currentDepth += 1;
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
                            currentDepth += 1;
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

            return new State(toReturn, decision, currentDepth);
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
        public void PrintCurrentBoard()
        {
            Console.WriteLine("===================================");
            for (int j = 0; j < iBoard.GetLength(0); j++)
            {
                for (int k = 0; k < iBoard.GetLength(1); k++)
                {
                    Console.Write(iBoard[j, k] + "\t");
                }
                Console.Write("\n");
            }
            Console.WriteLine("===================================");

        }
    };


    public class ByCurrentDepth : IComparer<State>
    {
        string iOrder;
        
        public ByCurrentDepth(string aNewOrder)
        {
            iOrder = aNewOrder;
        }
        public int Compare(State x, State y)
        {
            if (x.iCurrentDepth == y.iCurrentDepth)
            {
                char xCurrentPosition = x.GetLastLetter;
                char yCurrentPosition = y.GetLastLetter;
                int xIndex = 0;
                int yIndex = 0;
                for(int i = 0; i < iOrder.Length; i++)
                {
                    if (xCurrentPosition == iOrder[i])
                        xIndex = i;
                    if (yCurrentPosition == iOrder[i])
                        yIndex = i;
                }
                if (xIndex == yIndex)
                    return 0;
                else if (xIndex < yIndex)
                    return 1;
                else
                    return -1;

            }
            else if (x.iCurrentDepth > y.iCurrentDepth)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
