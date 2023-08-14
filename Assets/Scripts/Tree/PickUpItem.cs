using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed = 5f;
    [SerializeField] float pickUpDistance = 1.5f;
    [SerializeField] Item item;
    public int count = 1;
    GameObject toolbar;

    private void Start() {
        player = GameManager.instance.player.transform;
        toolbar = GameObject.FindWithTag("toolbar");
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > pickUpDistance) {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        if (distance < 0.1f) {
            if (GameManager.instance.inventoryContainer != null)
            {
                GameManager.instance.inventoryContainer.Add(item, count);

                toolbar.SetActive(!toolbar.activeInHierarchy);
                toolbar.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No inventory container attached to game manager");
            }

            Destroy(gameObject);
            
        }
    }
}
