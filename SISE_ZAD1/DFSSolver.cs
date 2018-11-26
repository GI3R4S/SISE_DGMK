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
            iOrder = iOrder.ToUpper();
        }

        public override void Solve()
        {
            State initialState = iState;
            Stack<State> opened = new Stack<State>();
            HashSet<State> closed = new HashSet<State>();

            opened.Push(initialState);
            bool isDone = false;

            Stopwatch timer = new Stopwatch();
            timer.Start();

            do
            {
                State currentState = opened.Pop();
                closed.Add(currentState);

                if (currentState.GetNumberOfIncorrect() == 0)
                {
                    isDone = true;
                    iDecisions = currentState.iDecisions.Substring(1, currentState.iDecisions.Length - 1);
                    break;
                }
                else
                {
                    if (currentState.iCurrentDepth == 64)
                    {
                        continue;
                    }
                    else
                    {
                        for (int i = 3; i >= 0; i--)
                        {
                            State optionalState = currentState.GetOptionalState(iOrder[i]);
                            if (optionalState != null && !closed.Contains(optionalState))
                            {
                                opened.Push(optionalState);
                            }
                        }
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
