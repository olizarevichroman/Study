using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
  public class GeneticAlgorithm
  {

    private Individual _bestIndividual;

    private readonly AlgorithmData _algorithmData;

    public List<int[]> TrafficMatrix { get; private set; }

    public GeneticAlgorithm(AlgorithmData algorithmData, List<int[]> trafficMatrix)
    {
      _algorithmData = algorithmData;
      TrafficMatrix = trafficMatrix;
    }

    /// <summary>
    /// Calculate fitness value from collection of individuals
    /// </summary>
    /// <param name="individuals">Collection of individuals</param>
    /// <returns>Value of total fitness</returns>
    private int GetFitnessValue(List<Individual> individuals)
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
      var initialparentsPool = InitializaParents();

      List<Individual> currentParentsPool = new List<Individual>();

      List<Individual> previousParentsPool = initialparentsPool;

      for(int i = 0; i < _algorithmData.LoopsAmountRestriction; i++)
      {
        currentParentsPool = Select(previousParentsPool);

        var childs = Crossover(currentParentsPool);
        currentParentsPool.AddRange(childs);

        var childsFitness = GetFitnessValue(currentParentsPool);
        var parentsFitness = GetFitnessValue(previousParentsPool);
        
        if(childsFitness >= parentsFitness)
        {
          currentParentsPool = TryMutate(currentParentsPool);
        }

        if (_bestIndividual == null)
        {
          _bestIndividual = currentParentsPool.First();
        }

        foreach (var individual in currentParentsPool)
        {
          if (_bestIndividual.FitnessValue > individual.FitnessValue)
          {
            _bestIndividual = individual;
          }
        }

        previousParentsPool = currentParentsPool;
      }

      return _bestIndividual;
    }

    /// <summary>
    /// Creates initial population
    /// </summary>
    /// <returns>Initial population</returns>
    private List<Individual> InitializaParents()
    {
      var parents = new List<Individual>(_algorithmData.InitialPopulationAmount);

      for( int i = 0; i < _algorithmData.InitialPopulationAmount; i++)
      {
        var chromosome = new List<int[]>()
          .FillRows(_algorithmData.ComputersAmount, _algorithmData.CommutatorsAmount, ChromosomeFiller);
         
        var parent = new Individual(chromosome, GetFitnessValue);

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

        row[index] = 1;
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
    public List<Individual> Crossover(List<Individual> parents)
    {
      var randomizedIndexes = GenerateShuffledList(parents.Count);

      List<Individual> childs = new List<Individual>(parents.Count);

      var randomizer = new Random();
      var breakPoint = randomizer.Next(1, parents.First().Chromosome.Count);

      for (int i = 0; i < parents.Count - 1; i += 2)
      {
        var firstParent = parents[i];
        var secondParent = parents[i + 1];

        List<int[]> firstChildChromosome = firstParent.Chromosome.Take(breakPoint)
          .Concat(secondParent.Chromosome.Skip(breakPoint)).ToList();

        var firstChild = new Individual(firstChildChromosome, GetFitnessValue);

        List<int[]> secondChildChromosome = secondParent.Chromosome.Take(breakPoint)
          .Concat(firstParent.Chromosome.Skip(breakPoint)).ToList();

        childs.Add(firstChild);
        childs.Add(secondParent);
      }

      return childs;
    }

    public List<Individual> TryMutate(List<Individual> individuals)
    {
      var randomizer = new Random();

      foreach (var individual in individuals)
      {
        foreach (var row in individual.Chromosome)
        {
          if (randomizer.Next(1, 101) <= _algorithmData.MutationProbability * 100)
          {
            Array.ForEach(row, (value) => value = value == 1 ? 0 : 1);
          }
        }
      }

      return individuals;
    }

    private List<int> GenerateShuffledList(int listLength)
    {
      var individualsIndexes = new List<int>(listLength);
      var randomizedIndexes = new List<int>(listLength);

      for (int i = 0; i < listLength; i++)
      {
        individualsIndexes.Add(i);
      }

      var randomizer = new Random();

      for (int i = 0; individualsIndexes.Count != 0; i++)
      {
        var index = randomizer.Next(0, individualsIndexes.Count);
        randomizedIndexes.Add(individualsIndexes[index]);
        individualsIndexes.RemoveAt(index);
      }

      return randomizedIndexes;
    }

    public List<Individual> Select(List<Individual> individuals)
    {
      var randomizedIndexes = GenerateShuffledList(individuals.Count);

      var winners = new List<Individual>(individuals.Count / 2);

      for ( int i = 0; i < randomizedIndexes.Count - 1; i += 2)
      {
        var first = individuals[i];
        var second = individuals[i + 1];

        winners.Add(first.FitnessValue > second.FitnessValue ? first : second);
      }

      return winners;
    }
  }
}
