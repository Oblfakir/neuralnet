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
            const int number = 12000;
            const int inputsNum = 3;
            const int k = 60;
            const double learningRate = 0.5;
            const string symbol1 = "^";
            const string symbol2 = "^";

            var random = new Random();

            var learn = new LearningFunction(random, number, symbol1, symbol2);
            var nnet = new NeuralNet(inputsNum, k, learningRate, random);

            for (int i = 0; i < learn.Inputs.Count; i++)
            {
                var num = $"i: {i} ";
                var input = $" x:{learn.Inputs[i][0]} y:{learn.Inputs[i][1]} z:{learn.Inputs[i][2]}";
                var target = @" target " + learn.Outputs[i];
                var a = nnet.Learn(learn.Inputs[i], learn.Outputs[i]);
                var result = @" result " + a.ToString("##0.####");
                var delta = $"delta {((learn.Outputs[i] - a) * 100).ToString("##0.####")}%";
                Console.WriteLine($"{num} {input} {target} {result} {delta}");
            }
        }
    }
}
