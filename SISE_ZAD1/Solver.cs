using System.IO;

namespace SISE_ZAD1
{
    internal abstract class Solver
    {
        public State iState;
        public State iSolution;
        public int iSolutionLength = 0;
        public int iProcessedStates = 0;
        public int iVisitedStates = 0;
        public int iRecursionDepth = 0;
        public double iComputingTime = 0;

        public abstract void Solve();

        public void PrintData(string aSolutionPath, string aDataPath)
        {
            using (StreamWriter stream = new StreamWriter(aSolutionPath))
            {
                stream.WriteLine(iSolution.iDecisions.Length);
                stream.WriteLine(iSolution.iDecisions.Substring(1, iSolution.iDecisions.Length - 1));
            }
            using (StreamWriter stream = new StreamWriter(aDataPath))
            {
                stream.WriteLine(iSolutionLength);
                stream.WriteLine(iVisitedStates);
                stream.WriteLine(iProcessedStates);
                stream.WriteLine(iRecursionDepth);
                stream.WriteLine(iComputingTime);
            }
        }
    }
}
