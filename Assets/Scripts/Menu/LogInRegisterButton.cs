using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInRegisterButton : MonoBehaviour
{
	private Button _button;
	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(() => _button.interactable = false);
	}

	private void OnEnable()
	{
		_button.interactable = true;
	}
}
