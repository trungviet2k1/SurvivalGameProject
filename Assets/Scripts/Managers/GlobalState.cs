using UnityEngine;

public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance {  get; set; }

    [HideInInspector] public float resourceHealth;
    [HideInInspector] public float resourceMaxHealth;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
