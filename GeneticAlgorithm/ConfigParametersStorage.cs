using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
  public static class ConfigParametersStorage
  {
    public static string CommutatorsAmount { get; } = "commutatorsAmount";

    public static string MaxConnectionsAmount { get; } = "maxConnectionsAmount";

    public static string ComputersAmount { get; } = "computersAmount";

    public static string LoopsAmountRestriction { get; } = "loopsAmountRestriction";

    public static string InitialPopulationAmount { get; } = "initialPopulationAmount";
  }
}
