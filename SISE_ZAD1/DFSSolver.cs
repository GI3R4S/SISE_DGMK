using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SISE_ZAD1
{
    internal class DFSSolver
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

        public DFSSolver(State aState, string aOrder)
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


            ByCurrentDepth comparer = new ByCurrentDepth();

            do
            {
                //iteration++;
                //int maxDepth = opened.Max(p => p.iCurrentDepth);
                //List<State> targets = opened.Where(p => p.iCurrentDepth == maxDepth).ToList();
                //State currentMax = null;

                //IEnumerable<State> firstLetter = targets.Where(p => p.GetLastLetter == order[0]);

                //if (firstLetter.Count() != 0)
                //{
                //    currentMax = firstLetter.First();
                //}
                //else if (targets.Where(p => p.GetLastLetter == order[1]).Count() != 0)
                //{
                //    currentMax = targets.Where(p => p.GetLastLetter == order[1]).First();
                //}
                //else if (targets.Where(p => p.GetLastLetter == order[2]).Count() != 0)
                //{
                //    currentMax = targets.Where(p => p.GetLastLetter == order[2]).First();
                //}
                //else
                //{
                //    currentMax = targets.Where(p => p.GetLastLetter == order[3]).First();
                //}
                opened.Sort(comparer);

                State currentMax = opened[0];

                if (currentMax.GetNumberOfIncorrect() == 0)
                {
                    isDone = true;
                    iSolution = currentMax;
                    break;
                }
                else
                {
                    if (currentMax.iCurrentDepth == 64)
                    {
                        opened.Remove(currentMax);
                        closed.Add(currentMax);
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
                }

            } while (isDone == false);

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
