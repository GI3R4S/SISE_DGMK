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
            Queue<State> opened = new Queue<State>();
            HashSet<State> closed = new HashSet<State>();

            
            
            opened.Enqueue(initialState);
            bool isDone = false;
            int iteration = 1;

            Stopwatch timer = new Stopwatch();
            timer.Start();
            do
            {
                State currentState = opened.Dequeue();
                closed.Add(currentState);
                if (currentState.GetNumberOfIncorrect() == 0)
                {
                    iRecursionDepth = currentState.iCurrentDepth;
                    isDone = true;
                    iDecisions = currentState.iDecisions.Substring(1, currentState.iDecisions.Length - 1);
                    timer.Stop();
                    break;
                }
                else
                {
                    for(int i = 0; i < 4; i++)
                    {
                        State optionalState = currentState.GetOptionalState(iOrder[i]);
                        if(optionalState != null && !closed.Contains(optionalState))
                        {
                            opened.Enqueue(optionalState);
                        }
                    }
                }

                iteration++;
            } while (!isDone);

            iProcessedStates = closed.Count;
            iVisitedStates = closed.Count + opened.Count;
            
            
            iComputingTime = Math.Round((1000.0 * timer.ElapsedTicks / Stopwatch.Frequency), 3);
        }
    }
}
