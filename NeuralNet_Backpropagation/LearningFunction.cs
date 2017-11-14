using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet_Backpropagation
{
    public class LearningFunction
    {
        public List<double[]> Inputs = new List<double[]>();
        public List<double> Outputs = new List<double>();
        public double[] Input;
        public double Output;

        public void GetAllFunctions(Random random, int num, string symbol1, string symbol2)
        {
            for (int i = 0; i < num; i++)
            {
                var x = random.Next(0, 2) != 0;
                var y = random.Next(0, 2) != 0;
                var z = random.Next(0, 2) != 0;

                var input = new[] { x ? 1.0 : 0.0, y ? 1.0 : 0.0, z ? 1.0 : 0.0 };
                var result = GetResult(x, y, z, symbol1, symbol2);
                Inputs.Add(input);
                Outputs.Add(result);
            }
        }

        public void GetNextLearningFunction(Random random, string symbol1, string symbol2)
        {
            var x = random.Next(0, 2) != 0;
            var y = random.Next(0, 2) != 0;
            var z = random.Next(0, 2) != 0;

            Input = new[] { x ? 1.0 : 0.0, y ? 1.0 : 0.0, z ? 1.0 : 0.0 };
            Output = GetResult(x, y, z, symbol1, symbol2);
        }

        private double GetResult(bool x, bool y, bool z, string symbol1, string symbol2)
        {
            var intRes = false;
            switch (symbol1)
            {
                case "&":
                    intRes = x & y;
                    break;
                case "|":
                    intRes = x | y;
                    break;
                case "^":
                    intRes = x ^ y;
                    break;
            }
            var res = false;
            switch (symbol2)
            {
                case "&":
                    res = intRes & z;
                    break;
                case "|":
                    res = intRes | z;
                    break;
                case "^":
                    res = intRes ^ z;
                    break;
            }
            return res ? 1.0 : 0.0;
        }
    }
}
