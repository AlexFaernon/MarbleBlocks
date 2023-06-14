using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyHelpButton : MonoBehaviour
{
	[SerializeField] private GameObject buyLabel;
	[SerializeField] private GameObject noBuyLabel;
	[SerializeField] private HelpManager helpManager;
	
	private Button _button;
	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(BuyHelp);
	}

	private void Update()
	{
		if (NameManager.PlayerName.ToUpper() == "TRW")
		{
			buyLabel.SetActive(true);
			_button.interactable = true;
			return;
		}
		
		buyLabel.SetActive(CoinsManager.Coins > 0);
		noBuyLabel.SetActive(CoinsManager.Coins == 0);
		_button.interactable = CoinsManager.Coins > 0;
	}

	private void BuyHelp()
	{
		transform.parent.gameObject.SetActive(false);
		helpManager.BuyHelp();
		helpManager.GetComponent<Toggle>().isOn = true;
		helpManager.gameObject.SetActive(true);
	}
}
