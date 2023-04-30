using Newtonsoft.Json;
using UnityEngine;
using File = System.IO.File;

public class SaveManager : MonoBehaviour
{
    public static int LevelNumber;
    public static Level LoadedLevel;
    [SerializeField] private TileManager tileManager;

    private void Awake()
    {
        var levelJson = Resources.Load<TextAsset>(LevelNumber.ToString()).text;
        LoadedLevel = JsonConvert.DeserializeObject<Level>(levelJson);
    }

    public void SaveLevel()
    {
        var levelSave = new Level
        {
            FieldSize = tileManager.fieldSize,
            Tiles = tileManager.GetSave(),
            FeeshPosition = GameObject.FindGameObjectWithTag("Feesh").GetComponent<Feesh>().GetSave,
            JumperPosition = GameObject.FindGameObjectWithTag("Jumper").GetComponent<Jumper>().GetSave,
            SonicPosition = GameObject.FindGameObjectWithTag("Sonic").GetComponent<Sonic>().GetSave,
        };
        var save = JsonConvert.SerializeObject(levelSave);
        File.WriteAllText(Application.persistentDataPath + "\\save.json", save);
    }
}