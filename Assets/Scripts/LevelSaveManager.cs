using Newtonsoft.Json;
using UnityEngine;
using File = System.IO.File;
using Firebase.Database;
public class LevelSaveManager : MonoBehaviour
{
    public static int LevelNumber;
    public static LevelClass LoadedLevel;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private bool isEditor;

    private void Awake()
    {
        if (isEditor) return;
        
        if (LevelNumber == 0) return;
        var levelJson = Resources.Load<TextAsset>(LevelNumber.ToString()).text;
        LoadedLevel = JsonConvert.DeserializeObject<LevelClass>(levelJson);
    }

    public void SaveLevel()
    {
        var sonic = CharacterManager.Sonic;
        var jumper = CharacterManager.Jumper;
        var feesh = CharacterManager.Feesh;
        var levelSave = new LevelClass
        {
            FieldSize      = TileManager.fieldSize,
            Tiles          = tileManager.GetSave(),
            FeeshPosition  = feesh ? feesh.GetGridPosition : Vector2Int.zero,
            JumperPosition = jumper ? jumper.GetGridPosition : Vector2Int.zero,
            SonicPosition  = sonic ? sonic.GetGridPosition : Vector2Int.zero
        };
        LoadedLevel = levelSave;
        var save = JsonConvert.SerializeObject(levelSave);
        File.WriteAllText(Application.persistentDataPath + "\\save.json", save);
    }
}