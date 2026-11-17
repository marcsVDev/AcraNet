using Godot;
using Newtonsoft.Json;
using System;

namespace Acra.Net;
public class Neuron
{
    [JsonIgnore]
    private static Random random = new Random();

    [JsonProperty]
    public float[] Weights;
    [JsonProperty]
    public float Bias;

    public Neuron(int inputLength)
    {
        Weights = new float[inputLength];
        for (int i = 0; i < inputLength; i++)
        {
            float rValue = (float)random.NextDouble() * (2 - -1) + -1; // 1 .. -1
            Weights[i] = rValue;
        }
        Bias = (float)random.NextDouble() * (2 - -1) + -1;
    }

    [JsonConstructor]
    public Neuron(float[] weights, float bias)
    {
        this.Weights = weights;
        this.Bias = bias;
    }

    public void SetNeuron(float[] weights, float bias) 
    {
        this.Weights = weights;
        this.Bias = bias;
    }
    public void Randomize(float min, float max)
    {
        for(int i = 0; i < Weights.Length; i++)
        {
            Weights[i] += (float)random.NextDouble() * (max - min) + min;
        }
        Bias += (float)random.NextDouble() * (max - min) + min;
    }

    public Neuron Clone()
    {
        Neuron copy = new Neuron(Weights, Bias);
        Weights.CopyTo(copy.Weights, 0);
        copy.Bias = Bias;
        return copy;
    }


    public float Activate(float[] inputs)
    {
        float sum = 0;
        for (int i = 0; i < inputs.Length; i++)
        {
            sum += inputs[i] * Weights[i];
        }
        sum += Bias;

        return Relu(sum);
    }
    private float Relu(float x)
    {
        if (x <= 0)
            return 0;
        else
            return x;
    }

    public override string ToString()
    {
        string result = "Weight: ";
        foreach (float w in Weights)
        {
            result += w + ", ";
        }
        result += ";\n    Bia: " + Bias;

        return result;
    }
}
