using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DecisionRegions
{
    /// <summary>
    /// Clasa care reprezinta un individ din populatie
    /// </summary>
    public class Chromosome
    {
        public int NoGenes { get; set; } // numarul de gene ale individului

        public double[] Genes { get; set; } // valorile genelor

        public double[] MinValues { get; set; } // valorile minime posibile ale genelor

        public double[] MaxValues { get; set; } // valorile maxime posibile ale genelor

        public double Fitness { get; set; } // valoarea functiei de adaptare a individului

        private static Random _rand = new Random();

        public Chromosome(int noGenes, double[] minValues, double[] maxValues)
        {
            Console.WriteLine("maxValues: " + maxValues.Length);
            Console.WriteLine("minValues: " + minValues.Length);
            Console.WriteLine("noGenes: " + noGenes);

            if (minValues.Length != noGenes || maxValues.Length != noGenes)
            {
                throw new ArgumentException("minValues and maxValues must have the same length as noGenes. minValues");
            }

            NoGenes = noGenes;
            Genes = new double[noGenes];
            MinValues = new double[noGenes];
            MaxValues = new double[noGenes];

            for (int i = 0; i < noGenes; i++)
            {
                MinValues[i] = minValues[i];
                MaxValues[i] = maxValues[i];

                
                Genes[i] = minValues[i] + _rand.NextDouble() * (maxValues[i] - minValues[i]); // initializare aleatorie a genelor
            }
        }

        public Chromosome(Chromosome c) // constructor de copiere
        {
            NoGenes = c.NoGenes;
            Fitness = c.Fitness;

            Genes = new double[c.NoGenes];
            MinValues = new double[c.NoGenes];
            MaxValues = new double[c.NoGenes];

            for (int i = 0; i < c.Genes.Length; i++)
            {
                Genes[i] = c.Genes[i];
                MinValues[i] = c.MinValues[i];
                MaxValues[i] = c.MaxValues[i];
            }
        }
    }
}
