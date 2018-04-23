using NUnit.Framework;

namespace ToyNN.Tests
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void AddMatrixToOtherMatrix()
        {
            var m = new Matrix(2, 2)
            {
                _data =
                {
                    [0] = new[] {1.0f, 2.0f},
                    [1] = new[] {3.0f, 4.0f}
                }
            };

            var m2 = new Matrix(2, 2)
            {
                _data =
                {
                    [0] = new[] {10.0f, 11.0f},
                    [1] = new[] {12.0f, 13.0f}
                }
            };

            m.Add(m2);

            Assert.AreEqual(new[]
            {
                new[] {11.0f, 13.0f},
                new[] {15.0f, 17.0f}
            }, m._data);
        }

        [Test]
        public void AddScalarToMatrix()
        {
            var m = new Matrix(3, 3)
            {
                _data =
                {
                    [0] = new[] {1.0f, 2.0f, 3.0f},
                    [1] = new[] {4.0f, 5.0f, 6.0f},
                    [2] = new[] {7.0f, 8.0f, 9.0f}
                }
            };
            m.Add(1);

            Assert.AreEqual(new[]
            {
                new[] {2.0f, 3.0f, 4.0f},
                new[] {5.0f, 6.0f, 7.0f},
                new[] {8.0f, 9.0f, 10.0f}
            }, m._data);
        }

        [Test]
        public void ChainingMatrixMethods()
        {
            var m = new Matrix(3, 3)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f, 3.0f},
                    new[] {4.0f, 5.0f, 6.0f},
                    new[] {7.0f, 8.0f, 9.0f}
                }
            };
            m.Map((val, row, col) => val - 1).Multiply(10).Add(6);

            Assert.AreEqual(3, m._rows);
            Assert.AreEqual(3, m._cols);
            Assert.AreEqual(new[]
            {
                new[] {6.0f, 16.0f, 26.0f},
                new[] {36.0f, 46.0f, 56.0f},
                new[] {66.0f, 76.0f, 86.0f}
            }, m._data);
        }

        [Test]
        public void HadamardProduct()
        {
            var m = new Matrix(3, 2)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f},
                    new[] {3.0f, 4.0f},
                    new[] {5.0f, 6.0f}
                }
            };

            var m2 = new Matrix(3, 2)
            {
                _data = new[]
                {
                    new[] {7.0f, 8.0f},
                    new[] {9.0f, 10.0f},
                    new[] {11.0f, 12.0f}
                }
            };
            m.Multiply(m2);

            Assert.AreEqual(new[]
            {
                new[] {7.0f, 16.0f},
                new[] {27.0f, 40.0f},
                new[] {55.0f, 72.0f}
            }, m._data);
        }

        [Test]
        public void InstanceMapWithRowAndColumnParams()
        {
            var m = new Matrix(3, 3)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f, 3.0f},
                    new[] {4.0f, 5.0f, 6.0f},
                    new[] {7.0f, 8.0f, 9.0f}
                }
            };

            m.Map((val, row, col) => val * 100 + row * 10 + col);

            Assert.AreEqual(3, m._rows);
            Assert.AreEqual(3, m._cols);
            Assert.AreEqual(new[]
            {
                new[] {100.0f, 201.0f, 302.0f},
                new[] {410.0f, 511.0f, 612.0f},
                new[] {720.0f, 821.0f, 922.0f}
            }, m._data);
        }

        [Test]
        public void MappingWithInstanceMap()
        {
            var m = new Matrix(3, 3)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f, 3.0f},
                    new[] {4.0f, 5.0f, 6.0f},
                    new[] {7.0f, 8.0f, 9.0f}
                }
            };

            m.Map((val, row, col) => val * 10);

            Assert.AreEqual(3, m._rows);
            Assert.AreEqual(3, m._cols);
            Assert.AreEqual(new[]
            {
                new[] {10.0f, 20.0f, 30.0f},
                new[] {40.0f, 50.0f, 60.0f},
                new[] {70.0f, 80.0f, 90.0f}
            }, m._data);
        }

        [Test]
        public void MappingWithStaticMap()
        {
            var m = new Matrix(3, 3)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f, 3.0f},
                    new[] {4.0f, 5.0f, 6.0f},
                    new[] {7.0f, 8.0f, 9.0f}
                }
            };

            var mMapped = Matrix.Map(m, (val, row, col) => val * 10);

            Assert.AreEqual(3, mMapped._rows);
            Assert.AreEqual(3, mMapped._cols);
            Assert.AreEqual(new[]
            {
                new[] {10.0f, 20.0f, 30.0f},
                new[] {40.0f, 50.0f, 60.0f},
                new[] {70.0f, 80.0f, 90.0f}
            }, mMapped._data);
        }

        [Test]
        public void MatrixCopy()
        {
            var m = new Matrix(5, 5);
            m.Randomize();

            var m2 = m.Copy();

            Assert.AreEqual(m._cols, m2._cols);
            Assert.AreEqual(m._rows, m2._rows);
            Assert.AreEqual(m._data, m2._data);
        }

        [Test]
        public void MatrixFromArray()
        {
            var array = new[] {1.0f, 2.0f, 3.0f};
            var m = Matrix.FromArray(array);
            Assert.AreEqual(3, m._rows);
            Assert.AreEqual(1, m._cols);
            Assert.AreEqual(new[]
            {
                new[] {1.0f},
                new[] {2.0f},
                new[] {3.0f}
            }, m._data);
        }

        [Test]
        public void MatrixProduct()
        {
            var m = new Matrix(2, 3)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f, 3.0f},
                    new[] {4.0f, 5.0f, 6.0f}
                }
            };
            var m2 = new Matrix(3, 2)
            {
                _data = new[]
                {
                    new[] {7.0f, 8.0f},
                    new[] {9.0f, 10.0f},
                    new[] {11.0f, 12.0f}
                }
            };

            var mProduct = Matrix.Multiply(m, m2);

            Assert.AreEqual(new[]
            {
                new[] {58.0f, 64.0f},
                new[] {139.0f, 154.0f}
            }, mProduct._data);
        }

        [Test]
        public void MatrixToArray()
        {
            var m = new Matrix(3, 3)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f, 3.0f},
                    new[] {4.0f, 5.0f, 6.0f},
                    new[] {7.0f, 8.0f, 9.0f}
                }
            };

            var array = m.ToArray();

            Assert.AreEqual(new[]
            {
                1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f
            }, array);
        }

        [Test]
        public void ScalarProduct()
        {
            var m = new Matrix(3, 2)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f},
                    new[] {3.0f, 4.0f},
                    new[] {5.0f, 6.0f}
                }
            };

            m.Multiply(7);

            Assert.AreEqual(new[]
            {
                new[] {7.0f, 14.0f},
                new[] {21.0f, 28.0f},
                new[] {35.0f, 42.0f}
            }, m._data);
        }

        [Test]
        public void StaticMapWithRowAndColumnParams()
        {
            var m = new Matrix(3, 3)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f, 3.0f},
                    new[] {4.0f, 5.0f, 6.0f},
                    new[] {7.0f, 8.0f, 9.0f}
                }
            };

            var mMapped = Matrix.Map(m, (val, row, col) => val * 100 + row * 10 + col);

            Assert.AreEqual(3, m._rows);
            Assert.AreEqual(3, m._cols);
            Assert.AreEqual(new[]
            {
                new[] {100.0f, 201.0f, 302.0f},
                new[] {410.0f, 511.0f, 612.0f},
                new[] {720.0f, 821.0f, 922.0f}
            }, mMapped._data);
        }

        [Test]
        public void SubtractMatrixFromOtherMatrix()
        {
            var m = new Matrix(2, 2)
            {
                _data = new[]
                {
                    new[] {10.0f, 11.0f},
                    new[] {12.0f, 13.0f}
                }
            };

            var m2 = new Matrix(2, 2)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f},
                    new[] {3.0f, 4.0f}
                }
            };

            var mMinusM2 = Matrix.Subtract(m, m2);

            Assert.AreEqual(new[]
            {
                new[] {9.0f, 9.0f},
                new[] {9.0f, 9.0f}
            }, mMinusM2._data);
        }

        [Test]
        public void TransposeMatrix1X1()
        {
            var m = new Matrix(1, 1)
            {
                _data = new[]
                {
                    new[] {1.0f}
                }
            };

            var mTranspose = Matrix.Transpose(m);

            Assert.AreEqual(new[]
            {
                new[] {1.0f}
            }, mTranspose._data);
        }

        [Test]
        public void TransposeMatrix1X5()
        {
            var m = new Matrix(5, 1)
            {
                _data = new[]
                {
                    new[] {1.0f},
                    new[] {2.0f},
                    new[] {3.0f},
                    new[] {4.0f},
                    new[] {5.0f}
                }
            };

            var mTranspose = Matrix.Transpose(m);

            Assert.AreEqual(1, mTranspose._rows);
            Assert.AreEqual(5, mTranspose._cols);
            Assert.AreEqual(new[]
            {
                new[] {1.0f, 2.0f, 3.0f, 4.0f, 5.0f}
            }, mTranspose._data);
        }

        [Test]
        public void TransposeMatrix2X3()
        {
            var m = new Matrix(3, 2)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f},
                    new[] {3.0f, 4.0f},
                    new[] {5.0f, 6.0f}
                }
            };

            var mTranspose = Matrix.Transpose(m);

            Assert.AreEqual(2, mTranspose._rows);
            Assert.AreEqual(3, mTranspose._cols);
            Assert.AreEqual(new[]
            {
                new[] {1.0f, 3.0f, 5.0f},
                new[] {2.0f, 4.0f, 6.0f}
            }, mTranspose._data);
        }

        [Test]
        public void TransposeMatrix3X2()
        {
            var m = new Matrix(2, 3)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f, 3.0f},
                    new[] {4.0f, 5.0f, 6.0f}
                }
            };

            var mTranspose = Matrix.Transpose(m);

            Assert.AreEqual(3, mTranspose._rows);
            Assert.AreEqual(2, mTranspose._cols);
            Assert.AreEqual(new[]
            {
                new[] {1.0f, 4.0f},
                new[] {2.0f, 5.0f},
                new[] {3.0f, 6.0f}
            }, mTranspose._data);
        }

        [Test]
        public void TransposeMatrix5X1()
        {
            var m = new Matrix(1, 5)
            {
                _data = new[]
                {
                    new[] {1.0f, 2.0f, 3.0f, 4.0f, 5.0f}
                }
            };

            var mTranspose = Matrix.Transpose(m);

            Assert.AreEqual(5, mTranspose._rows);
            Assert.AreEqual(1, mTranspose._cols);
            Assert.AreEqual(new[]
            {
                new[] {1.0f},
                new[] {2.0f},
                new[] {3.0f},
                new[] {4.0f},
                new[] {5.0f}
            }, mTranspose._data);
        }
    }
}