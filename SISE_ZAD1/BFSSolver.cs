using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISE_ZAD1
{
    class BFSSolver
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

        public BFSSolver(State aState, string aOrder)
        {
            iState = aState;
            iOrder = aOrder;
        }

        public void Solve()
        {
            State initialState = iState;
            HashSet<State> opened = new HashSet<State>();
            HashSet<State> closed = new HashSet<State>();

            var start = Process.GetCurrentProcess().TotalProcessorTime;

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
            var end = Process.GetCurrentProcess().TotalProcessorTime;
            iComputingTime = Math.Round((end - start).TotalMilliseconds, 3);
        }
    }
}
