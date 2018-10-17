using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
  public class GeneticAlgorithm
  {
    private readonly IEnumerable<Individual> _parents;

    private readonly Individual _bestIndividual;

    private readonly int _loopsAmountRestriction;

    private readonly int _initialPopulationAmount;

    public GeneticAlgorithm(int loopsAmountRestriction, int initialPopulationAmount)
    {
      _loopsAmountRestriction = loopsAmountRestriction;
      _initialPopulationAmount = initialPopulationAmount;
    }

    public Individual GetBest()
    {
      _parents = InitializaParents();

      for(int i = 0; i < _loopsAmountRestriction; i++)
      {

      }

      return _bestIndividual;
    }

    private IEnumerable<Individual> InitializaParents()
    {
      for( int i = 0; i < _initialPopulationAmount; i++)
      {
        var chromosome = new Matrix(3, 7).Fil
        var parent = new Individual(chromosome);
        _parents.Append()
      }
    }

    public IEnumerable<Individual> Crossover(Individual firstParent, Individual secondParent)
    {
      var randomizer = new Random();
      var breakPoint = randomizer.Next(1, firstParent.Chromosome.Rows);

      
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
