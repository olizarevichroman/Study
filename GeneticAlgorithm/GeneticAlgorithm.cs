using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
  public class GeneticAlgorithm
  {
    private IEnumerable<Individual> _parents;

    private readonly Individual _bestIndividual;

    private readonly AlgorithmData _algorithmData;

    public List<int[]> TrafficMatrix { get; private set; }

    public GeneticAlgorithm(AlgorithmData algorithmData)
    {
      _algorithmData = algorithmData;
    }

    /// <summary>
    /// Calculate fitness value from collection of individuals
    /// </summary>
    /// <param name="individuals">Collection of individuals</param>
    /// <returns>Value of total fitness</returns>
    private int GetFitnessValue(IEnumerable<Individual> individuals)
    {
      int result = 0;

      foreach(var individual in individuals)
      {
        result += GetFitnessValue(individual);
      }

      return result;
    }

    /// <summary>
    /// Calculate fitness value of single individual
    /// </summary>
    /// <param name="individual">Individual</param>
    /// <returns>Value of fitness</returns>
    private int GetFitnessValue(Individual individual)
    {
      var chromosome = individual.Chromosome;

      int result = 0;

      for (int i = 0; i < individual.Chromosome.Count; i++)
      {
        var senderCommutatorNumber = Array.FindIndex(chromosome[i], (value) => value == 1);

        for (int j = 0; j < individual.Chromosome.Count; j++)
        {
          if(TrafficMatrix[i][j] == 0)
          {
            continue;
          }

          var receiverCommutatorNumber = Array.FindIndex(chromosome[j], (value) => value == 1);

          if(senderCommutatorNumber != receiverCommutatorNumber)
          {
            result += TrafficMatrix[i][j];
          }
        }
      }

      return result;
    }
    public Individual GetBest()
    {
      _parents = InitializaParents();

      for(int i = 0; i < _algorithmData.LoopsAmountRestriction; i++)
      {

      }

      return _bestIndividual;
    }

    /// <summary>
    /// Creates initial population
    /// </summary>
    /// <returns>Initial population</returns>
    private IEnumerable<Individual> InitializaParents()
    {
      var parents = new List<Individual>(_algorithmData.InitialPopulationAmount);

      for( int i = 0; i < _algorithmData.InitialPopulationAmount; i++)
      {
        var chromosome = new List<int[]>()
          .FillRows(_algorithmData.ComputersAmount, _algorithmData.CommutatorsAmount, ChromosomeFiller);
         
        var parent = new Individual(chromosome);
        parents.Add(parent);
      }

      return parents;
    }

    /// <summary>
    /// Fills individual chromosome relying on restrictions
    /// </summary>
    /// <param name="chromosome">Result chromosome</param>
    private void ChromosomeFiller(List<int[]> chromosome)
    {
      var randomizer = new Random();

      int index = 0;

      foreach(var row in chromosome)
      {
        index = randomizer.Next(0, _algorithmData.CommutatorsAmount);

        while (GetColumnSum(chromosome, index) >= _algorithmData.MaxConnectionsAmount)
        {          
          index = randomizer.Next(0, _algorithmData.CommutatorsAmount);
        }

        row[index] = randomizer.Next(0, 2);
      }
    }

    /// <summary>
    /// Calculates total value of column
    /// </summary>
    /// <param name="rows">Maxtix</param>
    /// <param name="columnNumber">Column number to calculate</param>
    /// <returns>Total value</returns>
    private int GetColumnSum(List<int[]> rows, int columnNumber)
    {
      int result = 0;

      foreach(var row in rows)
      {
        result += row[columnNumber];
      }

      return result;
    }
    public IEnumerable<Individual> Crossover(Individual firstParent, Individual secondParent)
    {
      var randomizer = new Random();
      var breakPoint = randomizer.Next(0, firstParent.Chromosome.Count - 1);


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
