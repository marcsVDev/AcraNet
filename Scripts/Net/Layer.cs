
using Godot;
using Newtonsoft.Json;

namespace Acra.Net;
public class Layer
{
    [JsonProperty]
    public Neuron[] Neurons;

    public Layer(int numNeurons, int inputLength)
    {
        Neurons = new Neuron[numNeurons];
        for (int i = 0; i < numNeurons; i++)
        {
            Neurons[i] = new Neuron(inputLength);
        }
    }

    [JsonConstructor]
    public Layer(Neuron[] neurons) => Neurons = neurons;

    public void SetNeurons(Neuron[] neurons) => Neurons = neurons;
    public void Randomize(float min, float max)
    {
        foreach (var neuron in Neurons)
        {
            neuron.Randomize(min, max);
        }
    }

    public float[] FeedForward(float[] inputs)
    {
        float[] outputs = new float[Neurons.Length];
        for (int i = 0; i < Neurons.Length; i++)
        {
            outputs[i] = Neurons[i].Activate(inputs);
        }
        return outputs;
    }

    public Layer Clone() 
    {
        Layer copy = new Layer(Neurons);

        for (int i = 0; i < Neurons.Length; i++)
            copy.Neurons[i] = this.Neurons[i].Clone();

        return copy;
    }
}