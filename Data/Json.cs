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
        Plugin.Log.Msg("Fetching data from Json!");
        Stats stats = Read(JsonPath);

        Plugin.Log.Msg("Finding Opponent!");
        Opponent opponent = stats.Opponents.Find(x => x.PlayFabID == PlayFabID);
        if (opponent == null) opponent = new Opponent();
        Plugin.Log.Msg("Opponent: "+opponent.PlayFabID);

        Data data = opponent.Data;
        return data;
    }

    public static void Add(string JsonPath, string PlayFabID, Data Data)
    {
        Plugin.Log.Msg("Adding New Opponent!");
        Stats stats = Read(JsonPath);
        Opponent opponent = new Opponent
        {
            PlayFabID = PlayFabID,
            Data = Data
        };
        stats.Opponents.Add(opponent);

        Write(JsonPath, stats);
        Plugin.Log.Msg("Added New Opponent!");
    }

    public static void Edit(string JsonPath, string PlayFabID, Data NewData)
    {
        Plugin.Log.Msg("Editing Stats!");
        Stats stats = Read(JsonPath);

        Opponent opponent = stats.Opponents.Find(x => x.PlayFabID == PlayFabID);
        opponent.Data = NewData;

        Write(JsonPath, stats);
        Plugin.Log.Msg("Edited Stats!");
    }


    private static Stats Read(string JsonPath)
    {
        Plugin.Log.Msg("DEBUG: Reading Json!");
        string json = File.ReadAllText(JsonPath);
        Stats stats = JsonSerializer.Deserialize<Stats>(json);
        Plugin.Log.Msg("DEBUG: Read Json!");
        return stats;
    }
    private static void Write(string JsonPath, Stats UpdatedStats)
    {
        Plugin.Log.Msg("DEBUG: Writing Stats File!");
        string JsonString = JsonSerializer.Serialize(UpdatedStats);
        File.WriteAllText(JsonPath, JsonString);
        Plugin.Log.Msg("DEBUG: Wrote Stats File!");
    }
}

