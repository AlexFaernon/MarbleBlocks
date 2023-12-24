using Newtonsoft.Json;
using UnityEngine;
using File = System.IO.File;
using Firebase.Database;
using System;

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
        else if (GameMode.CurrentGameMode == GameModeType.SinglePlayer)
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
            FeeshPosition  = feesh ? feesh.GridPosition : Vector2Int.zero,
            JumperPosition = jumper ? jumper.GridPosition : Vector2Int.zero,
            SonicPosition  = sonic ? sonic.GridPosition : Vector2Int.zero,
        };
        LoadedLevel = levelSave;
        //File.WriteAllText(Application.persistentDataPath + "\\save.json", JsonConvert.SerializeObject(LoadedLevel));
        RealtimeDatabase.PushMap(LoadedLevel, false);
    }

    public void SaveLevelLocal()
    {
        LoadedLevel.OptimalTurns = StepCounter.Count;
        LoadedLevel.HelpClass = WriteHelpInEditor.GetHelp();
        File.WriteAllText(Application.persistentDataPath + $"\\save {DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year} {DateTime.Now.Hour}-{DateTime.Now.Minute}.json", JsonConvert.SerializeObject(LoadedLevel));
    }
}