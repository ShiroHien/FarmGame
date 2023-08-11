using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ToolsCharacterController : MonoBehaviour {
    CharacterController2D character;
    Rigidbody2D rgbd2d;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] float maxDistance = 1.5f;
    [SerializeField] CropsManager cropsManager;

    Vector3Int selectedTilePosition;
    bool selectable;

    private void Awake() {
        character = GetComponent<CharacterController2D>();
        rgbd2d = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        SelectTile();
        CanSelectCheck();
        Marker();
        if (Input.GetMouseButtonDown(0)) { // left click
            UseToolWorld();
            UseToolGrid();
        }
    }

    private void SelectTile() {
        selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
    }

    void CanSelectCheck() {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
    }

    private void Marker() {
        markerManager.markedCellPosition = selectedTilePosition;
    }

    private void UseToolWorld() {
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

    private void UseToolGrid() {
        if (selectable == true) {
            cropsManager.Plow(selectedTilePosition);
        }
    }
}
