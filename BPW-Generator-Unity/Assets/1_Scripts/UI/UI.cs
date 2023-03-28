using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : BaseClass
{
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI CrystalText;
    public TextMeshProUGUI TurnText;
    public TextMeshProUGUI HealthPotionText;

    private int totalCrystals;

    private EventManager eventManager;

    public override void OnAwake()
    {
        eventManager = FindObjectOfType<EventManager>();
    }

    public void UpdatePlayerHealth(int amount)
    {
        HealthText.SetText(amount.ToString());
    }

    public void UpdateTotalCrystals(int amount)
    {
        totalCrystals = amount;
        CrystalText.SetText(0 + " / " + totalCrystals.ToString());
    }

    public void UpdateInventoryUI(Dictionary<objectID, int> inventory)
    {
        if (inventory.ContainsKey(objectID.crystal))
        {
            CrystalText.SetText(inventory[objectID.crystal].ToString() + " / " + totalCrystals.ToString());
        }
        if (inventory.ContainsKey(objectID.healthPotion))
        {
            HealthPotionText.SetText(inventory[objectID.healthPotion].ToString());
        }
    }

    public void UpdateCurrentTurn(int currentTurn)
    {
        TurnText.SetText("Turn: " + currentTurn.ToString());
    }

    public void OnHealthPotion()
    {
        eventManager.TriggerHealthPotionUsed();
    }
}