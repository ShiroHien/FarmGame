using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTriggerController : MonoBehaviour
{
    bool playerinrange = false;
    void Start() {
        
    }

    void Update() {
        if (playerinrange == true && TemperatureController.currentTemperature < 100) {
            TemperatureController.currentTemperature += 1;
        }
        if (!FindObjectOfType<SoundManager>().SoundIsPlaying("Fire")) {
            FindObjectOfType<SoundManager>().Play("Fire");
        }

    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerinrange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerinrange = false;
        }
    }
}
