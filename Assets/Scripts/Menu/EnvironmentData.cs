using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentData
{
    public List<string> pickupItem;
    public List<string> allItems;
    public List<TreeData> treeData;
    public List<string> bushData;
    public List<string> fruits;
    public List<string> animals;
    public List<StorageData> storage;


    public EnvironmentData(List<string> _pickupItem, List<string> _allItems, List<TreeData> _treeData, List<string> _bushData,
        /*List<TreeData> _treeData,*/ List<string> _fruits, List<string> _animals, List<StorageData> _storage)
    {
        pickupItem = _pickupItem;
        allItems = _allItems;
        treeData = _treeData;
        bushData = _bushData;
        fruits = _fruits;
        animals = _animals;
        storage = _storage;
    }
}

[System.Serializable]
public class StorageData
{
    public List<string> items;
    public Vector3 position;
    public Vector3 rotation;
}

[System.Serializable]
public class TreeData
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
}