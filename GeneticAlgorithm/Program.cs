using Microsoft.Extensions.Configuration;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
  class Program
  {
    static void Main(string[] args)
    {
      string jsonData;

      using(var reader = new StreamReader($"{Environment.CurrentDirectory}/../../../appsettings.json"))
      {
        jsonData = reader.ReadToEnd();
      }

      var algorithmData = JsonConvert.DeserializeObject<AlgorithmData>(jsonData);

      var trafficMatrix = new List<int[]>().FillWithoutDiagonals(50, 140, algorithmData.ComputersAmount);

      var algorithm = new GeneticAlgorithm(algorithmData, trafficMatrix);
      Console.WriteLine(algorithm.GetBest());
    }
  }
}
