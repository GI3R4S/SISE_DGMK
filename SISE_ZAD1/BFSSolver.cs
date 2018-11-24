using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SISE_ZAD1
{
    class BFSSolver : Solver
    {

        public string iOrder;

        #region AdditionalData

        #endregion

        public BFSSolver(State aState, string aOrder)
        {
            base.iState = aState;
            iOrder = aOrder;
        }

        public override void Solve()
        {
            State initialState = iState;
            HashSet<State> opened = new HashSet<State>();
            HashSet<State> closed = new HashSet<State>();

            Stopwatch timer = new Stopwatch();
            timer.Start();

            opened.Add(initialState);
            bool isDone = false;
            int recursion = 1;
            do
            {
                List<State> newlyCreated = new List<State>();
                List<State> toRemove = new List<State>();

                foreach (State state in opened)
                {
                    if (state.GetNumberOfIncorrect() == 0)
                    {
                        isDone = true;
                        iSolution = state;
                        iSolutionLength = state.iDecisions.Length;

                        break;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        State optionalState = state.GetOptionalState(iOrder[i]);
                        if (!closed.Contains(optionalState) && optionalState != null)
                        {
                            newlyCreated.Add(optionalState);
                        }
                    }
                    closed.Add(state);
                    toRemove.Add(state);
                }

                recursion++;

                foreach (State stat in newlyCreated)
                {
                    opened.Add(stat);
                }

                foreach (State stat in toRemove)
                {
                    opened.Remove(stat);
                }

                newlyCreated.Clear();
                toRemove.Clear();

            } while (isDone == false);

            iRecursionDepth = recursion;
            iProcessedStates = closed.Count;
            iVisitedStates = closed.Count + opened.Count;
            timer.Stop();
            
            iComputingTime = Math.Round((1000.0 * timer.ElapsedTicks / Stopwatch.Frequency), 3);
        }
    }
}
