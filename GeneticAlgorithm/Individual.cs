using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
  public class Individual
  {
    public int FitnessValue
    {
      get => _fitnessFunction(this);
    }

    private readonly Func<Individual, int> _fitnessFunction;

    public List<int[]> Chromosome { get; private set; }

    public Individual(List<int[]> chromosome, Func<Individual, int> fitnessFunction)
    {
      Chromosome = chromosome;
      _fitnessFunction = fitnessFunction;
    }
  }
}
