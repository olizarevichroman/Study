using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GeneticAlgorithm
{
  public static class ArrayExtensions
  {
    public static  int[,] FillWithoutDiagonals(this int[,] array, int minValue, int maxValue)
    {
      var randomizer = new Random();

      for(int i = 0; i <= array.GetUpperBound(0); i++)
      {
        for(int j = 0; j <= array.GetUpperBound(1); j++)
        {
          if(i == j)
          {
            continue;
          }

          array[i, j] = randomizer.Next(minValue, maxValue + 1);
        }
      }

      return array;
    }

    public static int[,] FillWithRowsRestriction(this int[,] array, int minValue, int maxValue, Predicate<int[]> predicate )
    {
      var randomizer = new Random();

      var secondDimensionLength = array.GetLength(1);

      var currentArrayInlineRepresentation = array.OfType<int>();

      for(int i = 0; i <= array.GetUpperBound(0); i++)
      {

        var row = currentArrayInlineRepresentation.Skip(i * secondDimensionLength)
          .Take(secondDimensionLength);

        for(int j = 0; j <= array.GetUpperBound(1); j++)
        {
          if (predicate(row.ToArray()) == false)
          {
            break;
          }

          array[i, j] = randomizer.Next(minValue, maxValue + 1);
        }
      }

      return array;
    }

    public static int[,] FillWithColumnsRestriction(this int[,] array, int minValue, int maxValue, Predicate<int[]> predicate)
    {
      throw new NotImplementedException();
    }
  }
}
