using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentGeneratorData
{
    public List<string> pickupItem;
    public List<TreeGeneratorData> treeData;
    public List<string> animals;


    public EnvironmentGeneratorData(List<string> _pickupItem, List<TreeGeneratorData> _treeData, List<string> _animals)
    {
        pickupItem = _pickupItem;
        treeData = _treeData;
        animals = _animals;
    }
}

[System.Serializable]
public class TreeGeneratorData
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
}
