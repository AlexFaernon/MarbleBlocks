using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{
	[SerializeField] private GameObject winMultiplayer;
	[SerializeField] private GameObject winSingleplayer;
	[SerializeField] private GameObject lose;
	public static GameObject WinMultiplayer;
	public static GameObject WinSingleplayer;
	public static GameObject Lose;

	private void Awake()
	{
		WinMultiplayer = winSingleplayer;
		WinSingleplayer = winSingleplayer;
		Lose = lose;
	}
}
