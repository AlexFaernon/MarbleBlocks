using UnityEngine;

public class Spike : MonoBehaviour
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
