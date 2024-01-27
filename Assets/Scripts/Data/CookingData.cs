using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingData", menuName = "ScriptableObject/CookingData", order = 1)]
public class CookingData : ScriptableObject
{
    public List<string> validFuels = new();
    public List<CookableFoods> validFoods = new();
}

[System.Serializable]
public class CookableFoods
{
    public string name;
    public float timeToCook;
    public string cookedFoodName;
}