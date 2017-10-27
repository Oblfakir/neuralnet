using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNet_Backpropagation
{
    public enum ActivationFunctions
    {
        Linear,
        Percertron,
        Sigma,
        Tanh
    }

    static class Functions
    {
        public static double Linear(double value)
        {
            return value;
        }
        public static double Percertron(double value)
        {
            if (value > 0) return 1;
            return 0;
        }
        public static double Sigma(double value)
        {
            return 1 / (1 + Math.Pow(Math.E, -value));
        }
        public static double Tanh(double value)
        {
            return (Math.Pow(Math.E, value) - Math.Pow(Math.E, -value)) / (Math.Pow(Math.E, value) + Math.Pow(Math.E, -value));
        }
    }

    public class Neuron
    {
        public List<double> Weights;
        public int InputsCount;
        public ActivationFunctions Function;
        public double Bias;

        public Neuron(int inputsCount, ActivationFunctions function)
        {
            InputsCount = inputsCount;
            Weights = new List<double>(inputsCount);
            Function = function;
            //Initialize();
        }

        private void Initialize()
        {
            var random = new Random();
            Bias = random.NextDouble() - 0.5;
            for (int i = 0; i < Weights.Count; i++)
            {
                Weights[i] = random.NextDouble() - 0.5;
            }
        }

        public void Learn(double[] deltas)
        {
            if (deltas.Length != InputsCount) return;
            for (int i = 0; i < deltas.Length; i++)
            {
                Weights[i] = Weights[i] + deltas[i];
            }
        }

        public double GetAnswer(double[] input)
        {
            if (input.Length != Weights.Count || input.Length != InputsCount) return 0;
            var sum = GetSum(input) + Bias;
            var result = GetResultFromActivationFunction(sum);
            return result;
        }

        private double GetSum(double[] values)
        {
            return Weights.Select((t, i) => t * values[i]).Sum();
        }

        private double GetResultFromActivationFunction(double value)
        {
            if (Function == ActivationFunctions.Linear)
            {
                return Functions.Linear(value);
            }
            if (Function == ActivationFunctions.Percertron)
            {
                return Functions.Percertron(value);
            }
            if (Function == ActivationFunctions.Sigma)
            {
                return Functions.Sigma(value);
            }
            if (Function == ActivationFunctions.Tanh)
            {
                return Functions.Tanh(value);
            }
            return 0;
        }

        public void Save()
        {

        }
    }
}
