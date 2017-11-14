using System;

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

            for (int i = 0; i < number; i++)
            {
                var result = new Result(i, learn.Inputs[i], learn.Outputs[i], nnet.Learn(learn.Inputs[i], learn.Outputs[i]));
                result.Write();
            }
        }
    }
}
