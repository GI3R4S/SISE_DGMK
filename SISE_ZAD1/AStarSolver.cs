using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SISE_ZAD1
{
    internal class AStarSolver : Solver
    {
        public string iHeurestic;

        public AStarSolver(State aState, string aHeurestic)
        {
            iState = aState;
            iHeurestic = aHeurestic;
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

            do
            {
                State bestMatch = null;

                if (iHeurestic == "hamm")
                {
                    bestMatch = opened.OrderBy(p => p.HammingHeurestic).First();
                }

                else if (iHeurestic == "manh")
                {
                    bestMatch = opened.OrderBy(p => p.ManhattanLinearConflictHeurestic).First();
                }

                else if (iHeurestic == "manhlc")
                {
                    bestMatch = opened.OrderBy(p => p.ManhattanLinearConflictHeurestic).First();
                }

                if (bestMatch.GetNumberOfIncorrect() == 0)
                {
                    iSolution = bestMatch;
                    iSolutionLength = iSolution.iDecisions.Length - 1;
                    isDone = true;
                    break;
                }
                else
                {
                    string order = "LRUD";
                    for (int i = 0; i < 4; i++)
                    {
                        State optionalState = bestMatch.GetOptionalState(order[i]);
                        if (optionalState != null && !closed.Contains(optionalState))
                        {
                            opened.Add(optionalState);
                        }
                    }
                    closed.Add(bestMatch);
                    opened.Remove(bestMatch);
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
