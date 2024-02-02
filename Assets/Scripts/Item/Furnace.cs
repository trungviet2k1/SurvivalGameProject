using UnityEngine;

public class Furnace : MonoBehaviour
{
    public bool playerInRange;
    public bool isFurnacing;
    public float calciningTimer;

    public CalcinableMinerals mineralsBeingCalcined;
    public string readyMineral;

    public GameObject fire;

    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (isFurnacing)
        {
            calciningTimer -= Time.deltaTime;
            fire.SetActive(true);
        }
        else
        {
            fire.SetActive(false);
        }

        if (calciningTimer <= 0 && isFurnacing)
        {
            isFurnacing = false;
            readyMineral = GetCookedMineral(mineralsBeingCalcined);
        }
    }

    private string GetCookedMineral(CalcinableMinerals mineral)
    {
        return mineral.calcinedMineralName;
    }

    public void OpenUI()
    {
        FurnaceUIManager.Instance.OpenUI();
        FurnaceUIManager.Instance.selectedFurnace = this;

        if (readyMineral != "")
        {
            GameObject rm = Instantiate(Resources.Load<GameObject>(readyMineral),
                FurnaceUIManager.Instance.mineralSlot.transform.position,
                FurnaceUIManager.Instance.mineralSlot.transform.rotation);

            rm.transform.SetParent(FurnaceUIManager.Instance.mineralSlot.transform);

            readyMineral = "";
        }
    }

    public void StartFurnacing(InventoryItem mineral)
    {
        mineralsBeingCalcined = ConverIntoCalcinable(mineral);

        isFurnacing = true;

        calciningTimer = TimeToCalcinedMinerals(mineralsBeingCalcined);
    }

    private CalcinableMinerals ConverIntoCalcinable(InventoryItem mineral)
    {
        foreach (CalcinableMinerals calcinable in FurnaceUIManager.Instance.furnacingData.validMinerals)
        {
            if (calcinable.name == mineral.thisName)
            {
                return calcinable;
            }
        }
        return new CalcinableMinerals();
    }

    private float TimeToCalcinedMinerals(CalcinableMinerals mineral)
    {
        return mineral.timeToCalcine;
    }
}