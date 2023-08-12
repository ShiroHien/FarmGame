using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;

public class ToolsCharacterController : MonoBehaviour {

    CharacterController2D character;
    Rigidbody2D rgbd2d;
    ToolbarController toolbarController;
    Animator animator;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] float maxDistance = 1.5f;
    [SerializeField] CropsManager cropsManager;
    [SerializeField] TileData plowableTiles;

    Vector3Int selectedTilePosition;
    bool selectable;

    private void Awake() {
        character = GetComponent<CharacterController2D>();
        rgbd2d = GetComponent<Rigidbody2D>();
        toolbarController = GetComponent<ToolbarController>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        SelectTile();
        CanSelectCheck();
        Marker();
        if (Input.GetMouseButtonDown(0)) { // left click
            if (UseToolWorld() == true) { return; }
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

    private bool UseToolWorld() {
        Vector2 position = rgbd2d.position + character.lastMotionvector * offsetDistance;

        Item item = toolbarController.GetItem;

        if(item == null) { return false; }
        if (item.onAction == null) { return false; }

        animator.SetTrigger("act");
        bool complete = item.onAction.OnApply(position);
        
        return false;
    }

    private void UseToolGrid() {
        if (selectable == true) {

            TileBase tileBase = tileMapReadController.GetTileBase(selectedTilePosition);
            TileData tileData = tileMapReadController.GetTileData(tileBase);

            if (tileData != plowableTiles) { return; }

            if (cropsManager.Check(selectedTilePosition)) {
                cropsManager.Seed(selectedTilePosition);
            }
            else {
                cropsManager.Plow(selectedTilePosition);
            }
        }
    }
}
