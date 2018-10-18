using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
  public class Individual
  {
    public int FitnessValue { get; private set; }

    public List<int[]> Chromosome { get; private set; }

    public Individual(List<int[]> chromosome)
    {
      Chromosome = chromosome;
    }
  }
}
