using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCuttable : Toolhit {
    public override void Hit() {
        Destroy(gameObject);
    }
}
