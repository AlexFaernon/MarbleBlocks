using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryRecord : MonoBehaviour
{
	[SerializeField] private TMP_Text playerName;
	[SerializeField] private TMP_Text stepCount;
	[SerializeField] private TMP_Text rating;
	[SerializeField] private TMP_Text result;
	[SerializeField] private Image rankIcon;
	[SerializeField] private Sprite loseSprite;

	public void SetRecord(string player, int steps, bool isWin, Sprite rankSprite)
	{
		playerName.text = player;
		stepCount.text = steps.ToString();
		rating.text = isWin ? "-1" : "+1";
		result.text = isWin ? "Пройден" : "Не пройден";
		rankIcon.sprite = rankSprite;
		if (!isWin)
		{
			GetComponentInChildren<Image>().sprite = loseSprite;
		}
	}
}