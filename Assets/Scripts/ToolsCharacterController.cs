using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class ToolsCharacterController : MonoBehaviour
{
    PlayerControl character;
    Rigidbody2D rgbd2d;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] CropsReadController cropsReadController;
    [SerializeField] float maxDistance = 2f;
    [SerializeField] CropsManager cropsManager;
    [SerializeField] TileData plowableTiles;
    [SerializeField] TileData toMowTiles;
    [SerializeField] TileData toSeedTiles;
    [SerializeField] TileData waterableTiles;
    InventoryController inventoryController;
    ToolbarController toolbarController;
    [SerializeField] GameObject toolbarPanel;

    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;

    private static int cornPickUpCount = 3;
    private static int parsleyPickUpCount = 1;
    private static int potatoPickUpCount = 1;
    private static int strawberryPickUpCount = 1;
    private static int tomatoPickUpCount = 1;

    private static int cornSeedsCount = 4;
    private static int parsleySeedsCount = 3;
    private static int potatoSeedsCount = 1;
    private static int strawberrySeedsCount = 6;
    private static int tomatoSeedsCount = 3;

    Vector3Int selectedTilePosition;
    Vector3Int selectedCropPosition;
    bool selectable;

    public static Dictionary<Vector2Int, TileData> fields;
    public static Dictionary<Vector2Int, CropData> crops;

    UI_ShopController shopPanel;

    void Start() {
        character = GetComponent<PlayerControl>();
        rgbd2d = GetComponent<Rigidbody2D>();
        fields = new Dictionary<Vector2Int, TileData>();
        crops = new Dictionary<Vector2Int, CropData>();
        toolbarController = GetComponent<ToolbarController>();
        inventoryController = GetComponent<InventoryController>();

        var shopPanelAll = Resources.FindObjectsOfTypeAll<UI_ShopController>();
        shopPanel = shopPanelAll[0];
    }

    void Update() {
        SelectTile();
        CanSelectCheck();
        Marker();
        if (Input.GetMouseButtonDown(0)) { 
            if (!inventoryController.isOpen) {
                if (UseToolWorld() == true) {
                    return;
                }
                UseTool();
            }

        }
    }

    private bool CastRay() { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit) {
            if (hit.collider.gameObject.name.Contains("Tree")) {
                return true;
            }
            if (hit.collider.gameObject.name.Contains("CampFire")) {
                return true;
            }
            if (hit.collider.gameObject.name.Contains("Chest")) {
                return true;
            }
        }
        return false;
    }
    private bool CastRayPlayer() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit) {
            if (hit.collider.gameObject.name.Contains("Player")) {
                return true;
            }
        }
        return false;
    }
    private void SelectTile() {
        selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
        TileBase tileBase = tileMapReadController.GetTileBase(selectedTilePosition);
        try {
            TileData tileData = tileMapReadController.GetTileData(tileBase);
            if (!(tileData is null)) {
                if (!fields.ContainsKey((Vector2Int)selectedTilePosition)) {
                    fields.Add((Vector2Int)selectedTilePosition, tileData);
                }
                else {
                    fields[(Vector2Int)selectedTilePosition] = tileData;
                }
            }
        }
        catch {
            return;
        }

        selectedCropPosition = cropsReadController.GetGridPosition(Input.mousePosition, true);
        TileBase cropBase = cropsReadController.GetTileBase(selectedTilePosition);
        try {
            CropData cropData = cropsReadController.GetCropData(cropBase);
            if (!(cropData is null)) {
                if (!crops.ContainsKey((Vector2Int)selectedTilePosition)) {
                    crops.Add((Vector2Int)selectedTilePosition, cropData);
                }
                else {
                    crops[(Vector2Int)selectedTilePosition] = cropData;
                }
            }
        }
        catch
        {
            return;
        }

    }

    void CanSelectCheck() {
        if (Time.timeScale == 0)
            return;

        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
    }

    private void Marker() {
        markerManager.markedCellPosition = selectedTilePosition;
    }

    private bool UseToolWorld() {
        if (Time.timeScale == 0)
            return false;
        Vector2 position = rgbd2d.position + character.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);


        foreach (Collider2D collidor in colliders) {
            ToolHit hitTree = collidor.GetComponent<ToolHit>();
            CampFireHit hitFire = collidor.GetComponent<CampFireHit>();
            ChestHit hitChest = collidor.GetComponent<ChestHit>();
            PlayerHit hitPlayer = collidor.GetComponent<PlayerHit>();

            if (hitTree != null && toolbarController.GetItem != null &&
                toolbarController.GetItem.Name == "Axe" && CastRay() == true)
            {
                hitTree.Hit();
                return true;
            }
            if (hitFire != null && toolbarController.GetItem != null &&
                toolbarController.GetItem.Name == "Wood" && CastRay() == true)
            {
                hitFire.Hit();
                return true;
            }
            if (hitChest != null && CastRay() == true)
            {
                hitChest.Hit();
                return true;
            }
            if (hitPlayer != null && toolbarController.GetItem != null && CastRayPlayer() == true && (toolbarController.GetItem.Name == "Food_Corn" || toolbarController.GetItem.Name == "Food_Parsley"
                    || toolbarController.GetItem.Name == "Food_Potato" || toolbarController.GetItem.Name == "Food_Strawberry" || toolbarController.GetItem.Name == "Food_Tomato"))
            {
                hitPlayer.Hit();
                return true;
            }
        }

        return false;
    }

    private void RefreshToolbar()
    {
        toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy);
        toolbarPanel.SetActive(true);
    }

    private void UseTool()
    {
        if (Time.timeScale == 0)
            return;

        if (selectable == true && toolbarController.GetItem != null)
        {
            TileBase tileBase = tileMapReadController.GetTileBase(selectedTilePosition);
            TileData tileData = tileMapReadController.GetTileData(tileBase);

            if (tileData != plowableTiles && tileData != toMowTiles && tileData != toSeedTiles && tileData != waterableTiles)
            {
                return;
            }
            if (crops[(Vector2Int)selectedTilePosition].noPlant)
            {
                if (fields[(Vector2Int)selectedTilePosition].ableToMow && toolbarController.GetItem.Name == "Shovel" 
                    && shopPanel.isOpen == false)
                {
                    cropsManager.Mow(selectedTilePosition);
                }
                else if (fields[(Vector2Int)selectedTilePosition].plowable && toolbarController.GetItem.Name == "Hoe")
                {
                    cropsManager.Plow(selectedTilePosition);
                }
                else if (fields[(Vector2Int)selectedTilePosition].ableToSeed && toolbarController.GetItem.isSeed == true)
                {
                    switch (toolbarController.GetItem.Name)
                    {
                        case "Seeds_Corn":
                            if (GameManager.instance.inventoryContainer.slots[toolbarController.selectedTool].count 
                                >= cornSeedsCount)
                            {
                                cropsManager.SeedCrop(selectedTilePosition, "corn");
                                GameManager.instance.inventoryContainer.RemoveItem(toolbarController.GetItem, cornSeedsCount);
                            }
                        break;
                        case "Seeds_Parsley":
                            if (GameManager.instance.inventoryContainer.slots[toolbarController.selectedTool].count 
                                >= parsleySeedsCount)
                            {
                                cropsManager.SeedCrop(selectedTilePosition, "parsley");
                                GameManager.instance.inventoryContainer.RemoveItem(toolbarController.GetItem, parsleySeedsCount);
                            }
                        break;
                        case "Seeds_Potato":
                            if (GameManager.instance.inventoryContainer.slots[toolbarController.selectedTool].count 
                                >= potatoSeedsCount)
                            {
                                cropsManager.SeedCrop(selectedTilePosition, "potato");
                                GameManager.instance.inventoryContainer.RemoveItem(toolbarController.GetItem, potatoSeedsCount);
                            }
                        break;
                        case "Seeds_Strawberry":
                            if (GameManager.instance.inventoryContainer.slots[toolbarController.selectedTool].count 
                                >= strawberrySeedsCount)
                            {
                                cropsManager.SeedCrop(selectedTilePosition, "strawberry");
                                GameManager.instance.inventoryContainer.RemoveItem(toolbarController.GetItem, strawberrySeedsCount);
                            }
                        break;
                        case "Seeds_Tomato":
                            if (GameManager.instance.inventoryContainer.slots[toolbarController.selectedTool].count 
                                >= tomatoSeedsCount)
                            {
                                cropsManager.SeedCrop(selectedTilePosition, "tomato");
                                GameManager.instance.inventoryContainer.RemoveItem(toolbarController.GetItem, tomatoSeedsCount); 
                            }
                        break;
                    }

                    RefreshToolbar();
                }               
            }

            else if (crops[(Vector2Int)selectedTilePosition].planted && fields[(Vector2Int)selectedTilePosition].waterable && toolbarController.GetItem.Name == "WateringCan")
            {
                cropsManager.Water(selectedTilePosition);
                FindObjectOfType<SoundManager>().Play("Water");
            }

            else if (crops[(Vector2Int)selectedTilePosition].collectibleCorn && toolbarController.GetItem.Name == "Bag")
            {
                cropsManager.Collect(selectedTilePosition, "corn");
                foreach (ItemSlot itemSlot in GameManager.instance.allItemsContainer.slots)
                {
                    if (itemSlot.item.Name == "Food_Corn")
                    {
                        GameManager.instance.inventoryContainer.Add(itemSlot.item, cornPickUpCount);
                        RefreshToolbar();
                        break;
                    }
                }

            }
            else if (crops[(Vector2Int)selectedTilePosition].collectibleParsley && toolbarController.GetItem.Name == "Bag")
            {
                cropsManager.Collect(selectedTilePosition, "parsley");
                foreach (ItemSlot itemSlot in GameManager.instance.allItemsContainer.slots)
                {
                    if (itemSlot.item.Name == "Food_Parsley")
                    {
                        GameManager.instance.inventoryContainer.Add(itemSlot.item, parsleyPickUpCount);
                        RefreshToolbar();
                        break;
                    }
                }
            }
            else if (crops[(Vector2Int)selectedTilePosition].collectiblePotato && toolbarController.GetItem.Name == "Bag")
            {
                cropsManager.Collect(selectedTilePosition, "potato");
                foreach (ItemSlot itemSlot in GameManager.instance.allItemsContainer.slots)
                {
                    if (itemSlot.item.Name == "Food_Potato")
                    {
                        GameManager.instance.inventoryContainer.Add(itemSlot.item, potatoPickUpCount);
                        RefreshToolbar();
                        break;
                    }
                }
            }
            else if (crops[(Vector2Int)selectedTilePosition].collectibleStrawberry && toolbarController.GetItem.Name == "Bag")
            {
                cropsManager.Collect(selectedTilePosition, "strawberry");
                foreach (ItemSlot itemSlot in GameManager.instance.allItemsContainer.slots)
                {
                    if (itemSlot.item.Name == "Food_Strawberry")
                    {
                        GameManager.instance.inventoryContainer.Add(itemSlot.item, strawberryPickUpCount);
                        RefreshToolbar();
                        break;
                    }
                }
            }
            else if (crops[(Vector2Int)selectedTilePosition].collectibleTomato && toolbarController.GetItem.Name == "Bag")
            {
                cropsManager.Collect(selectedTilePosition, "tomato");
                foreach (ItemSlot itemSlot in GameManager.instance.allItemsContainer.slots)
                {
                    if (itemSlot.item.Name == "Food_Tomato")
                    {
                        GameManager.instance.inventoryContainer.Add(itemSlot.item, tomatoPickUpCount);
                        RefreshToolbar();
                        break;
                    }
                }
            }

        }
    }
}
