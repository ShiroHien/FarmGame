using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainerInteract : Interactable
{
    [SerializeField] GameObject closedChest;
    [SerializeField] GameObject openedChest;
    [SerializeField] bool open;

    public override void Interact(Character character) {
        if (open == false) {
            open = true;
            closedChest.SetActive(false);
            openedChest.SetActive(true);
        }
    }


}
