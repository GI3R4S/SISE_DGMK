using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SISE_ZAD1
{
    internal class AStarSolver : Solver
    {
        private delegate State SortOpened(ref HashSet<State> aOpened);

        private SortOpened sortingMethod;
        public string iHeurestic;

        public static State SortByHamming(ref HashSet<State> opened)
        {
            return opened.OrderBy(p => p.HammingHeurestic).First();
        }

        public static State SortByManhattan(ref HashSet<State> opened)
        {
            return opened.OrderBy(p => p.ManhattanHeurestic).First();
        }
        public static State SortByMixed(ref HashSet<State> opened)
        {
            return opened.OrderBy(p => p.MixedHeurestic).First();
        }

        public AStarSolver(State aState, string aHeurestic)
        {
            iState = aState;
            iHeurestic = aHeurestic.ToLower();
        }

        public override void Solve()
        {
            State initialState = iState;
            HashSet<State> opened = new HashSet<State>();
            HashSet<State> closed = new HashSet<State>();




            if (iHeurestic == "hamm")
            {
                sortingMethod = SortByHamming;
            }

            else if (iHeurestic == "manh")
            {
                sortingMethod = SortByManhattan;
            }

            else if (iHeurestic == "manhlc")
            {
                sortingMethod = SortByMixed;
            }


            opened.Add(initialState);
            bool isDone = false;
            Stopwatch timer = new Stopwatch();


            timer.Start();
            do
            {
                State currentState = sortingMethod(ref opened);


                if (currentState.GetNumberOfIncorrect() == 0)
                {
                    iDecisions = currentState.iDecisions.Substring(1, currentState.iDecisions.Length - 1);
                    isDone = true;
                    timer.Stop();
                    break;
                }
                else
                {
                    string order = "LRUD";
                    for (int i = 0; i < 4; i++)
                    {
                        State optionalState = currentState.GetOptionalState(order[i]);
                        if (optionalState != null && !closed.Contains(optionalState))
                        {
                            opened.Add(optionalState);
                        }
                    }
                    closed.Add(currentState);
                    opened.Remove(currentState);
                }

            } while (isDone == false);

            iRecursionDepth = opened.Max(p => p.iCurrentDepth) > closed.Max(p => p.iCurrentDepth) ? opened.Max(p => p.iCurrentDepth) : closed.Max(p => p.iCurrentDepth);

            iProcessedStates = closed.Count;
            iVisitedStates = closed.Count + opened.Count;


            iComputingTime = Math.Round((1000.0 * timer.ElapsedTicks / Stopwatch.Frequency), 3);
        }
    }
}
