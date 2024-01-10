using TMPro;
using UnityEngine;

public class EnergyLabel : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    private void Update()
    {
        label.text = $"{EnergyManager.CurrentEnergy}/{EnergyManager.MaxEnergy}";

        // if (!timerParent) return;
        //
        // if (EnergyManager.CurrentEnergy < EnergyManager.MaxEnergy)
        // {
        //     timerParent.SetActive(true);
        //     timer.text = $"{(int)EnergyManager.TimeUntilRefill / 60}:{(int)EnergyManager.TimeUntilRefill % 60:00}";
        // }
        // else
        // {
        //     timerParent.SetActive(false);
        // }
    }
}
