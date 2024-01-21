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
	[SerializeField] private Sprite done;
	[SerializeField] private Sprite claimed;
	[SerializeField] private Sprite tickDone;
	[SerializeField] private Sprite tickClaimed;
	[SerializeField] private Sprite checkWhite;
	public TMP_Text label;
	public int progress;
	public int target;
	private Image _image;
	private Image _tickImage;

	private void Awake()
	{
		claimButton.onClick.AddListener(ClaimReward);
		_image = GetComponent<Image>();
		_tickImage = check.transform.parent.GetComponent<Image>();
	}

	private void Update()
	{
		progressBar.fillAmount = (float)progress / target;
		progressLabel.text = $"{progress}/{target}";

		if (progress >= target)
		{
			claimButton.gameObject.SetActive(true);
			check.gameObject.SetActive(true);
			progressBar.transform.parent.gameObject.SetActive(false);
			_image.sprite = claimButton.interactable ? done : claimed;
			_tickImage.sprite = claimButton.interactable ? tickDone : tickClaimed;
		}

		label.text = claimButton.interactable ? "Получить" : "Получено";
		
		if (!claimButton.interactable)
		{
			check.sprite = checkWhite;
		}
	}

	private void ClaimReward()
	{
		CoinsManager.Coins += coinsReward;
		ExpLevelManager.Exp += expReward;
	}
}