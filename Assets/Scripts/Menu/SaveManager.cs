using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }

    public bool isSavingToJson;

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
        AllGameData data = new();
        data.playerData = GetPlayerData();
        SavingTypeSwitch(data, slotNumber);
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

        return new PlayerData(playerStarts, playerPosAndRot);
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
        }
        else
        {
            print("PlayerState.Instance is null!");
        }
    }

    public void StartLoadedGame(int slotNumber)
    {
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
        string encryptedJson = Encrypt(json);
        using (StreamWriter writer = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json"))
        {
            writer.WriteLine(encryptedJson);
        }
        print("Saved Game to Json file at: " + jsonPathProject + fileName + slotNumber + ".json");
    }

    public AllGameData LoadGameDataFromJSonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
        {
            string encryptedJson = reader.ReadToEnd();
            string json = Decrypt(encryptedJson);
            AllGameData data = JsonUtility.FromJson<AllGameData>(json);
            print("Loaded Game from Json file at: " + jsonPathProject + fileName + slotNumber + ".json");
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
}