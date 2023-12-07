using Newtonsoft.Json;
using UnityEngine;
using File = System.IO.File;
using Firebase.Database;
public class LevelSaveManager : MonoBehaviour
{
    public static int LevelNumber;
    public static LevelClass LoadedLevel;
    public static bool LoadLevelTaskCompleted => RealtimeDatabase.LevelLoaded;
    [SerializeField] private TileManager tileManager;

    private void Awake()
    {
        if (GameMode.CurrentGameMode == GameModeType.LevelEditor)
        {
            StartCoroutine(RealtimeDatabase.ExportEditorLevel());
        }
        else
        {
            var levelJson = Resources.Load<TextAsset>(LevelNumber.ToString()).text;
            LoadedLevel = JsonConvert.DeserializeObject<LevelClass>(levelJson);
        }
    }

    public void SaveLevel()
    {
        var sonic = CharacterManager.Sonic;
        var jumper = CharacterManager.Jumper;
        var feesh = CharacterManager.Feesh;
        var levelSave = new LevelClass
        {
            FieldSize      = TileManager.EditorFieldSize,
            Tiles          = tileManager.GetSave(),
            FeeshPosition  = feesh ? feesh.GetGridPosition : Vector2Int.zero,
            JumperPosition = jumper ? jumper.GetGridPosition : Vector2Int.zero,
            SonicPosition  = sonic ? sonic.GetGridPosition : Vector2Int.zero
        };
        LoadedLevel = levelSave;
        RealtimeDatabase.PushMap(LoadedLevel, false);
    }
}