﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class ItemSlot
{
    public Item item;
    public int count;

    public void Copy(ItemSlot slot)
    {
        item = slot.item;
        count = slot.count;
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void Clear()
    {
        item = null;
        count = 0;
    }
}

[CreateAssetMenu(menuName = "Data/Item Container")]

public class ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;

    public void Add(Item item, int count = 1) {
        if (item.stackable) {   
            ItemSlot itemSlot = slots.Find(x => x.item == item);

            if (itemSlot != null) {
                itemSlot.count += count; 
            }
            else {
                itemSlot = slots.Find(x => x.item == null);

                if (itemSlot != null) {
                    itemSlot.item = item;
                    itemSlot.count = count;
                }
            }
        }
        else { 
            ItemSlot itemSlot = slots.Find(x => x.item == null);

            if (itemSlot != null) {
                itemSlot.item = item;
                itemSlot.count = count;
            }
        }
    }


    public void RemoveItem(Item removedItem, int count) {
        if (removedItem.stackable) {
            ItemSlot itemSlot = slots.Find(slot => slot.item == removedItem);

            if (itemSlot == null) {
                return;
            }

            itemSlot.count-=count;

            if (itemSlot.count <= 0)  {
                itemSlot.Clear();
            }
        }

        else {
            while (count > 0) { 
                count--;
                ItemSlot itemSlot = slots.Find(slot => slot.item == removedItem);
                if (itemSlot == null) {
                    break;
                }
                itemSlot.Clear();
            }
        }
    }

}
