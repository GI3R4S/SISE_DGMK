using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            iOrder = iOrder.ToUpper();
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
            int iteration = 1;

            do
            {
                List<State> currentLevel = new List<State>();
                currentLevel = opened.Where(p => p.iCurrentDepth == iteration).ToList();

                for (int h = 0; h < 4; h++)
                {
                    List<State> currentState = currentLevel.Where(p => p.GetLastLetter == iOrder[h]).ToList();
                    for (int i = 0; i < currentState.Count; i++)
                    {
                        if (currentState[i].GetNumberOfIncorrect() == 0)
                        {
                            iDecisions = currentState[i].iDecisions.Substring(1, currentState[i].iDecisions.Length - 1);
                            isDone = true;
                            break;
                        }
                        else
                        {
                            closed.Add(currentState[i]);
                            opened.Remove(currentState[i]);
                        }
                        for (int j = 0; j < 4; j++)
                        {
                            State temporaryState = currentState[i].GetOptionalState(iOrder[j]);
                            if (temporaryState != null && !closed.Contains(temporaryState))
                            {
                                opened.Add(temporaryState);
                            }
                        }
                    }
                }
                iteration++;
            } while (!isDone);


            iRecursionDepth = opened.Max(p => p.iCurrentDepth) > closed.Max(p => p.iCurrentDepth) ? opened.Max(p => p.iCurrentDepth) : closed.Max(p => p.iCurrentDepth);

            iProcessedStates = closed.Count;
            iVisitedStates = closed.Count + opened.Count;
            timer.Stop();
            
            iComputingTime = Math.Round((1000.0 * timer.ElapsedTicks / Stopwatch.Frequency), 3);
        }
    }
}
