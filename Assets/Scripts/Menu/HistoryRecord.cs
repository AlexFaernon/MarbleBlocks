using TMPro;
using UnityEngine;

public class HistoryRecord : MonoBehaviour
{
	[SerializeField] private TMP_Text playerName;
	[SerializeField] private TMP_Text stepCount;
	[SerializeField] private TMP_Text rating;
	[SerializeField] private TMP_Text result;

	public void SetRecord(string player, int steps, bool isWin)
	{
		playerName.text = player;
		stepCount.text = steps.ToString();
		rating.text = isWin ? "-1" : "+1";
		result.text = isWin ? "Пройден" : "Не пройден";
	}
}