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

        public LearningFunction(Random random)
        {
            for (int i = 0; i < 10000; i++)
            {
                var x = random.Next(0, 2) != 0;
                var y = random.Next(0, 2) != 0;
                var z = random.Next(0, 2) != 0;

                var input = new[] { x ? 1.0 : 0.0, y ? 1.0 : 0.0, z ? 1.0 : 0.0 };
                var result = x | y & z ? 1.0 : 0.0;
                Inputs.Add(input);
                Outputs.Add(result);
            }
        }
    }
}
