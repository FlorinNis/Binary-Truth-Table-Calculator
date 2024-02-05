using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRegions
{
    /// <summary>
    /// Clasa care reprezinta operatia de selectie
    /// </summary>
    public class Selection
    {
        private static Random _rand = new Random();

        public static Chromosome Tournament(Chromosome[] population)
        {
            Chromosome c = population[_rand.Next(population.Length)];
            Chromosome c1 = population[_rand.Next(population.Length)];
            if (c.Fitness > c1.Fitness)
                return c;
            else
                return c1;
        }

        public static Chromosome GetBest(Chromosome[] population)
        {
            int contorAdaptat = 0;
            double adaptat = population[0].Fitness;
            for (int i = 1; i < population.Length; i++)
            {
                if (population[i].Fitness > adaptat)
                {
                    adaptat = population[i].Fitness;
                    contorAdaptat = i;
                    if (adaptat > 0.90f) break;
                }
            }
            Chromosome celMaiAdaptat;
            celMaiAdaptat = population[contorAdaptat];
            return celMaiAdaptat;
        }
    }
    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta operatia de incrucisare
    /// </summary>
    public class Crossover
    {
        private MainForm mainForm;
        public Crossover(MainForm form)
        {
            mainForm = form;
        }
        private static Random _rand = new Random();

        public static Chromosome Arithmetic(Chromosome mother, Chromosome father, double rate)
        {
            
            double pc = _rand.NextDouble();
            if (rate > pc)
            {
                Random MasculinFeminin = new Random();
                int Fata_Sau_Baiat = MasculinFeminin.Next(1, 3);
                if (Fata_Sau_Baiat == 1)
                {
                    Chromosome copil;
                    copil = new Chromosome(father);
                    return copil;
                }
                else
                {
                    Chromosome copil;
                    copil = new Chromosome(mother);
                    return copil;
                }
            }
            else
            {
                int maxGenes;
                if (mother.NoGenes > father.NoGenes)
                    maxGenes = father.NoGenes;
                else maxGenes = mother.NoGenes;

                Chromosome copil;
                if (maxGenes == 3)
                {
                    copil = new Chromosome(maxGenes, new double[] { -1, -1, -1 }, new double[] { 1, 1, 1 });
                }
                else { 
                    copil = new Chromosome(maxGenes, new double[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
                }

                //for (int i = 0; i < maxGenes; i++)
                //{
                Random number = new Random();
                double a = number.NextDouble();
                while (a == 0)
                    a = number.NextDouble();
                    //copil.Genes[i] = a * mother.Genes[i] + (1 - a) * father.Genes[i];

                //optimizationProblem var = new optimizationProblem();
                MainForm mainForm = new MainForm();
                int[]  output = mainForm.getExpectedOutput();

                //Verificam cazul 0 0 0
                if (output[0] == 0 && mother.Genes[2] <= 0 && father.Genes[2] <= 0)
                    copil.Genes[2] = 1 - (mother.Genes[2] * a + father.Genes[2] * a);
                else if (mother.Genes[2] > 0) copil.Genes[2] = mother.Genes[2];
                else copil.Genes[2] = father.Genes[2];

                //Verificam cazul 0 0 1
                if (output[0] == 1 && mother.Genes[2] > 0 && father.Genes[2] > 0)
                    copil.Genes[2] = 1 + (mother.Genes[2] * a - father.Genes[2] * a);
                else if (mother.Genes[2] <= 0 && output[0] == 1) copil.Genes[2] = mother.Genes[2];
                else if(output[0] == 1) copil.Genes[2] = father.Genes[2];

                //Verificam cazul 0 1 0
                if (output[1] == 0 && mother.Genes[1] > mother.Genes[2] && father.Genes[1] > father.Genes[2])
                    copil.Genes[1] = mother.Genes[2] - a * mother.Genes[1] + father.Genes[1];
                else if (mother.Genes[1] <= mother.Genes[2] && output[1] == 0) copil.Genes[1] = mother.Genes[1];
                else if (output[1] == 0) copil.Genes[1] = father.Genes[1];

                //Verificam cazul 0 1 1
                if (output[1] == 1 && mother.Genes[1] >= mother.Genes[2] && father.Genes[1] >= father.Genes[2])
                    copil.Genes[1] = mother.Genes[2] - a * mother.Genes[1] + father.Genes[1];
                else if (mother.Genes[1] > mother.Genes[2] && output[1] == 1) copil.Genes[1] = mother.Genes[1];
                else if (output[1] == 1) copil.Genes[1] = father.Genes[1];

                //Verificam cazul 1 0 0
                if (output[2] == 0 && mother.Genes[0] > mother.Genes[2] && father.Genes[0] > father.Genes[2])
                    copil.Genes[0] = mother.Genes[2] - a * mother.Genes[0] + father.Genes[0];
                else if (mother.Genes[0] <= mother.Genes[2] && output[2] == 0) copil.Genes[0] = mother.Genes[0];
                else if (output[2] == 0) copil.Genes[0] = father.Genes[0];

                //Verificam cazul 1 0 1
                if (output[2] == 1 && mother.Genes[0] <= mother.Genes[2] && father.Genes[0] <= father.Genes[2])
                    copil.Genes[0] = mother.Genes[2] + a * mother.Genes[0] - father.Genes[0];
                else if (mother.Genes[0] > mother.Genes[2] && output[2] == 0) copil.Genes[0] = mother.Genes[0];
                else if (output[2] == 0) copil.Genes[0] = father.Genes[0];

                //Verificam cazul 1 1 0
                //if (output[3] == 0 && (mother.Genes[0] + mother.Genes[1]) < mother.Genes[3] && (father.Genes[0] + father.Genes[1]) < father.Genes[3])

                return copil;
            }


        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta operatia de mutatie
    /// </summary>
    public class Mutation
    {
        private static Random _rand = new Random();

        public static void Reset(Chromosome child, double rate)
        {
            Random number = new Random();
            double probabilitate = number.NextDouble();

            for (int i = 0; i < child.NoGenes; i++)
            {
                if (rate > probabilitate)
                {                  
                    child.Genes[i] = number.NextDouble() * (child.MaxValues[i] - child.MinValues[i]) - child.Genes[i];
                }
            }
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care implementeaza algoritmul evolutiv pentru optimizare
    /// </summary>
    public class EvolutionaryAlgorithm
    {
        /// <summary>
        /// Metoda de optimizare care gaseste solutia problemei
        /// </summary>
        public Chromosome Solve(IOptimizationProblem p, int populationSize, int maxGenerations, double crossoverRate, double mutationRate)
        {
            //P0
            Chromosome[] population = new Chromosome[populationSize]; //A1, I1

            //P0.1
            for (int i = 0; i < population.Length; i++) //C0
            {
                Console.WriteLine("Iteratia " + i + "\n");
                //P1
                population[i] = p.MakeChromosome(); //A2, I2
                //P2
                p.ComputeFitness(population[i]); //A3, I3
            }

            //P2.1
            for (int gen = 0; gen < maxGenerations; gen++) //C1
            {
                //P3
                Chromosome[] newPopulation = new Chromosome[populationSize]; //A4, I4
                //P4
                newPopulation[0] = Selection.GetBest(population); // elitism //A5, I5
                if (newPopulation[0].Fitness > 0.75f) break;

                //P4.1
                for (int i = 0; i < population.Length; i++) //C2
                {

                    // selectare 2 parinti: Selection.Tournament
                    //P5
                    Chromosome mama = Selection.Tournament(population); //A6
                    //P6
                    Chromosome tata = Selection.Tournament(population); //A7
                    // generarea unui copil prin aplicare crossover: Crossover.Arithmetic
                    //P7
                    Chromosome copil = Crossover.Arithmetic(mama, tata, crossoverRate); //A8
                    // aplicare mutatie asupra copilului: Mutation.Reset
                    //P8
                    Mutation.Reset(copil, mutationRate); //A9
                    // calculare fitness pentru copil: ComputeFitness din problema p
                    //P9
                    p.ComputeFitness(copil); //A10
                    // introducere copil in newPopulation
                    //populationSize++;
                    //P10
                    newPopulation[i] = copil; //A11, I6
                    if (newPopulation[i].Fitness > 0.75f) break;
                }

                //P10.1
                for (int i = 0; i < populationSize; i++) //C3
                    //P11
                    population[i] = newPopulation[i]; //A12, I7
            }

            //P12
            return Selection.GetBest(population);
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta problema din prima aplicatie: rezolvarea ecuatiei
    /// </summary>
    public class optimizationProblem : IOptimizationProblem
    {
        public MainForm mainForm;
        public optimizationProblem() { }
        public optimizationProblem(MainForm form)
        {
            mainForm = form;
        }
        public MainForm getMainForm()
        {
            return mainForm;
        }

        public Chromosome MakeChromosome()
        {// initializare cu 3 | 9 gene, minValues și maxValues de lungime 3 | 9
            double[] initialWeights = mainForm.initialWeightsFromGUI();
            double[] minValues, maxValues;
            if (initialWeights.Length == 3)
            {
                minValues = new double[] { -1, -1, -1 };
                maxValues = new double[] { 1, 1, 1 };
            }
            else
            {
                minValues = new double[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                maxValues = new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            }

            return new Chromosome(initialWeights.Length, minValues, maxValues);
        }

        public void ComputeFitness(Chromosome c)
        {
            // Seteaza ponderile retelei neuronale cu cele din cromozom
            mainForm.UpdateNetworkWeights(c);

            // Calculeaza performanta retelei neuronale pe un set de date de test
            double fitness = mainForm.CalculateNetworkPerformance();

            // Seteaza fitness-ul cromozomului
            c.Fitness = fitness;
        }
    }
}
