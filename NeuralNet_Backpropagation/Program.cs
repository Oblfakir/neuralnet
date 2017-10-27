using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet_Backpropagation
{
    class Program
    {
        static void Main(string[] args)
        {
            int InputsNum = 3;
            const int k = 60;
            var random = new Random();

            var learn = new LearningFunction(random);
            var nnet = new NeuralNet(InputsNum, k, random);

            for (int i = 0; i < learn.Inputs.Count; i++)
            {
                var num = $"i: {i} ";
                var input = $" x:{learn.Inputs[i][0]} y:{learn.Inputs[i][1]} z:{learn.Inputs[i][2]}";
                var target = @" target " + learn.Outputs[i];
                var a = nnet.Learn(learn.Inputs[i], learn.Outputs[i]);
                var result = @" result " + a;
                var delta = $"delta {((learn.Outputs[i] - a) * 100)}%";
                Console.WriteLine($"{num} {input} {target} {result} {delta}");
            }
        }
    }
}
