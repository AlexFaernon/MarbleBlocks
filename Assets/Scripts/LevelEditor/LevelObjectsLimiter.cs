using UnityEngine;
using Button = UnityEngine.UI.Button;

public class LevelObjectsLimiter : MonoBehaviour
{
    [SerializeField] private Button sonicButton;
    [SerializeField] private Button jumperButton;
    [SerializeField] private Button feeshButton;
    public static int SonicCount;
    public static int JumperCount;
    public static int FeeshCount;

    private void Update()
    {
        jumperButton.interactable = JumperCount < 1;
    }
}
