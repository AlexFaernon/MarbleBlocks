using Newtonsoft.Json;
using UnityEngine;
using File = System.IO.File;

public class LevelSaveManager : MonoBehaviour
{
    [SerializeField] private Vector2Int feesh;
    [SerializeField] private Vector2Int jumper;
    [SerializeField] private Vector2Int sonic;
    public static int LevelNumber;
    public static LevelClass LoadedLevel;
    [SerializeField] private TileManager tileManager;

    private void Awake()
    {
        if (LevelNumber == 0) return;
        var levelJson = Resources.Load<TextAsset>(LevelNumber.ToString()).text;
        LoadedLevel = JsonConvert.DeserializeObject<LevelClass>(levelJson);
    }

    public void SaveLevel()
    {
        var levelSave = new LevelClass
        {
            FieldSize      = tileManager.fieldSize,
            Tiles          = tileManager.GetSave(),
            FeeshPosition  = feesh,
            JumperPosition = jumper,
            SonicPosition  = sonic
        };
        var save = JsonConvert.SerializeObject(levelSave);
        File.WriteAllText(Application.persistentDataPath + "\\save.json", save);
    }
}