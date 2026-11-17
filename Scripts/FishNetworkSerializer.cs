using Acra.Net;
using System.IO;
using Newtonsoft.Json;
using Acra.Fishs;

namespace Acra.Scripts;

class FishNetworkSerializer
{
    public static void SerializeWriter(FishNetwork fNetwork, string path = "Json/net.json") 
    {
        if (!File.Exists(path))
            throw new FileNotFoundException();

        Network cloneNet = fNetwork.CloneNetwork();
        string netSer = JsonConvert.SerializeObject(cloneNet, Formatting.Indented);

        using StreamWriter sw = new StreamWriter(path);
        sw.Write(netSer);
    }

    public static FishNetwork DeserializeFromFile(string path = "Json/net.json") 
    {
        if (!File.Exists(path))
            throw new FileNotFoundException();

        Network net;
        using (StreamReader sr = new(path))
        {
            net = JsonConvert.DeserializeObject<Network>(sr.ReadToEnd());
        }

        return FishNetwork.Init(net);
    }
}
