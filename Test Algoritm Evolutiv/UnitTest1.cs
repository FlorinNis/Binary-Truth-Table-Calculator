using Microsoft.VisualStudio.TestTools.UnitTesting;
using DecisionRegions;
using System;

namespace Test_Algoritm_Evolutiv
{
    [TestClass]
    public class SelectionTests
    {
        [TestMethod]
        public void Tournament_ReturnsChromosomeWithHigherFitness()
        {
            double[] initialWeights = new double[] { 0.5, -0.5, 0.2, -0.2, 0.1 };
            double[] minValues = new double[] { -1, -1, -1, -1, -1 };
            double[] maxValues = new double[] { 1, 1, 1, 1, 1 };
            // Arrange
            Chromosome[] population = new Chromosome[]
            {
            new Chromosome(initialWeights.Length, minValues, maxValues) { Fitness = 0.8 },
            new Chromosome(initialWeights.Length, minValues, maxValues) { Fitness = 0.6 }
            };

            // Act
            Chromosome winner = Selection.Tournament(population);

            // Assert
            Assert.AreEqual(population[0], winner);
        }

        [TestMethod]
        public void GetBest_ReturnsChromosomeWithHighestFitness()
        {
            double[] initialWeights = new double[] { 0.5, -0.5, 0.2, -0.2, 0.1 };
            double[] minValues = new double[] { -1, -1, -1, -1, -1 };
            double[] maxValues = new double[] { 1, 1, 1, 1, 1 };
            // Arrange
            Chromosome[] population = new Chromosome[]
            {
            new Chromosome(initialWeights.Length, minValues, maxValues) { Fitness = 0.8 },
            new Chromosome(initialWeights.Length, minValues, maxValues) { Fitness = 0.6 }
            };

            // Act
            Chromosome best = Selection.GetBest(population);

            // Assert
            Assert.AreEqual(population[0], best);
        }
    }

    [TestClass]
    public class CrossoverTests
    {
        [TestMethod]
        public void Arithmetic_ReturnsChildWithGenesFromFatherOrMotherBasedOnRate()
        {
            double[] initialWeights = new double[] { 0.5, -0.5, 0.2, -0.2, 0.1 };
            double[] minValues = new double[] { -1, -1, -1, -1, -1 };
            double[] maxValues = new double[] { 1, 1, 1, 1, 1 };
            // Arrange
            Chromosome mother = new Chromosome(initialWeights.Length, minValues, maxValues);
            Chromosome father = new Chromosome(initialWeights.Length, minValues, maxValues);
            double rate = 1;

            // Act
            Chromosome child = Crossover.Arithmetic(mother, father, rate);

            // Assert
            CollectionAssert.AreNotEqual(mother.Genes, child.Genes);
        }
    }

    [TestClass]
    public class MutationTests
    {
        [TestMethod]
        public void Reset_UpdatesGenesWithProbabilityBasedOnRate()
        {
            double[] initialWeights = new double[] { 0.5, -0.5, 0.2, -0.2, 0.1 };
            double[] minValues = new double[] { -1, -1, -1, -1, -1 };
            double[] maxValues = new double[] { 1, 1, 1, 1, 1 };
            // Arrange
            Chromosome child = new Chromosome(initialWeights.Length, minValues, maxValues);
            double rate = 0.2;

            // Act
            Mutation.Reset(child, rate);

            // Assert
            CollectionAssert.AreNotEqual(new double[] { 1, 2, 3 }, child.Genes);
        }
    }

    [TestClass]
    public class ChromosomeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "minValues and maxValues must have the same length as noGenes.")]
        public void Constructor_ThrowsExceptionWhenLengthMismatch()
        {
            // Arrange
            int noGenes = 3;
            double[] minValues = { 0, 1, 2 };
            double[] maxValues = { 1, 2 };  // Mismatched length

            // Act
            Chromosome chromosome = new Chromosome(noGenes, minValues, maxValues);
        }

        [TestMethod]
        public void Constructor_InitializesGenesWithinRange()
        {
            // Arrange
            int noGenes = 3;
            double[] minValues = { 0, 1, 2 };
            double[] maxValues = { 1, 2, 3 };

            // Act
            Chromosome chromosome = new Chromosome(noGenes, minValues, maxValues);

            // Assert
            Assert.AreEqual(noGenes, chromosome.NoGenes);

            for (int i = 0; i < noGenes; i++)
            {
                Assert.IsTrue(chromosome.Genes[i] >= minValues[i]);
                Assert.IsTrue(chromosome.Genes[i] <= maxValues[i]);
            }
        }
    }


}
