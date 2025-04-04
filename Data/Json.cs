using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace PlayCount.Data;

public class Stats
{
    public List<Opponent> Opponents { get; set; }
}
public class Opponent
{
    public string PlayFabID { get; set; }
    public Data Data { get; set; }
}
public class Data
{
    public int Wins { get; set; }
    public int Losses { get; set; }
    public bool WonPrevious { get; set; }
}

public class Json
{
    public static Data Get(string JsonPath, string PlayFabID)
    {
        Plugin.Log.LogInfo("Fetching data from Json!");
        Stats stats = Get(JsonPath);
        //int OpponentIndex = stats.Opponents.FindIndex(x => x.PlayFabID == PlayFabID);
        Opponent opponent = stats.Opponents.Find(x => x.PlayFabID == PlayFabID);

        //Data data = stats.Opponents[OpponentIndex].Data;
        Data data = opponent.Data;
        return data;
    }

    public static void Add(string JsonPath, string PlayFabID, Data Data)
    {
        Plugin.Log.LogInfo("Adding New Opponent!");
        Stats stats = Get(JsonPath);
        Opponent opponent = new Opponent
        {
            PlayFabID = PlayFabID,
            Data = Data
        };
        stats.Opponents.Add(opponent);

        Write(JsonPath, stats);
        Plugin.Log.LogInfo("Added New Opponent!");
    }

    public static void Edit(string JsonPath, string PlayFabID, Data NewData)
    {
        Stats stats = Get(JsonPath);
        //int OpponentIndex = stats.Opponents.FindIndex(x => x.PlayFabID == PlayFabID);
        Opponent opponent = stats.Opponents.Find(x => x.PlayFabID == PlayFabID);

        //stats.Opponents[OpponentIndex].Data = NewData;
        opponent.Data = NewData;

        Write(JsonPath, stats);
    }


    private static Stats Get(string JsonPath)
    {
        string json = File.ReadAllText(JsonPath);
        Stats stats = JsonSerializer.Deserialize<Stats>(json);
        return stats;
    }
    private static void Write(string JsonPath, Stats UpdatedStats)
    {
        Plugin.Log.LogInfo("Writing Stats File!");
        string JsonString = JsonSerializer.Serialize(UpdatedStats);
        File.WriteAllText(JsonPath, JsonString);
        Plugin.Log.LogInfo("Wrote Stats File!");
    }
}

