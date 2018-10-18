using Microsoft.Extensions.Configuration;
using System;
using Newtonsoft.Json;
using System.IO;

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

      var algorithm = new GeneticAlgorithm(algorithmData);
      Console.WriteLine(algorithm.GetBest());
    }
  }
}
