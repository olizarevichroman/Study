using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
  public class GeneticAlgorithm
  {
    private readonly IEnumerable<Individual> _parents;

    private readonly Individual _bestIndividual;

    private readonly int _loopsAmountRestriction;

    public GeneticAlgorithm(int loopsAmountRestriction)
    {
      _loopsAmountRestriction = loopsAmountRestriction;
    }

    public Individual GetBest()
    {
      for(int i = 0; i < _loopsAmountRestriction; i++)
      {

      }

      return _bestIndividual;
    }

    public void Crossover(Individual firstParent, Individual secondParent)
    {
      throw new NotImplementedException();
    }

    public Individual Mutate(Individual individual)
    {
      throw new NotImplementedException();
    }
    public Individual Select(Individual first, Individual second)
    {
      return (first.FitnessValue > second.FitnessValue ? first : second);
    }
  }
}
