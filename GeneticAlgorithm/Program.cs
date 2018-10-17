using Microsoft.Extensions.Configuration;
using System;

namespace GeneticAlgorithm
{
  class Program
  {
    static void Main(string[] args)
    {
      var configBuilder = new ConfigurationBuilder();
      var config = configBuilder.AddJsonFile("appsettings.json").Build();

      var computersAmount = Int32.Parse(config[ConfigParametersStorage.ComputersAmount]);

      var trafficMatrix = new Matrix(computersAmount).FillRandomValues();
    }
  }
}
