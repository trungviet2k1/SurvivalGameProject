using System.Collections.Generic;
using UnityEngine;

public class LootAble : MonoBehaviour
{
    public List<LootPossibility> possibleLoot;
    public List<LootReceived> finalLoot;

    public bool wasLootCalculated;

    void Start()
    {
        CalculateLoot();
    }

    public void CalculateLoot()
    {
        if (wasLootCalculated) return;

        finalLoot = new List<LootReceived>();

        foreach (var loot in possibleLoot)
        {
            int amount = Random.Range(loot.amountMin, loot.amountMax + 1);

            if (amount > 0)
            {
                LootReceived receivedLoot = new LootReceived();
                receivedLoot.item = loot.item;
                receivedLoot.amount = amount;
                finalLoot.Add(receivedLoot);
            }
        }

        wasLootCalculated = true;
    }
}

[System.Serializable]
public class LootPossibility
{
    public GameObject item;
    public int amountMin;
    public int amountMax;
}

[System.Serializable]
public class LootReceived
{
    public GameObject item;
    public int amount;
}