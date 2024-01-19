using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuyHelpButton : MonoBehaviour
{
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
		if (PlayerData.Name.ToUpper() == "TRW")
		{
			_button.interactable = true;
			return;
		}
		
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
