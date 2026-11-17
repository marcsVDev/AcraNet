
using Godot;
using Newtonsoft.Json;

namespace Acra.Net;
public class Network
{
    [JsonProperty("Hidden_Layer")]
    private Layer hiddenLayer;
    [JsonProperty("Output_Layer")]
    private Layer outputLayer;

    [JsonProperty("Hidden_Neurons")]
    private readonly int hiddenNeurons;
    [JsonProperty("Output_Neurons")]
    private readonly int outputNeurons;    

    [JsonIgnore]
    private float[] netInputs;

    public Network(int hiddenNeurons, int outputNeurons, int inputLenght)
    {
        this.hiddenNeurons = hiddenNeurons;
        this.outputNeurons = outputNeurons;

        hiddenLayer = new Layer(hiddenNeurons, inputLenght);
        outputLayer = new Layer(outputNeurons, hiddenNeurons);
    }

    [JsonConstructor]
    public Network(Layer hiddenLayer, Layer outputLayer, int hiddenNeurons, int outputNeurons) 
    {
        this.hiddenLayer = hiddenLayer;
        this.outputLayer = outputLayer;
        this.hiddenNeurons = hiddenNeurons;
        this.outputNeurons = outputNeurons;
    }

    public float[] FeedForward(float[] inputs)
    {
        netInputs = inputs;
        
        float[] hiddenOutput = hiddenLayer.FeedForward(inputs);
        
        return outputLayer.FeedForward(hiddenOutput);
    }

    public void Randomize(float min, float max)
    {
        hiddenLayer.Randomize(min, max);
        outputLayer.Randomize(min, max);
    }

    public Network Clone()
    {
        Network copy = new Network(
            hiddenLayer.Clone(), outputLayer.Clone(), 
            this.hiddenNeurons, this.outputNeurons);

        return copy;
    }

    public override string ToString()
    {
        string result = "Input: ";
        foreach (var i in netInputs)
        {
            result += i + ", ";
        }
        result += "\nHidden layer neurons:\n ";
        int count = 0;
        foreach (var h in hiddenLayer.Neurons)
        {
            result += count + ": " + h.ToString() + "\n ";
            count++;
        }
        result += "\nOutput layer neurons: \n";
        count = 0;
        foreach (var o in outputLayer.Neurons)
        {
            result += count + ": " + o.ToString() + "\n ";
            count++;
        }

        return result;
    }
}