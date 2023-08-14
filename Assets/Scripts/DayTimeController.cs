using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class DayTimeController : MonoBehaviour
{
    const float SecondsInDay = 86400f;
    public float time;
    [SerializeField] Text TimeDisplay;
    [SerializeField] float TimeScale;
    [SerializeField] float LightTransition = 0.0001f;
    public int hungerUpdaterCounter;
    public int healthUpdaterCounter;
    public int temperatureUpdateCounter;
    public int day;

    private void Start() {
        day = 0;
        time = 25200f;
        hungerUpdaterCounter = 0;
        healthUpdaterCounter = 0;
        temperatureUpdateCounter = 0;
        TemperatureController.currentTemperature = 100;
    }
    private float getHours {
        get { return time / 3600f; }
    }

    public float GetTime {
        get { return time; }
    }


    void Update() {
        if (Time.timeScale == 0)
            return;

        hungerUpdaterCounter += 1;
        temperatureUpdateCounter += 1;

        if (hungerUpdaterCounter == 250)
        {
            HungerController.currentHunger -= 1;
            hungerUpdaterCounter = 0;
        }

        if(HungerController.currentHunger < 10 || TemperatureController.currentTemperature < 10)
        {
            healthUpdaterCounter += 1;

            if (healthUpdaterCounter == 100)
            {
                HealthController.currentHealth -= 1;
                healthUpdaterCounter = 0;
            }
        }
        
        time += Time.deltaTime * TimeScale;
        int hours = (int)getHours;
        TimeDisplay.text = hours.ToString("00") + ":00";
        Light2D light = transform.GetComponent<Light2D>();

        if (time > 25200f && time < 72000f) {
            light.intensity = 1f;
            TemperatureController.currentTemperature = 100;
        }

        if ((time > 72000f && time < 86400f) || ((time > 0f && time < 18000f))) {
            if (light.intensity > 0.3f) {
                light.intensity -= LightTransition;
            }
            if (temperatureUpdateCounter > 50) {
                TemperatureController.currentTemperature -= 1;
                temperatureUpdateCounter = 0;
            }
        }
        
        if (time > 18000f && time < 25200f) {
            if (light.intensity < 1f)
                light.intensity += LightTransition;
            if (temperatureUpdateCounter > 50) {
                TemperatureController.currentTemperature -= 1;
                temperatureUpdateCounter = 0;
            }
        }

        if (time > SecondsInDay) {
            time = 0;
            day += 1;
            MoneyController.money += 200;
        }

        if(HealthController.currentHealth < 1) {
            Application.LoadLevel(3);
        }

        if(day == 9 && time > 25200f) {
            Application.LoadLevel(4);
        }
    }

}
