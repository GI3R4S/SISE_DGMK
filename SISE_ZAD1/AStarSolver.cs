using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SISE_ZAD1
{
    internal class AStarSolver
    {
        public State iState;
        public State iSolution;
        public string iOrder;
        #region AdditionalData

        public int iSolutionLength = 0;
        public int iProcessedStates = 0;
        public int iVisitedStates = 0;
        public int iRecursionDepth = 0;
        public double iComputingTime = 0;
        #endregion

        public AStarSolver(State aState, string aOrder)
        {
            iState = aState;
            iOrder = aOrder;
        }

        public void Solve()
        {
            State initialState = iState;
            List<State> opened = new List<State>();
            HashSet<State> closed = new HashSet<State>();
            string order = "LRUD";
            var start = Process.GetCurrentProcess().TotalProcessorTime;

            opened.Add(initialState);
            bool isDone = false;
            ByManhattanDistance byHammDistance = new ByManhattanDistance();

            do
            {
                if ((Process.GetCurrentProcess().TotalProcessorTime - start).Seconds >= 10)
                    break;

                opened.Sort(byHammDistance);
                State currentMax = opened[0];
                
                if (currentMax.GetNumberOfIncorrect() == 0)
                {
                    isDone = true;
                    iSolution = currentMax;
                    break;
                }
                else
                {

                    for (int i = 0; i < 4; i++)
                    {
                        State optionalState = currentMax.GetOptionalState(order[i]);
                        if (optionalState != null && !closed.Contains(optionalState))
                        {
                            opened.Add(optionalState);
                        }
                    }

                    opened.Remove(currentMax);
                    closed.Add(currentMax);

                }

            } while (isDone == false);

            if (!isDone)
            {
                iSolution = new State(new int[,] { {0, 0, 0, 0}, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, }, "", 0);
                return;
            }
            int maxRecursionClosed = closed.Max(p => p.iCurrentDepth);
            int maxRecursionOpened = opened.Max(p => p.iCurrentDepth);

            iRecursionDepth = maxRecursionClosed > maxRecursionOpened ? maxRecursionClosed : maxRecursionOpened;
            iProcessedStates = closed.Count;
            iVisitedStates = closed.Count + opened.Count;
            var end = Process.GetCurrentProcess().TotalProcessorTime;
            iComputingTime = Math.Round((end - start).TotalMilliseconds, 3);
        }
    }
}
