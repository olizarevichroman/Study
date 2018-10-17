using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
  public class Matrix
  {
    public int Rows { get; private set; }

    public int Columns { get; private set; }

    private readonly int[,] _values;

    public Matrix(int rows, int columns)
    {
      Rows = rows;
      Columns = columns;
      _values = new int[rows, columns];
    }

    public Matrix(int dimension) : this(dimension, dimension) { }

    public Matrix FillRandomValues(int min, int max)
    {
      var randomizer = new Random();

      for(int i =0; i < Rows; i++)
      {
        for(int j =0; j < Columns; j++)
        {
          _values[i, j] = randomizer.Next(min, max);
        }
      }

      return this;
    }


  }
}
