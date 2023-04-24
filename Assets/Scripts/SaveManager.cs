using Newtonsoft.Json;
using UnityEngine;
using File = System.IO.File;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;

    public void SaveLevel()
    {
        var save = JsonConvert.SerializeObject(tileManager.GetSave());
        File.WriteAllText(Application.persistentDataPath + "\\save.json", save);
    }

    public static TileClass[,] LoadLevel()
    {
        var file = File.ReadAllText(Application.persistentDataPath + "\\save.json");
        return JsonConvert.DeserializeObject<TileClass[,]>(file);
    }
}