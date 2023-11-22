using UnityEngine;

public class Exit : MonoBehaviour
{
    public static int Count;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Feesh") || col.CompareTag("Sonic") || col.CompareTag("Jumper"))
        {
            col.gameObject.SetActive(false);
            GameObject.FindWithTag("Finish").transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    
    private void OnEnable()
    {
        Count++;
    }

    private void OnDisable()
    {
        Count--;
    }
}
