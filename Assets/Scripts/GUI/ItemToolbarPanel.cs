using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToolbarPanel : ItemPanel
{
    [SerializeField] ToolbarController toolbarController;

    private void Start() {
        Init();
        toolbarController.onChange += HighLight;
        HighLight(0);
    }

    public override void OnClick(int id) {
        toolbarController.Set(id);
        HighLight(id);
    }

    int currentSelectedTool;

    public void HighLight(int id) {
        buttons[currentSelectedTool].Hightlight(false); 
        currentSelectedTool = id;
        buttons[currentSelectedTool].Hightlight(true);
    }
}
