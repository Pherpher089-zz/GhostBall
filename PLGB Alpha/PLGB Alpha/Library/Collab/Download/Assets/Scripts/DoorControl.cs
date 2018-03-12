using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : Interaction {

    Transform hinge;

    private void Awake()
    {
        hinge = transform.parent;
    }

    public override void Interact()
    {
        transform.Rotate(0, 90, 0);
    }
}
