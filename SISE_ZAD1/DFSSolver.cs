using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SISE_ZAD1
{
    internal class DFSSolver : Solver
    {
        public string iOrder;

        public DFSSolver(State aState, string aOrder)
        {
            iState = aState;
            iOrder = aOrder;
        }

        public override void Solve()
        {
            State initialState = iState;
            ByCurrentDepth comparer = new ByCurrentDepth(iOrder);
            SortedSet<State> opened = new SortedSet<State>(comparer);
            HashSet<State> closed = new HashSet<State>();

            opened.Add(initialState);
            bool isDone = false;

            Stopwatch timer = new Stopwatch();
            timer.Start();

            do
            {
                State currentMax = opened.Max;

                if (currentMax.GetNumberOfIncorrect() == 0)
                {
                    isDone = true;
                    iSolution = currentMax;
                    iSolutionLength = iSolution.iDecisions.Length -1;
                    break;
                }
                else
                {
                    //if (currentMax.iCurrentDepth == 64)
                    //{
                    //    closed.Add(currentMax);
                    //    opened.Remove(currentMax);
                    //}
                    //else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            State optionalState = currentMax.GetOptionalState(iOrder[i]);
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

            iRecursionDepth = opened.Max(p => p.iCurrentDepth) > closed.Max(p => p.iCurrentDepth) ? opened.Max(p => p.iCurrentDepth) : closed.Max(p => p.iCurrentDepth);

            iProcessedStates = closed.Count;
            iVisitedStates = closed.Count + opened.Count;
            timer.Stop();

            iComputingTime = Math.Round((1000.0 * timer.ElapsedTicks / Stopwatch.Frequency), 3);
        }
    }
}
