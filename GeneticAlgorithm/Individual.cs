using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
  public class Individual
  {
    public int FitnessValue { get; private set; }

    private Matrix _chromosome;

    public Individual(Matrix chromosome)
    {
      _chromosome = chromosome;
    }
  }
}
