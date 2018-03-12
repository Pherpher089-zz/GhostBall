using UnityEngine;
using System.Collections;

/// <summary>
/// The derivitive for all interaction scripts. This may not stay this way for long. I may add a slot in the interaction manager for a scriptable object instead of this extra class.
/// </summary>
public class Interaction : MonoBehaviour
{
    [HideInInspector] public bool interaction;          //Returns true when interac is triggerd

    public virtual void Interact()
    {
        
    }
}
