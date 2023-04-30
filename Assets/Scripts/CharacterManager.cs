using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject sonic;
    [SerializeField] private GameObject jumper;
    [SerializeField] private GameObject feesh;
    [SerializeField] private Grid grid;

    private void Awake()
    {
        var sonicPos = grid.GetCellCenterWorld((Vector3Int)SaveManager.LoadedLevel.SonicPosition);
        var jumperPos = grid.GetCellCenterWorld((Vector3Int)SaveManager.LoadedLevel.JumperPosition);
        var feeshPos = grid.GetCellCenterWorld((Vector3Int)SaveManager.LoadedLevel.FeeshPosition);

        Instantiate(sonic, sonicPos, Quaternion.identity);
        Instantiate(jumper, jumperPos, Quaternion.identity);
        Instantiate(feesh, feeshPos, Quaternion.identity);
    }
}
