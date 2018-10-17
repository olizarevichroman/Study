using Microsoft.Extensions.Configuration;
using System;

namespace GeneticAlgorithm
{
  class Program
  {
    static void Main(string[] args)
    {
      var configBuilder = new ConfigurationBuilder();
      var config = configBuilder.AddJsonFile($"{Environment.CurrentDirectory}/../../../appsettings.json").Build();

      var computersAmount = Int32.Parse(config[ConfigParametersStorage.ComputersAmount]);

      var commutatorsAmount = Int32.Parse(config[ConfigParametersStorage.CommutatorsAmount]);

      var trafficMatrix = new int[commutatorsAmount, commutatorsAmount].FillWithoutDiagonals(1, 100);

      var loopsAmountRestriction = Int32.Parse(config[ConfigParametersStorage.LoopsAmountRestriction]);

      var initialPopulationAmount = Int32.Parse(config[ConfigParametersStorage.InitialPopulationAmount]);

      var algorithm = new GeneticAlgorithm(loopsAmountRestriction, initialPopulationAmount);
      algorithm.GetBest();
    }
  }
}
