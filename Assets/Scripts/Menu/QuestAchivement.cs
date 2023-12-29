using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestAchivement : MonoBehaviour
{
	[SerializeField] private int coinsReward;
	[SerializeField] private int expReward;
	public Image progressBar;
	[SerializeField] private TMP_Text progressLabel;
	public Button claimButton;
	[SerializeField] private Image check;
	public TMP_Text label;
	public int progress;
	public int target;

	private void Awake()
	{
		claimButton.onClick.AddListener(ClaimReward);
	}

	private void Update()
	{
		progressBar.fillAmount = (float)progress / target;
		progressLabel.text = $"{progress}/{target}";

		if (progress >= target)
		{
			claimButton.gameObject.SetActive(true);
		}

		label.text = claimButton.interactable ? "Получить" : "Получено";
		check.gameObject.SetActive(!claimButton.interactable);
	}

	private void ClaimReward()
	{
		CoinsManager.Coins += coinsReward;
		ExpLevelManager.Exp += expReward;
	}
}