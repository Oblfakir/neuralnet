using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet_Backpropagation
{
    public class Interaction1C
    {
        private int inputsNum = 3;
        private int k = 60;
        private double _learningRate = 0.5;
        private string _symbol1 = "^";
        private string _symbol2 = "^";
        private Random _random;
        private LearningFunction _learn;
        private NeuralNet _neuralnet;

        public void Initialize(string initString)
        {
            // {k}###{learningRate}###{symbol1}###{symbol2}
            var arr = initString.Split(new[] {"###"}, StringSplitOptions.None);
            k = int.Parse(arr[0]);
            _learningRate = double.Parse(arr[1]);
            _symbol1 = arr[2];
            _symbol2 = arr[3];

            _random = new Random();
            _learn = new LearningFunction();
            _neuralnet = new NeuralNet(inputsNum, k, _learningRate, _random);
        }
        
        public string GetNextResult(int i)
        {
            _learn.GetNextLearningFunction(_random, _symbol1, _symbol2);
            var result = new Result(i, _learn.Input, _learn.Output, _neuralnet.Learn(_learn.Input, _learn.Output));
            result.Write();
            return result.Serialize();
        }
    }
}
