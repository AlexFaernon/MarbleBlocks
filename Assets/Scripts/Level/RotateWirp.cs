using UnityEngine;
using Random = UnityEngine.Random;

public class RotateWirp : MonoBehaviour
{
    private float _rotate;
    private void Awake()
    {
        _rotate = Random.Range(-0.1f, 0.1f);
        if (_rotate >= 0)
        {
            _rotate += 0.1f;
        }
        else
        {
            _rotate -= 0.1f;
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, _rotate);
    }
}
