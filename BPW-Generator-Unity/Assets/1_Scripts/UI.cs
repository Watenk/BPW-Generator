using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : BaseClass
{
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI CrystalText;
    public TextMeshProUGUI TurnText;

    public void UpdatePlayerHealth(int amount)
    {
        HealthText.SetText(amount.ToString());
    }

    public void UpdateCrystalAmount(int collectedCrystals, int totalCrystals)
    {
        CrystalText.SetText(collectedCrystals.ToString() + " / " + totalCrystals.ToString());
    }

    public void UpdateCurrentTurn(int currentTurn)
    {
        TurnText.SetText("Turn: " + currentTurn.ToString());
    }
}