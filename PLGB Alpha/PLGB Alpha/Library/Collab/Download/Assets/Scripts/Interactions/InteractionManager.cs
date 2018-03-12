using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Manages interactions by other objects to this object
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public UnityEvent OnInteract;
    Interaction interaction;

    private void Awake()
    {
        if(GetComponent<Interaction>())
        {
            interaction = GetComponent<Interaction>();
        }
        else
        {
            Debug.LogError(gameObject.name + " does not have an Interaction component. This is required by the InteractionManager on this object. You may need to add an Interaction component in the inspector.");
            return;
        }

        OnInteract.AddListener(interaction.Interact);
    }

    public void Interact()
    {
        OnInteract.Invoke();
    }
}
