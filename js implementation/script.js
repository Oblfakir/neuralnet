let LEARNING_RATE = 0.5;
let NEURONS_PER_LAYER = 60;
const INPUTS_COUNT = 3;

class Neuron {
    constructor(inputsCount, bias) {
        this.weights = [];
        this.inputsCount = inputsCount;
        this.bias = bias;
        this.initialize();
    }
    initialize() {
        for (let k = 0; k < this.inputsCount; k++)
        {
            const weight = Math.random() * 2 - 1;
            this.weights.push(weight);
        }
    }
    getAnswer(input) {
        if (input.length != this.weights.length || input.length != this.inputsCount) return;
        const sum = this.weights.reduce((s, w, i) => s + w * input[i], 0) + this.bias;
        return 1 / (1 + Math.pow(Math.E, - sum));
    }
}

class NeuralNet {
    constructor() {
        this.layer1 = [];
        this.layer2 = [];
        this.outputNeuron = null;

        this.buildNet();
    }
    buildNet() {
        for (let j = 0; j < NEURONS_PER_LAYER; j++)
        {
            this.layer1.push(new Neuron(INPUTS_COUNT, 0));
            this.layer2.push(new Neuron(NEURONS_PER_LAYER, 0));
        }
        this.outputNeuron = new Neuron(NEURONS_PER_LAYER, 0);
    }
    learn(input, target) {
        var layer1Answers = [];
        for (let j = 0; j < NEURONS_PER_LAYER; j++)
        {
            layer1Answers.push(this.layer1[j].getAnswer(input));
        }

        var layer2Answers = [];
        for (let j = 0; j < NEURONS_PER_LAYER; j++)
        {
            layer2Answers.push(this.layer2[j].getAnswer(layer1Answers));
        }

        var outf = this.outputNeuron.getAnswer(layer2Answers);

        var deByDnet = (outf - target) * outf * (1 - outf);

        for (let j = 0; j < NEURONS_PER_LAYER; j++)
        {
            this.outputNeuron.weights[j] = this.outputNeuron.weights[j] - LEARNING_RATE * deByDnet * layer2Answers[j];
        }

        for (let j = 0; j < NEURONS_PER_LAYER; j++)
        {
            for (let m = 0; m < NEURONS_PER_LAYER; m++)
            {
                this.layer2[j].weights[m] = this.layer2[j].weights[m] -
                                        LEARNING_RATE * deByDnet * this.outputNeuron.weights[j] * layer2Answers[j] *
                                        (1 - layer2Answers[j]) * layer1Answers[m];
            }
        }

        for (let j = 0; j < NEURONS_PER_LAYER; j++)
        {
            for (let m = 0; m < INPUTS_COUNT; m++)
            {
                var sum = 0.0;
                for (let k = 0; k < NEURONS_PER_LAYER; k++)
                {
                    sum += this.outputNeuron.weights[k] * layer2Answers[k] * (1 - layer2Answers[k]) * this.layer2[k].weights[j];
                }
                this.layer1[j].weights[m] = this.layer1[j].weights[m] -
                                        LEARNING_RATE * sum * deByDnet * layer1Answers[j] * (1 - layer1Answers[j]) *
                                        input[m];
            }
        }

        return outf;
    }
}

window.onload = () => {
    document.getElementById('learningRate').value = 0.5;
    document.getElementById('neuronsNum').value = 10;
    document.getElementById('examplesNum').value = 1000;
    document.getElementById('function').value = 'x & y ^ z';
}

document.getElementById('start').onclick = () => {
    const learningRate = document.getElementById('learningRate');
    const neuronsNum = document.getElementById('neuronsNum');
    const examplesNum = document.getElementById('examplesNum');
    const func = document.getElementById('function');

    LEARNING_RATE = +learningRate.value;
    NEURONS_PER_LAYER = +neuronsNum.value;

    const results = [];
    const nn = new NeuralNet();
    for (let i = 0; i < +examplesNum.value; i++)
    {
        var x = getNextInt(2) != 0;
        var y = getNextInt(2) != 0;
        var z = getNextInt(2) != 0;

        var input = [x ? 1.0 : 0.0, y ? 1.0 : 0.0, z ? 1.0 : 0.0 ];
        var result = eval(func.value+ ' ? 1.0 : 0.0');
        results.push({output: nn.learn(input, result), expected: result});
    }
    outputResults(results);
}

function getNextInt(n) {
    return Math.floor(Math.random()*n)
}

function outputResults(arr){
    var root = document.getElementById('root');
    for (let i = 0; i < root.children.length; i++) {
        root.children[i].remove();
    }
    const container = document.createElement('div');
    for (let i = 0; i < arr.length; i++) {
        const element = document.createElement('div');
        const expected = document.createElement('span');
        expected.innerText = `Expected output: ${arr[i].expected}`;
        const output = document.createElement('span');
        output.innerText = `   Net output: ${arr[i].output}`;
        output.className = Math.abs(arr[i].expected - arr[i].output) < 0.2 ? 'green' : 'red' ;
        element.appendChild(expected);
        element.appendChild(output);
        container.appendChild(element);
    }
    root.appendChild(container);
}