﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public bool dialogueActive;
    GameObject toolbar;
    GameObject inventory;
    GameObject shop;
    GameObject chest;
    public string[] sentences;
    private int index;
    public Text textDisplay;
    public float typingSpeed;
    public GameObject pressToContinue;

    private void Awake() {
        toolbar = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("toolbar"));
        shop = Resources.FindObjectsOfTypeAll<GameObject>().LastOrDefault(g => g.CompareTag("shop"));
        chest = Resources.FindObjectsOfTypeAll<GameObject>().LastOrDefault(g => g.CompareTag("chest"));
        inventory = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("inventory"));
    }

    void Start() {
        toolbar.SetActive(false);
        shop.SetActive(false);
        chest.SetActive(false);
        inventory.SetActive(false);

        StartCoroutine(Type());
    }

    void Update() {
        if (textDisplay.text == sentences[index] && dialogueActive && Input.GetKeyDown(KeyCode.Space)) {
            GoToNextSentence();
            pressToContinue.SetActive(true);
        }
        
    }

    IEnumerator Type() {
        foreach(char letter in sentences[index].ToCharArray()) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void GoToNextSentence() {
        pressToContinue.SetActive(false);

        if (index < sentences.Length - 1) {
            textDisplay.text = "";
            index++;
            StartCoroutine(Type());
        } else {
            if (Input.GetKeyDown(KeyCode.Space)) {
                dialogueBox.SetActive(false);
                toolbar.SetActive(true);
                chest.SetActive(true);

                shop.SetActive(true);
            }
            
        }
    }
}
