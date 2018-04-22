using System;

namespace ToyNN
{
    public class ActivationFunction
    {
        public Func<float, int, int, float> Dfunc;
        public Func<float, int, int, double> Func;

        public ActivationFunction(Func<float, int, int, double> func, Func<float, int, int, float> dfunc)
        {
            Func = func;
            Dfunc = dfunc;
        }
    }
}