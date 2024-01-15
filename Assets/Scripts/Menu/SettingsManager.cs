using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; set; }

    [Header("Master")]
    public Slider masterSlider;
    public GameObject masterValue;

    [Header("Music")]
    public Slider musicSlider;
    public GameObject musicValue;

    [Header("Effects")]
    public Slider effectsSlider;
    public GameObject effectsValue;

    [Header("Button")]
    public Button backBtn;

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
    }

    void Start()
    {
        backBtn.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value);
        });

        StartCoroutine(LoadAndApplySettings());
    }

    private IEnumerator LoadAndApplySettings()
    {
        LoadAndSetVolume();
        yield return new WaitForSeconds(0.1f);
    }

    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();

        masterSlider.value = volumeSettings.master;
        musicSlider.value = volumeSettings.music;
        effectsSlider.value = volumeSettings.effects;

        print("Volume Settings are Load");
    }

    void Update()
    {
        masterValue.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicValue.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        effectsValue.GetComponent<TextMeshProUGUI>().text = "" + (effectsSlider.value) + "";
    }
}