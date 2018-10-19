using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
      int result = 0;

      foreach(var individual in individuals)
      {
        result += GetFitnessValue(individual);
      }
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
      return result;
    }

    /// <summary>
    /// Calculate fitness value of single individual
    /// </summary>
    /// <param name="individual">Individual</param>
    /// <returns>Value of fitness</returns>
    private int GetFitnessValue(Individual individual)
    {
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
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
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
      return result;
    }

    /// <summary>
    /// Find the best individual
    /// </summary>
    /// <returns>Best individual</returns>
    public Individual GetBest()
    {
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
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
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
      return _bestIndividual;
    }

    /// <summary>
    /// Creates initial population
    /// </summary>
    /// <returns>Initial population</returns>
    private List<Individual> InitializaParents()
    {
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
      var parents = new List<Individual>(_algorithmData.InitialPopulationAmount);

      for( int i = 0; i < _algorithmData.InitialPopulationAmount; i++)
      {
        var chromosome = new List<int[]>()
          .FillRows(_algorithmData.ComputersAmount, _algorithmData.CommutatorsAmount, ChromosomeFiller);
         
        var parent = new Individual(chromosome, GetFitnessValue);

        parents.Add(parent);
      }
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
      return parents;
    }

    /// <summary>
    /// Fills individual chromosome relying on restrictions
    /// </summary>
    /// <param name="chromosome">Result chromosome</param>
    private void ChromosomeFiller(List<int[]> chromosome)
    {
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
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
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
    }

    /// <summary>
    /// Calculates total value of column
    /// </summary>
    /// <param name="rows">Maxtix</param>
    /// <param name="columnNumber">Column number to calculate</param>
    /// <returns>Total value</returns>
    private int GetColumnSum(List<int[]> rows, int columnNumber)
    {
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
      int result = 0;

      foreach(var row in rows)
      {
        result += row[columnNumber];
      }
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
      return result;
    }

    /// <summary>
    /// Crossover parents 
    /// </summary>
    /// <param name="parents">Parents</param>
    /// <returns>Crossovered childs</returns>
    public List<Individual> Crossover(List<Individual> parents)
    {
      Console.WriteLine(MethodBase.GetCurrentMethod().Name);
      var randomizedIndexes = GenerateShuffledList(parents.Count);

      List<Individual> childs = new List<Individual>(parents.Count);

      var randomizer = new Random();
      var breakPoint = randomizer.Next(1, parents.First().Chromosome.Count);
      Console.WriteLine("Tournir");
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
      Console.WriteLine("Tournir finished");
      Console.WriteLine("Checking for restrictions started");
      for (int i = 0; i < childs.Count; i++)
      {
        if(IsСompatibleToRestrictions(childs[i]) == false)
        {
          Repair(childs[i]);
        }
      }
      Console.WriteLine("Checking for restrictions finished");
      Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
      return childs;
    }

    /// <summary>
    /// Mutate individuals with established mutation probability
    /// </summary>
    /// <param name="individuals">Individuals to mutate</param>
    /// <returns>New invividuals with random persent of mutated instances</returns>
    public List<Individual> TryMutate(List<Individual> individuals)
    {
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
      var randomizer = new Random();

      for(int i = 0; i < individuals.Count; i++)
      {
        if (randomizer.Next(1, 101) <= _algorithmData.MutationProbability * 100)
        {
          var newChromosome = new List<int[]>().FillRows(_algorithmData.ComputersAmount, _algorithmData.CommutatorsAmount, ChromosomeFiller);

          individuals[i] = new Individual(newChromosome, GetFitnessValue);
        }
      }
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
      return individuals;
    }

    /// <summary>
    /// Difines is individual compatible to the restrictions
    /// </summary>
    /// <param name="individual">Individual to check</param>
    /// <returns>Is individual compatible</returns>
    private bool IsСompatibleToRestrictions(Individual individual)
    {
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
      for (int i = 0; i < _algorithmData.CommutatorsAmount; i++)
      {
        if(GetColumnSum(individual.Chromosome, i) > _algorithmData.MaxConnectionsAmount)
        {
          return false;
        }
      }
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
      return true;
    }

    /// <summary>
    /// Repair individual after crossover if individual does not sutisfy restrictions
    /// </summary>
    /// <param name="individual">Individual to repair</param>
    private void Repair(Individual individual)
    {
      Console.WriteLine(MethodBase.GetCurrentMethod().Name);
      var invalidColumns = new Dictionary<int, int>();//key - columns index, value - amount of the extra connections

      int currentColumnSum;

      var chromosome = individual.Chromosome;

      for (int i = 0; i < _algorithmData.CommutatorsAmount; i++)
      {
        currentColumnSum = GetColumnSum(individual.Chromosome, i);

        if (currentColumnSum > _algorithmData.MaxConnectionsAmount)
        {
          invalidColumns.Add(i, currentColumnSum - _algorithmData.MaxConnectionsAmount);
        }
      }

      foreach(var invalidColumnValues in invalidColumns)
      {
        int columnTotalValue;
        int extraConnectionsAmount = invalidColumnValues.Value;
        
        while (extraConnectionsAmount != 0)
        {
          bool isRelocated = false;

          for (int columnNumber = 0; columnNumber < _algorithmData.CommutatorsAmount && isRelocated == false; columnNumber++)
          {
            if (invalidColumns.ContainsKey(columnNumber))
            {
              continue;
            }

            columnTotalValue = GetColumnSum(individual.Chromosome, columnNumber);

            if (columnTotalValue < _algorithmData.MaxConnectionsAmount)
            {
              //here we have free column (i index) to relocate connection
              for (int rowNumber = 0; rowNumber < _algorithmData.ComputersAmount; rowNumber++)
              {
                //here we should find what computer(row index) we need to relocate from invalid column
                if(chromosome[rowNumber][invalidColumnValues.Key] == 1)
                {
                  chromosome[rowNumber][invalidColumnValues.Key] = 0;
                  chromosome[rowNumber][columnNumber] = 1;
                  extraConnectionsAmount--;
                  isRelocated = true;

                  break;
                }
              }
            }
          }
        }
      }
      Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
    }

    /// <summary>
    /// Generate shuffled list filled with values from 0 to listLength parameter/>
    /// </summary>
    /// <param name="listLength">Length and max value in list</param>
    /// <returns>Shuffled list</returns>
    private List<int> GenerateShuffledList(int listLength)
    {
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
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
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
      return randomizedIndexes;
    }

    /// <summary>
    /// Select best individuals
    /// </summary>
    /// <param name="individuals">Individuals for selection</param>
    /// <returns></returns>
    public List<Individual> Select(List<Individual> individuals)
    {
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name);
      var randomizedIndexes = GenerateShuffledList(individuals.Count);

      var winners = new List<Individual>(individuals.Count / 2);

      for ( int i = 0; i < randomizedIndexes.Count - 1; i += 2)
      {
        var first = individuals[i];
        var second = individuals[i + 1];

        winners.Add(first.FitnessValue > second.FitnessValue ? first : second);
      }
      //Console.WriteLine(MethodBase.GetCurrentMethod().Name + "returned");
      return winners;
    }
  }
}
