using System.IO;

namespace SISE_ZAD1
{
    internal abstract class Solver
    {
        public State iState;

        public string iDecisions= "";
        public int iProcessedStates = 0;
        public int iVisitedStates = 0;
        public int iRecursionDepth = 0;
        public double iComputingTime = 0;

        public abstract void Solve();

        public void PrintData(string aSolutionPath, string aDataPath)
        {
            using (StreamWriter stream = new StreamWriter(aSolutionPath))
            {
                stream.WriteLine(iDecisions.Length);
                stream.WriteLine(iDecisions);
            }
            using (StreamWriter stream = new StreamWriter(aDataPath))
            {
                stream.WriteLine(iDecisions.Length);
                stream.WriteLine(iVisitedStates);
                stream.WriteLine(iProcessedStates);
                stream.WriteLine(iRecursionDepth);
                stream.WriteLine(iComputingTime);
            }
        }
    }
}
