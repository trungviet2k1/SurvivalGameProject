[System.Serializable]
public class PlayerData
{
    public float[] playerStarts;
    public float[] playerPositionAndRotaion;
    //public string[] inventoryContent;

    public PlayerData(float[] _playerStarts, float[] _playerPosAndRot)
    {
        playerStarts = _playerStarts;
        playerPositionAndRotaion = _playerPosAndRot;
    }
}