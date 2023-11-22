using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject sonic;
    [SerializeField] private GameObject jumper;
    [SerializeField] private GameObject feesh;
    [SerializeField] private Grid grid;
    public static Sonic Sonic;
    public static Jumper Jumper;
    public static Feesh Feesh;

    private void Awake()
    {
        if (LevelSaveManager.LoadedLevel is null) return;
        
        if (LevelSaveManager.LoadedLevel.SonicPosition != Vector2Int.zero)
        {
            var sonicPos = grid.GetCellCenterWorld((Vector3Int)LevelSaveManager.LoadedLevel.SonicPosition);
            Instantiate(sonic, sonicPos, Quaternion.identity);
        }
        
        if (LevelSaveManager.LoadedLevel.JumperPosition != Vector2Int.zero)
        {
            var jumperPos = grid.GetCellCenterWorld((Vector3Int)LevelSaveManager.LoadedLevel.JumperPosition);
            Instantiate(jumper, jumperPos, Quaternion.identity);
        }
        
        if (LevelSaveManager.LoadedLevel.FeeshPosition != Vector2Int.zero)
        {
            var feeshPos = grid.GetCellCenterWorld((Vector3Int)LevelSaveManager.LoadedLevel.FeeshPosition);
            Instantiate(feesh, feeshPos, Quaternion.identity);
        }
    }

    public void ResetCharacters()
    {
        if (Sonic)
        {
            Destroy(Sonic.gameObject);
            var sonicPos = grid.GetCellCenterWorld((Vector3Int)LevelSaveManager.LoadedLevel.SonicPosition);
            Instantiate(sonic, sonicPos, Quaternion.identity);
        }
        
        if (Jumper)
        {
            Destroy(Jumper.gameObject);
            var jumperPos = grid.GetCellCenterWorld((Vector3Int)LevelSaveManager.LoadedLevel.JumperPosition);
            Instantiate(jumper, jumperPos, Quaternion.identity);
        }

        if (Feesh)
        {
            Destroy(Feesh.gameObject);
            var feeshPos = grid.GetCellCenterWorld((Vector3Int)LevelSaveManager.LoadedLevel.FeeshPosition);
            Instantiate(feesh, feeshPos, Quaternion.identity);
        }
    }
}
