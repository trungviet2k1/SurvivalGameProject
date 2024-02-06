using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }

    public bool isSavingToJson;
    public bool isLoading;
    public Canvas loadingScreen;

    //JSon Project Save Path
    string jsonPathProject;

    //JSon External/Real Save Path
    string jsonPathPersistant;

    //Binary Save Path
    string binaryPath;

    string fileName = "SavedGame";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    #region ||--- General Section --- ||

    #region ||--- Saving --- ||
    public void SaveGame(int slotNumber)
    {
        AllGameData data = new()
        {
            playerData = GetPlayerData(),
            environmentData = GetEnvironmentData()
        };
        SavingTypeSwitch(data, slotNumber);
    }

    private EnvironmentData GetEnvironmentData()
    {
        List<string> itemsPickedup = InventorySystem.Instance.itemPickedup;

        Dictionary<string, List<TreeData>> treeDataDict = new();

        foreach (Transform tree in EnvironmentManager.Instance.allTrees.transform)
        {
            string treeType = tree.CompareTag("Plant") ? "Parent" : "Stump";
            string treeName = "";

            if (tree.name.Contains("Birch"))
            {
                treeName = "BirchTree" + treeType;
            }
            else if (tree.name.Contains("Oak"))
            {
                treeName = "OakTree" + treeType;
            }
            else if (tree.name.Contains("Deciduous"))
            {
                treeName = "DeciduousTree" + treeType;
            }

            if (!treeDataDict.ContainsKey(treeName))
            {
                treeDataDict[treeName] = new List<TreeData>();
            }

            var td = new TreeData
            {
                name = treeName,
                position = tree.position,
                rotation = new Vector3(tree.rotation.x, tree.rotation.y, tree.rotation.z)
            };

            treeDataDict[treeName].Add(td);
        }

        List<string> allBushs = new();
        foreach (Transform bushType in EnvironmentManager.Instance.allBushs.transform)
        {
            foreach (Transform bush in bushType.transform)
            {
                allBushs.Add(bush.gameObject.name);
            }
        }

        List<string> allFruits = new();
        foreach (Transform fruits in EnvironmentManager.Instance.allFruits.transform)
        {
            foreach (Transform fruit in fruits.transform)
            {
                allFruits.Add(fruit.gameObject.name);
            }
        }

        List<string> allAnimals = new();
        foreach (Transform animalType in EnvironmentManager.Instance.allAnimals.transform)
        {
            foreach (Transform animal in animalType.transform)
            {
                allAnimals.Add(animal.gameObject.name);
            }
        }

        List<StorageData> allStorage = new();
        foreach (Transform placeable in EnvironmentManager.Instance.placeable.transform)
        {
            if (placeable.gameObject.GetComponent<StorageBox>())
            {
                var sd = new StorageData
                {
                    items = placeable.gameObject.GetComponent<StorageBox>().items,
                    position = placeable.position,
                    rotation = new Vector3(placeable.rotation.x, placeable.rotation.y, placeable.rotation.z)
                };

                allStorage.Add(sd);
            }
        }

        List<TreeData> treeToSave = treeDataDict.Values.SelectMany(x => x).ToList();

        return new EnvironmentData(itemsPickedup, treeToSave, allBushs, allFruits, allAnimals, allStorage);
    }

    private PlayerData GetPlayerData()
    {
        float[] playerStarts = new float[3];
        playerStarts[0] = PlayerState.Instance.currentHealth;
        playerStarts[1] = PlayerState.Instance.currentCalories;
        playerStarts[2] = PlayerState.Instance.currentHydrationPercent;

        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

        string[] inventory = InventorySystem.Instance.itemList.ToArray();
        string[] quickSlot = GetQuickSlotContent();

        return new PlayerData(playerStarts, playerPosAndRot, inventory, quickSlot);
    }

    private string[] GetQuickSlotContent()
    {
        List<string> temp = new();

        foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if (slot.transform.childCount != 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string cleanName = name.Replace(str2, "");
                temp.Add(cleanName);
            }
        }

        return temp.ToArray();
    }

    public void SavingTypeSwitch(AllGameData gameData, int slotNumber)
    {
        if (isSavingToJson)
        {
            SaveGameDataToJSonFile(gameData, slotNumber);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData, slotNumber);
        }
    }
    #endregion

    #region ||--- Loading --- ||

    public AllGameData LoadingTypeSwitch(int slotNumber)
    {
        if (isSavingToJson)
        {
            AllGameData gameData = LoadGameDataFromJSonFile(slotNumber);
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile(slotNumber);
            return gameData;
        }
    }

    public void LoadGame(int slotNumber)
    {
        //Player Data
        SetPlayerData(LoadingTypeSwitch(slotNumber).playerData);

        //Environment Data
        SetEnvironmentData(LoadingTypeSwitch(slotNumber).environmentData);

        isLoading = false;

        DisableLoadingScreen();
    }

    private void SetEnvironmentData(EnvironmentData environmentData)
    {
        // ======= Pick up items ======= //
        foreach (Transform itemType in EnvironmentManager.Instance.allItems.transform)
        {
            foreach (Transform item in itemType.transform)
            {
                if (environmentData.pickupItem.Contains(item.name))
                {
                    Destroy(item.gameObject);
                }
            }
        }

        InventorySystem.Instance.itemPickedup = environmentData.pickupItem;

        // ======= Trees ======= //
        //Remove all default trees
        foreach (Transform tree in EnvironmentManager.Instance.allTrees.transform)
        {
            Destroy(tree.gameObject);
        }

        // Add Trees and Stump
        foreach (TreeData tree in environmentData.treeData)
        {
            var treePrefab = Instantiate(Resources.Load<GameObject>(tree.name),
                new Vector3(tree.position.x, tree.position.y, tree.position.z),
                Quaternion.Euler(tree.rotation.x, tree.rotation.y, tree.rotation.z));

            treePrefab.transform.SetParent(EnvironmentManager.Instance.allTrees.transform);
        }

        // ======= Fruits ======= //
        foreach (Transform fruits in EnvironmentManager.Instance.allFruits.transform)
        {
            foreach (Transform fruit in fruits.transform)
            {
                if (!environmentData.fruits.Contains(fruit.gameObject.name))
                {
                    Destroy(fruit.gameObject);
                }
            }
        }

        // ======= Animals ======= //
        foreach (Transform animalType in EnvironmentManager.Instance.allAnimals.transform)
        {
            foreach (Transform animal in animalType.transform)
            {
                if (!environmentData.animals.Contains(animal.gameObject.name))
                {
                    Destroy(animal.gameObject);
                }
            }
        }

        // ======= Storage Boxes ======= //
        foreach (Transform placeable in EnvironmentManager.Instance.placeable.transform)
        {
            if (placeable.gameObject.GetComponent<StorageBox>())
            {
                Destroy(placeable.gameObject);
            }
        }

        foreach (StorageData storage in environmentData.storage)
        {
            var storageBoxPrefab = Instantiate(Resources.Load<GameObject>("StorageBoxModel"),
                new Vector3(storage.position.x, storage.position.y, storage.position.z),
                Quaternion.Euler(storage.rotation.x, storage.rotation.y, storage.rotation.z));

            storageBoxPrefab.GetComponent<StorageBox>().items = storage.items;

            storageBoxPrefab.transform.SetParent(EnvironmentManager.Instance.placeable.transform);
        }
    }

    private void SetPlayerData(PlayerData playerData)
    {
        if (PlayerState.Instance != null)
        {
            //Setting Player Starts
            PlayerState.Instance.currentHealth = playerData.playerStarts[0];
            PlayerState.Instance.currentCalories = playerData.playerStarts[1];
            PlayerState.Instance.currentHydrationPercent = playerData.playerStarts[2];

            //Setting Player Position
            Vector3 loadedPosition;
            loadedPosition.x = playerData.playerPositionAndRotaion[0];
            loadedPosition.y = playerData.playerPositionAndRotaion[1];
            loadedPosition.z = playerData.playerPositionAndRotaion[2];

            PlayerState.Instance.playerBody.transform.position = loadedPosition;

            //Setting Player Rotaion
            Vector3 loadedRotation;
            loadedRotation.x = playerData.playerPositionAndRotaion[3];
            loadedRotation.y = playerData.playerPositionAndRotaion[4];
            loadedRotation.z = playerData.playerPositionAndRotaion[5];

            PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);

            //Setting the inventory content
            foreach (string item in playerData.inventoryContent)
            {
                InventorySystem.Instance.AddToInventory(item);
            }

            //Setting the quicky slot content
            foreach (string item in playerData.quickSlotContent)
            {
                //Find next free quick slot
                GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();
                var itemToAdd = Instantiate(Resources.Load<GameObject>(item));
                itemToAdd.transform.SetParent(availableSlot.transform, false);
            }

            isLoading = false;
        }
        else
        {
            print("PlayerState.Instance is null!");
        }
    }

    public void StartLoadedGame(int slotNumber)
    {
        ActivateLoadingScreen();
        isLoading = true;
        SceneManager.LoadScene("GameScene");
        StartCoroutine(DelayedLoading(slotNumber));
    }

    private IEnumerator DelayedLoading(int slotNumber)
    {
        yield return new WaitForSeconds(1f);
        LoadGame(slotNumber);
    }

    #endregion

    #endregion

    #region ||--- To Binary Section --- ||

    public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Data saved to " + binaryPath + fileName + slotNumber + ".bin");
    }

    public AllGameData LoadGameDataFromBinaryFile(int slotNumber)
    {
        if (File.Exists(binaryPath + fileName + slotNumber + ".bin"))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("Data loaded from " + binaryPath + fileName + slotNumber + ".bin");

            return data;
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region ||--- To JSon Section --- ||

    public void SaveGameDataToJSonFile(AllGameData gameData, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);
        //string encryptedJson = Encrypt(json);
        using (StreamWriter writer = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json"))
        {
            writer.WriteLine(json);
        }
    }

    public AllGameData LoadGameDataFromJSonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
        {
            //string encryptedJson = reader.ReadToEnd();
            //string json = Decrypt(encryptedJson);
            string json = reader.ReadToEnd();
            AllGameData data = JsonUtility.FromJson<AllGameData>(json);
            return data;
        }
    }

    #endregion

    #region ||--- Settings Section --- ||

    #region ||--- Volume Settings --- ||
    [System.Serializable]
    public class VolumeSettings
    {
        public float music;
        public float effects;
        public float master;
    }

    public void SaveVolumeSettings(float _music, float _effects, float _master)
    {
        VolumeSettings volumeSettings = new()
        {
            music = _music,
            effects = _effects,
            master = _master
        };

        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();
    }

    public VolumeSettings LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
    }
    #endregion

    #endregion

    #region || --- EncryptionDecryption --- ||

    private readonly string encryptionKey = "1234567";

    private string Encrypt(string jsonString)
    {
        byte[] data = Encoding.UTF8.GetBytes(jsonString);
        byte[] key = Encoding.UTF8.GetBytes(encryptionKey);

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (byte)(data[i] ^ key[i % key.Length]);
        }

        return Convert.ToBase64String(data);
    }

    private string Decrypt(string encryptedString)
    {
        byte[] data = Convert.FromBase64String(encryptedString);
        byte[] key = Encoding.UTF8.GetBytes(encryptionKey);

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (byte)(data[i] ^ key[i % key.Length]);
        }

        return Encoding.UTF8.GetString(data);
    }

    #endregion

    #region || --- Utility --- ||

    public bool DoesFileExits(int slotNumber)
    {
        if (isSavingToJson)
        {
            if (System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool IsSlotEmpty(int slotNumber)
    {
        if (DoesFileExits(slotNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    #endregion

    #region || --- Loading Screen --- ||

    public void ActivateLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Music for loading screen

        //Animation

        //Show
    }

    public void DisableLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(false);
    }

    #endregion
}