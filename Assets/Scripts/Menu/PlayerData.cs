[System.Serializable]
public class PlayerData
{
    public float[] playerStarts;
    public float[] playerPositionAndRotaion;
    public string[] inventoryContent;
    public string[] quickSlotContent;

    public PlayerData(float[] _playerStarts, float[] _playerPosAndRot, string[] _inventoryContent, string[] _quickSlotContent)
    {
        playerStarts = _playerStarts;
        playerPositionAndRotaion = _playerPosAndRot;
        inventoryContent = _inventoryContent;
        quickSlotContent = _quickSlotContent;
    }
}