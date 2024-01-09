using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInRegisterButton : MonoBehaviour
{
	private void Awake()
	{
		var button = GetComponent<Button>();
		button.onClick.AddListener(() => button.interactable = false);
	}
}
