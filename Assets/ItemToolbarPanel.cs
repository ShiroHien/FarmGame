using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToolbarPanel : ItemPanel
{
    [SerializeField] ToolbarController toolbarController;

    public override void OnClick(int id) {
        toolbarController.Set(id);
        buttons[id].Hightlight(true);
    }

    int currentSelectedTool;

    public void HighLight(int id) {
        buttons[currentSelectedTool].Hightlight(false); 
        currentSelectedTool = id;
        buttons[currentSelectedTool].Hightlight(true);
    }
}
