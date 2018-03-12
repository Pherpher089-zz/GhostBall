using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactions with other objects by this object
/// </summary>
public class ActorInteraction : MonoBehaviour {

    public float interactionDistance = 4;
    public float sightSphereRadious = 1;
    int interactLayer;
    ActorEquipment equipment;

    private void Awake()
    {
        equipment = GetComponent<ActorEquipment>();
    }
    public void Start()
    {
        interactLayer = LayerMask.GetMask("Interaction");
    }

    public void RaycastInteraction(bool interact)
    {
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward * 4);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 2, out hit, 5, interactLayer, QueryTriggerInteraction.Collide))
        {
           

            if (equipment.hasItem && equipment.equipedItem.GetComponent<ConstructionMaterial>())
            {
                if (hit.collider.gameObject.GetComponent<ConstructionStateController>().isBuilt)
                {
                    if (Input.GetButtonDown(GetComponent<ActorUserControl>().playerPrefix + "Apply"))
                    {
                        if (hit.collider.gameObject.GetComponent<Processor>().AddMaterial(equipment.equipedItem))
                        {
                            Debug.Log("Added Matterial");
                            equipment.UnequiptItem(true);
                        }
                    }
                }

                hit.collider.gameObject.GetComponent<ConstructionStateController>().EnableXRay(true);
            }
            if (interact)
            {
                bool build = hit.collider.gameObject.GetComponent<ConstructionStateController>().Construct(equipment.equipedItem);
                {

                    if (build)
                    {
                        equipment.UnequiptItem(true);
                        return;
                    }
                }
                hit.collider.gameObject.GetComponent<InteractionManager>().Interact();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward * 8);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }
}
