using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rpg;
using System.IO;

public enum SaveKey
{
    facebookDone,
    seenBull,
    seenSnake,
    metAltea,
    metOrion,
    seenMontgeron,
    seenHouse,
    defeatedCerberus
}

public class GameDataDebug : MonoBehaviour
{
    public string place = null;
    public bool facebookDone = false;
    public bool seenBull = false;
    public int seenSnake = 0; // -1 before, 0 unseen, 1 after
    public bool metAltea = false;
    public bool metOrion = false;
    public bool seenMontgeron = false;
    public bool seenHouse = false;
    public bool defeatedCerberus = false;

    public GameData GetData()
    {
        GameData data = new GameData();

        data.place = place;
        data.SetBool(SaveKey.facebookDone, facebookDone);
        data.SetBool(SaveKey.seenBull, seenBull);
        data.SetKey(SaveKey.seenSnake, seenSnake);
        data.SetBool(SaveKey.metAltea, metAltea);
        data.SetBool(SaveKey.metOrion, metOrion);
        data.SetBool(SaveKey.seenMontgeron, seenMontgeron);
        data.SetBool(SaveKey.seenHouse, seenHouse);
        data.SetBool(SaveKey.defeatedCerberus, defeatedCerberus);

        return data;
    }

    public void SetData(GameData data)
    {
        place = data.place;
        facebookDone = data.GetBool(SaveKey.facebookDone);
        seenBull = data.GetBool(SaveKey.seenBull);
        seenSnake = data.GetKey(SaveKey.seenSnake);
        metAltea = data.GetBool(SaveKey.metAltea);
        metOrion = data.GetBool(SaveKey.metOrion);
        seenMontgeron = data.GetBool(SaveKey.seenMontgeron);
        seenHouse = data.GetBool(SaveKey.seenHouse);
        defeatedCerberus = data.GetBool(SaveKey.defeatedCerberus);
    }

}

[System.Serializable]
public class GameData
{
    private static string SAVE_PATH = Application.dataPath + "/save.json";
    //private static string SAVE_PATH = Application.persistentDataPath + "/save.json";

    public string place = null;
    public Dictionary<SaveKey, int> data = new Dictionary<SaveKey, int>();
    [SerializeField]
    public List<GameDataKey> savedata = new List<GameDataKey>();

    public void SetKey(SaveKey key, int value)
    {
        data[key] = value;
    }
    public void SetBool(SaveKey key, bool value)
    {
        data[key] = value ? 1 : 0;
    }
    public int GetKey(SaveKey key)
    {
        int value = 0;
        data.TryGetValue(key, out value);
        return value;
    }
    public bool GetBool(SaveKey key)
    {
        return GetKey(key) == 1;
    }

    public static void SaveToFile(GameData gameData)
    {
        gameData.savedata = new List<GameDataKey>(gameData.data.Count);
        foreach (var keyvalue in gameData.data)
        {
            gameData.savedata.Add(new GameDataKey(keyvalue));
        }

        string saveText = JsonUtility.ToJson(gameData, true);
        Debug.Log("SAVE to " + SAVE_PATH + "\n" + saveText);
        File.WriteAllText(SAVE_PATH, saveText);
    }
    public static GameData LoadFromFile()
    {
        string saveText = File.ReadAllText(SAVE_PATH);
        Debug.Log("LOAD from " + SAVE_PATH + "\n" + saveText);
        GameData loadedData = JsonUtility.FromJson<GameData>(saveText);

        loadedData.data = new Dictionary<SaveKey, int>(loadedData.savedata.Count);
        foreach (var gamedatakey in loadedData.savedata)
        {
            loadedData.data[gamedatakey.key] = gamedatakey.value;
        }

        return loadedData;
    }
}

[System.Serializable]
public class GameDataKey
{
    public SaveKey key;
    public int value;

    public GameDataKey(SaveKey k, int v)
    {
        key = k;
        value = v;
    }

    public GameDataKey(KeyValuePair<SaveKey, int> pair)
    {
        key = pair.Key;
        value = pair.Value;
    }

    public KeyValuePair<SaveKey, int> Pair()
    {
        return new KeyValuePair<SaveKey, int>(key, value);
    }
}
