using Newtonsoft.Json;
using System;

namespace GeneticAlgorithm
{
  [Serializable]
  public class AlgorithmData
  {
    [JsonProperty(ConfigParametersStorage.COMPUTERS_AMOUNT)]
    public int ComputersAmount { get; set; }

    [JsonProperty(ConfigParametersStorage.COMMUTATORS_AMOUNT)]
    public int CommutatorsAmount { get; set; }

    [JsonProperty(ConfigParametersStorage.LOOPS_AMOUNT_RESTRICTION)]
    public int LoopsAmountRestriction { get; set; }

    [JsonProperty(ConfigParametersStorage.INITIAL_POPULATIONS_AMOUNT)]
    public int InitialPopulationAmount { get; set; }

    [JsonProperty(ConfigParametersStorage.MAX_CONNECTIONS_AMOUNT)]
    public int MaxConnectionsAmount { get; set; }

    [JsonProperty(ConfigParametersStorage.MUTATION_PROBABILITY)]
    public double MutationProbability { get; set; }
  }
}
