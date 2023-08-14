using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class SeedSlot
{
    public Crop item;
    public int count;

    public void Copy(SeedSlot slot) {
        item = slot.item;
        count = slot.count;
    }

    public void Set(Crop item, int count) {
        this.item = item;
        this.count = count;
    }

    public void Clear() {
        item = null;
        count = 0;
    }
}
[CreateAssetMenu(menuName = "Data/Seed Container")]

public class SeedContainer : ScriptableObject
{
    public List<SeedSlot> slots;

    public void Add(Crop item, int count = 1) {

            SeedSlot SeedSlot = slots.Find(x => x.item == null);
            if (SeedSlot != null) {
                SeedSlot.item = item;
                SeedSlot.count = count;
            }
        }
    

}
