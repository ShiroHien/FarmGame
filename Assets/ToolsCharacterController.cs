using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsCharacterController : MonoBehaviour {
    CharacterController2D character;
    Rigidbody2D rgbd2d;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;

    private void Awake() {
        character = GetComponent<CharacterController2D>();
        rgbd2d = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            UseTool();
        }
    }

    private void UseTool() {
        Vector2 position = rgbd2d.position + character.lastMotionvector * offsetDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);  // xu ly va cham voi vat co the pha huy

        foreach (Collider2D c in colliders) {
            Toolhit hit = c.GetComponent<Toolhit>();
            if (hit != null) {
                hit.Hit();
                break;
            }
        }
    }
}
