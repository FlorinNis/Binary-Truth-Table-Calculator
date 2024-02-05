using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRegions
{
    /// <summary>
    /// Interfata pentru problemele de optimizare
    /// </summary>
    public interface IOptimizationProblem
    {
        void ComputeFitness(Chromosome c);

        Chromosome MakeChromosome();
    }
}
