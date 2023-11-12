using UnityEngine;

public class Whirlpool : MonoBehaviour
{
    public static int Count;

    private void OnEnable()
    {
        Count++;
    }

    private void OnDisable()
    {
        Count--;
    }
}
