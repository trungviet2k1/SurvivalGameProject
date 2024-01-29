using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    public Light directionlLight;

    public float dayDurationInSeceonds = 24.0f;
    public int currentHours;
    float currentTimeOfDay = 0.35f;

    public List<SkyBoxTimeMapping> timeMappings;
    float blenderValue = 0.0f;
    bool lockNextDayTrigger = false;
    public TextMeshProUGUI timeUI;
    public Shader myShader;

    public WeatherSystem weatherSystem;

    void Update()
    {

        currentTimeOfDay += Time.deltaTime / dayDurationInSeceonds;
        currentTimeOfDay %= 1;

        currentHours = Mathf.FloorToInt(currentTimeOfDay * 24);

        timeUI.text = $"{currentHours}:00";

        directionlLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));

        if (weatherSystem.isSpecialWeather == false)
        {
            UpdateSkybox(); 
        }

        if (currentHours == 0 && lockNextDayTrigger == false)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }

        if (currentHours != 0)
        {
            lockNextDayTrigger = false;
        }
    }

    private void UpdateSkybox()
    {
        Material currentSkyBox = null;
        foreach (SkyBoxTimeMapping mapping in timeMappings)
        {
            if (currentHours == mapping.hour)
            {
                currentSkyBox = mapping.skyboxMaterial;
                if (currentSkyBox.shader != null )
                {
                    if (currentSkyBox.shader.name == myShader.name)
                    {
                        blenderValue += Time.deltaTime;
                        blenderValue = Mathf.Clamp01(blenderValue);

                        currentSkyBox.SetFloat("_TransitionFactor", blenderValue);
                    }
                    else
                    {
                        blenderValue = 0;
                    }
                }

                break;
            } 
        }

        if (currentSkyBox != null)
        {
            RenderSettings.skybox = currentSkyBox;
        }
    }
}

[System.Serializable]
public class SkyBoxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterial;
}