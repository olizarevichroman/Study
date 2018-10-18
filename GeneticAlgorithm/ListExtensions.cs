using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GeneticAlgorithm
{
  public static class ListExtensions
  {
    public static  List<int[]> FillWithoutDiagonals(this List<int[]> rows, int minValue, int maxValue, int rowsAmount, int columnsAmount)
    {
      var randomizer = new Random();

      for(int i = 0; i < rowsAmount; i++)
      {
        rows.Add(new int[columnsAmount]);

        for(int j = 0; j < rows[i].Length; j++)
        {
          if(i == j)
          {
            continue;
          }

          rows[i][j] = randomizer.Next(minValue, maxValue + 1);
        }
      }

      return rows;
    }
    public static List<int[]> FillRows(this List<int[]> rows, int rowsAmount, int columnsAmount, Action<List<int[]>> action )
    {
      var randomizer = new Random();

      for(int i = 0; i < rowsAmount; i++)
      {
        rows.Add(new int[columnsAmount]);   
      }

      action(rows);

      return rows;
    }
  }
}
