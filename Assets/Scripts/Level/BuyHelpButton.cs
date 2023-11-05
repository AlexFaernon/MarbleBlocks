using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuyHelpButton : MonoBehaviour
{
	[SerializeField] private GameObject buyLabel;
	[SerializeField] private GameObject noBuyLabel;
	[FormerlySerializedAs("helpManager")]
	[SerializeField] private HelpSwitch helpSwitch;
	
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
		helpSwitch.BuyHelp();
		helpSwitch.GetComponent<Toggle>().isOn = true;
		helpSwitch.gameObject.SetActive(true);
	}
}
