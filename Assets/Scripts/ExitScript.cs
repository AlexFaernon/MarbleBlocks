using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Feesh") || col.CompareTag("Sonic") || col.CompareTag("Jumper"))
        {
            Destroy(col.gameObject);
            GameObject.FindWithTag("Finish").transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
