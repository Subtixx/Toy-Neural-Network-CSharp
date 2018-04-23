using System;
using System.Collections.Generic;
using System.IO;

namespace ToyNN
{
    public class Matrix
    {
        private static readonly Random Random = new Random((int) DateTime.Now.Ticks);
        internal int _cols;

        internal float[][] _data;
        internal int _rows;

        public Matrix(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;

            _data = new float[rows][];
            for (var row = 0; row < _rows; row++)
            {
                _data[row] = new float[_cols];
                for (var col = 0; col < _cols; col++) _data[row][col] = 0;
            }
        }

        public int Columns => _cols;
        public int Rows => _rows;
        public float[][] Data => _data;

        public Matrix Copy()
        {
            var m = new Matrix(_rows, _cols);
            for (var row = 0; row < _rows; row++)
            for (var col = 0; col < _cols; col++)
                m._data[row][col] = _data[row][col];

            return m;
        }

        public static Matrix FromArray(IReadOnlyList<float> array)
        {
            return new Matrix(array.Count, 1).Map((val, row, col) => array[row]);
        }

        // TODO: Use operator?
        public static Matrix Subtract(Matrix lh, Matrix rh)
        {
            if (lh._rows != rh._rows || lh._cols != rh._cols)
                throw new Exception("Columns and rows must match.");

            return new Matrix(lh._rows, lh._cols).Map((val, row, col) => lh._data[row][col] - rh._data[row][col]);
        }

        public float[] ToArray()
        {
            var array = new List<float>();
            for (var row = 0; row < _rows; row++)
            for (var col = 0; col < _cols; col++)
                array.Add(_data[row][col]);

            return array.ToArray();
        }

        public Matrix Randomize()
        {
            return Map((val, row, col) => { return Random.NextDouble() * 2.0f - 1.0f; });
        }

        // TODO: Use operator?
        public Matrix Add(Matrix m)
        {
            if (m._rows != _rows || m._cols != _cols)
                throw new Exception("Columns and rows must match.");

            return Map((val, row, col) => val + m._data[row][col]);
        }

        // TODO: Use operator?
        public Matrix Add(float m)
        {
            return Map((val, row, col) => val + m);
        }

        public static Matrix Transpose(Matrix m)
        {
            return new Matrix(m._cols, m._rows).Map((val, row, col) => m._data[col][row]);
        }

        // TODO: Use operator?
        public static Matrix Multiply(Matrix lh, Matrix rh)
        {
            if (lh._cols != rh._rows)
                throw new Exception("Columns must match Rows");

            return new Matrix(lh._rows, rh._cols)
                .Map((e, i, j) =>
                {
                    // Dot product of values in col
                    var sum = 0.0f;
                    for (var k = 0; k < lh._cols; k++) sum += lh._data[i][k] * rh._data[k][j];

                    return sum;
                });
        }

        public Matrix Multiply(Matrix lh)
        {
            if (_rows != lh._rows || _cols != lh._cols)
                throw new Exception("Columns and rows must match.");

            // hadamard product
            return Map((val, row, col) => val * lh._data[row][col]);
        }

        public Matrix Multiply(float m)
        {
            return Map((val, row, col) => val * m);
        }

        public Matrix Map(Func<float, int, int, float> func)
        {
            for (var row = 0; row < _rows; row++)
            for (var col = 0; col < _cols; col++)
            {
                var val = _data[row][col];
                _data[row][col] = func(val, row, col);
            }

            return this;
        }

        public Matrix Map(Func<float, int, int, double> func)
        {
            for (var row = 0; row < _rows; row++)
            for (var col = 0; col < _cols; col++)
            {
                var val = _data[row][col];
                _data[row][col] = (float) func(val, row, col);
            }

            return this;
        }

        public static Matrix Map(Matrix m, Func<float, int, int, float> func)
        {
            return new Matrix(m._rows, m._cols)
                .Map((e, i, j) => func(m._data[i][j], i, j));
        }

        // Serialize
        // Deserialize
        public void Save(BinaryWriter bw)
        {
            bw.Write(_rows);
            bw.Write(_cols);
            for (var row = 0; row < _rows; row++)
            for (var col = 0; col < _cols; col++)
                bw.Write(_data[row][col]);
        }

        public void Load(BinaryReader br)
        {
            _rows = br.ReadInt32();
            _cols = br.ReadInt32();

            _data = new float[_rows][];
            for (var row = 0; row < _rows; row++)
            {
                _data[row] = new float[_cols];
                for (var col = 0; col < _cols; col++)
                    _data[row][col] = br.ReadSingle();
            }
        }
    }
}