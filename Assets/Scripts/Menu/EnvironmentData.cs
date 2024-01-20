using System.Collections.Generic;

[System.Serializable]

public class EnvironmentData
{
    public List<string> pickupItem;
    public EnvironmentData(List<string> _pickupItem)
    {
        pickupItem = _pickupItem;
    }
}
