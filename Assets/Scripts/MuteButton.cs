using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    private AudioSource _audioSource;
    private Sprite _unmute;
    [SerializeField] private Sprite mute;
    [SerializeField] private Image image;

    private void Awake()
    {
        _unmute = image.sprite;
        _audioSource = GameObject.FindWithTag("EnergyManager").GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(Mute);
    }

    private void Update()
    {
        image.sprite = _audioSource.volume != 0 ? _unmute : mute;
    }

    private void Mute()
    {
        _audioSource.volume = _audioSource.volume != 0 ? 0 : 1;
    }
}
