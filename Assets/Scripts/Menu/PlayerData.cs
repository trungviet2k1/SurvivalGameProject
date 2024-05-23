[System.Serializable]
public class PlayerData
{
    public float[] playerStarts;
    public float[] playerPositionAndRotation;
    public string[] inventoryContent;
    public string[] quickSlotContent;
    public int currency;

    public PlayerData(float[] _playerStarts, float[] _playerPosAndRot, string[] _inventoryContent, string[] _quickSlotContent, int _currency)
    {
        playerStarts = _playerStarts;
        playerPositionAndRotation = _playerPosAndRot;
        inventoryContent = _inventoryContent;
        quickSlotContent = _quickSlotContent;
        currency = _currency;
    }
}