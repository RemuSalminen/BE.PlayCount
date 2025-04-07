using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace PlayCount.Data;

public class Stats
{
    public List<Opponent> Opponents { get; set; } = new List<Opponent>();
}
public class Opponent
{
    public string PlayFabID { get; set; }
    public Data Data { get; set; } = new Data();
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
        Stats stats = Read(JsonPath);

        Plugin.Log.LogInfo("Finding Opponent!");
        Opponent opponent = stats.Opponents.Find(x => x.PlayFabID == PlayFabID);
        if (opponent == null) opponent = new Opponent();
        Plugin.Log.LogInfo("Opponent: "+opponent.PlayFabID);

        Data data = opponent.Data;
        return data;
    }

    public static void Add(string JsonPath, string PlayFabID, Data Data)
    {
        Plugin.Log.LogInfo("Adding New Opponent!");
        Stats stats = Read(JsonPath);
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
        Plugin.Log.LogInfo("Editing Stats!");
        Stats stats = Read(JsonPath);

        Opponent opponent = stats.Opponents.Find(x => x.PlayFabID == PlayFabID);
        opponent.Data = NewData;

        Write(JsonPath, stats);
        Plugin.Log.LogInfo("Edited Stats!");
    }


    private static Stats Read(string JsonPath)
    {
        Plugin.Log.LogDebug("Reading Json!");
        string json = File.ReadAllText(JsonPath);
        Stats stats = JsonSerializer.Deserialize<Stats>(json);
        Plugin.Log.LogDebug("Read Json!");
        return stats;
    }
    private static void Write(string JsonPath, Stats UpdatedStats)
    {
        Plugin.Log.LogDebug("Writing Stats File!");
        string JsonString = JsonSerializer.Serialize(UpdatedStats);
        File.WriteAllText(JsonPath, JsonString);
        Plugin.Log.LogDebug("Wrote Stats File!");
    }
}

