using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject sonic;
    [SerializeField] private GameObject jumper;
    [SerializeField] private GameObject feesh;
    [SerializeField] private Grid grid;

    private void Awake()
    {
        if (LevelSaveManager.LevelNumber == 0) return;
        
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
}
