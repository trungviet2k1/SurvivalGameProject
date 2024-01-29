using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeatherSystem : MonoBehaviour
{
    [Range(0f, 1f)]
    public float chanceToRainSpring = 0.3f;
    [Range(0f, 1f)]
    public float chanceToRainSummer = 0.01f;
    [Range(0f, 1f)]
    public float chanceToRainFall = 0.4f;
    [Range(0f, 1f)]
    public float chanceToRainWinter = 0.7f;

    public GameObject rainEffect;
    public Material rainSkyBox;

    public bool isSpecialWeather;
    public AudioSource rainChanel;
    public AudioClip rainSound;

    public enum WeatherCondition
    {
        Sunny,
        Rainy
    }

    public WeatherCondition currentWeather = WeatherCondition.Sunny;

    private void Start()
    {
        TimeManager.Instance.OnDayPass.AddListener(GenerateRandomWeather);
    }

    private void GenerateRandomWeather()
    {
        TimeManager.Season currentSeason = TimeManager.Instance.currentSeason;

        float chanceToRain = 0f;

        switch (currentSeason)
        {
            case TimeManager.Season.Spring:
                chanceToRain = chanceToRainSpring;
                break;
            case TimeManager.Season.Summer:
                chanceToRain = chanceToRainSummer;
                break;
            case TimeManager.Season.Fall:
                chanceToRain = chanceToRainFall;
                break;
            case TimeManager.Season.Winter:
                chanceToRain = chanceToRainWinter;
                break;
        }

        if (Random.value <= chanceToRain)
        {
            currentWeather = WeatherCondition.Rainy;
            isSpecialWeather = true;

            RenderSettings.skybox = rainSkyBox;

            Invoke("StartRain", 1f);

            StartRain();
        }
        else
        {
            currentWeather = WeatherCondition.Sunny;
            isSpecialWeather = false;

            StopRain();
        }
    }

    private void StartRain()
    {
        if (rainChanel.isPlaying == false)
        {
            rainChanel.clip = rainSound;
            rainChanel.loop = true;
            rainChanel.Play(); 
        }

        RenderSettings.skybox = rainSkyBox;
        rainEffect.SetActive(true);
    }

    private void StopRain()
    {
        if (rainChanel.isPlaying)
        {
            rainChanel.Stop();
        }

        rainEffect.SetActive(false);
    }
}
