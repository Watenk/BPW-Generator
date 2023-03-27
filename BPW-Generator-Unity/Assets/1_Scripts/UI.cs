using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : BaseClass
{
    public TextMeshProUGUI HealthText;

    public void UpdatePlayerHealth(int amount)
    {
        HealthText.SetText(amount.ToString());
    }
}