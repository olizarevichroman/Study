using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
  public class GeneticAlgorithm
  {
    private IEnumerable<Individual> _parents;

    private readonly Individual _bestIndividual;

    private readonly int _loopsAmountRestriction;

    private readonly int _initialPopulationAmount;

    public int[,] TrafficMatrix { get; private set; }

    public GeneticAlgorithm(int loopsAmountRestriction, int initialPopulationAmount)
    {
      _loopsAmountRestriction = loopsAmountRestriction;
      _initialPopulationAmount = initialPopulationAmount;
    }

    public int GetFitnessValue(Individual individual)
    {
      var chromosome = individual.Chromosome;

      int result = 0;

      var length = individual.Chromosome.GetLength(0);

      for (int i = 0; i < length; i++)
      {
        for (int j = 0; j < length; j++)
        {
          if(TrafficMatrix[i, j] == 0)
          {
            continue;
          }

          throw new NotImplementedException();
        }
      }

      return result;
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
      var parents = new List<Individual>(_initialPopulationAmount);

      for( int i = 0; i < _initialPopulationAmount; i++)
      {
        var chromosome = new int[5, 8]
          .FillWithRowsRestriction(0, 1, (a => a.Where((element) => element == 1).Count() < 1));
         
        var parent = new Individual(chromosome);
        parents.Add(parent);
      }

      return parents;
    }

    public IEnumerable<Individual> Crossover(Individual firstParent, Individual secondParent)
    {
      var randomizer = new Random();
      var breakPoint = randomizer.Next(1, firstParent.Chromosome.Length);


      return new Individual[] { new Individual(null) };
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
