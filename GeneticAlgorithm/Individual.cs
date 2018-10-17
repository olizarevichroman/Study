using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
  public class Individual
  {
    public int FitnessValue { get; private set; }

    public int[,] Chromosome { get; private set; }

    public Individual(int[,] chromosome)
    {
      Chromosome = chromosome;
    }
  }
}
