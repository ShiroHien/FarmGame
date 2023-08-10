using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryPanel : ItemPanel
{
    public override void OnClick(int id) {
        GameManager.instance.dragAndDropController.OnClick(inventory.slots[id]);
        Show();
    }
}
