using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCuttable : Toolhit {
    [SerializeField] GameObject pickUpDrop; 

    [SerializeField] Item item;
    [SerializeField] int dropCount = 15;
    [SerializeField] int itemCountInOneDrop = 1;

    [SerializeField] float spread = 0.7f;
    public override void Hit() {
        while (dropCount > 0) {
            dropCount--;

            Vector3 position = transform.position;
            position.x += spread * UnityEngine.Random.value - spread / 2;
            position.y += spread * UnityEngine.Random.value - spread / 2;

            ItemSpawnManager.instance.SpawnItem(position, item, itemCountInOneDrop);
        }
        
        Destroy(gameObject);
    }
}