using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
  public class Matrix
  {
    private readonly int _rows;

    private readonly int _columns;

    private readonly int[,] _values;

    public Matrix(int rows, int columns)
    {
      _rows = rows;
      _columns = columns;
      _values = new int[rows, columns];
    }

    public Matrix FillRandomValues(int min, int max)
    {
      throw new NotImplementedException();
    }


  }
}
