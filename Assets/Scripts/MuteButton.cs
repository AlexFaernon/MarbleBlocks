using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    private AudioSource _audioSource;
    private Sprite _unmute;
    [SerializeField] private Sprite mute;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _unmute = _image.sprite;
        _audioSource = GameObject.FindWithTag("EnergyManager").GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(Mute);
    }

    private void Update()
    {
        _image.sprite = _audioSource.volume != 0 ? _unmute : mute;
    }

    private void Mute()
    {
        _audioSource.volume = _audioSource.volume != 0 ? 0 : 1;
    }
}
