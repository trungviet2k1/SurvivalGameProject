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
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
        binaryPath = Application.persistentDataPath + "/save_game.bin";
    }

    #region ||--- General Section --- ||

    #region ||--- Saving --- ||
    public void SaveGame()
    {
        AllGameData data = new();
        data.playerData = GetPlayerData();
        SavingTypeSwitch(data);
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

    public void SavingTypeSwitch(AllGameData gameData)
    {
        if (isSavingToJson)
        {
            SaveGameDataToJSonFile(gameData);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData);
        }
    }
    #endregion

    #region ||--- Loading --- ||

    public AllGameData LoadingTypeSwitch()
    {
        if (isSavingToJson)
        {
            AllGameData gameData = LoadGameDataFromJSonFile();
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile();
            return gameData;
        }
    }

    public void LoadGame()
    {
        AllGameData gameData = LoadingTypeSwitch();

        //Player Data
        SetPlayerData(gameData.playerData);

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

    public void StartLoadedGame()
    {
        SceneManager.LoadScene("GameScene");
        StartCoroutine(DelayedLoading());
    }

    private IEnumerator DelayedLoading()
    {
        yield return new WaitForSeconds(1f);
        LoadGame();
    }

    #endregion

    #endregion

    #region ||--- To Binary Section --- ||

    public void SaveGameDataToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(binaryPath, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Data saved to " + binaryPath);
    }

    public AllGameData LoadGameDataFromBinaryFile()
    {
        if (File.Exists(binaryPath))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(binaryPath, FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("Data loaded from " + binaryPath);

            return data;
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region ||--- To JSon Section --- ||

    public void SaveGameDataToJSonFile(AllGameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);
        string encryptedJson = Encrypt(json);
        using (StreamWriter writer = new StreamWriter(jsonPathProject))
        {
            writer.WriteLine(encryptedJson);
        }
        print("Saved Game to Json file at: " + jsonPathProject);
    }

    public AllGameData LoadGameDataFromJSonFile()
    {
        using (StreamReader reader = new StreamReader(jsonPathProject))
        {
            string encryptedJson = reader.ReadToEnd();
            string json = Decrypt(encryptedJson);
            AllGameData data = JsonUtility.FromJson<AllGameData>(json);
            print("Loaded Game from Json file at: " + jsonPathProject);
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
}