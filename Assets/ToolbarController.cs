using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 12;
    int selectedTool;

    private void Update() {
        float delta = Input.mouseScrollDelta.y;

        if (delta != 0 ) {
            if(delta > 0 ) { 
                selectedTool += 1;
            } else {
                selectedTool -= 1;
            }
            Debug.Log(selectedTool);
        }
    }

}
