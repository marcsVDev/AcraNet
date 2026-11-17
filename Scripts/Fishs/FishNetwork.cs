using Acra.Net;
using Godot;

namespace Acra.Fishs;

public class FishNetwork
{
    private Network network;

    private FishNetwork(Network net) 
    {
        network = net;
    }

    public (float rotation, float speed) FeedForward(Fish fish)
    {
        float nHealth = fish.health / 100;
        float nHunger = fish.hunger / 100;

        Vector2 nFood = fish.food.Normalized();
        float nFoodX = nFood.X;
        float nFoodY = nFood.Y;

        Vector2 nPreadator = fish.preadator.Normalized();
        float nPredatorX = nPreadator.X;
        float nPredatorY = nPreadator.Y;

        float[] netInput =
        [
            nFoodX, nFoodY, nPredatorX, nPredatorY
        ];
        
        float[] outNet = network.FeedForward(netInput);
        return (outNet[0], outNet[1]);
    }

    public Network CloneNetwork() => network.Clone();

    public void Randomize(float min, float max) => network.Randomize(min, max);

    public static FishNetwork Init(int inputLenght) => new FishNetwork(new Network(6, 2, inputLenght));
    public static FishNetwork Init(Network net) => new FishNetwork(net);

    public static FishNetwork InitFathertWithVar(Fish father, float minVar, float maxVar)
    {
        var fishNet = new FishNetwork(father.FishNetwork.CloneNetwork());

        fishNet.network.Randomize(minVar, maxVar);

        return fishNet;
    }

    public override string ToString()
    {
        return network.ToString();
    }
}