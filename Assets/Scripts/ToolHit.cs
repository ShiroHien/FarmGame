using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolhit : MonoBehaviour {
    public virtual void Hit() {

    }

    public virtual bool CanBeHit(List<ResourceNodeType> canBeHit) {
        return true;    
    }
}
