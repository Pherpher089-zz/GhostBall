using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionStateController : MonoBehaviour {

    [SerializeField] List<GameObject> unbuiltChilderen;
    public bool isBuilt;
    PlayersManager playersManager;
    Collider col;

    float xrayCounter = 0;
    private void Awake()
    {
        playersManager = GameObject.FindWithTag("GameController").GetComponent<PlayersManager>();
        col = GetComponent<Collider>();
    }

    private void Start()
    {
        GatherChilderen();
        UnbuildAll();
    }

    private void Update()
    {
        if(xrayCounter <= 0)
        {
            EnableXRay(false);
        }
        else
        {
            xrayCounter -= Time.deltaTime;
        }
    }

    private void UnbuildAll()
    {
        foreach (GameObject child in unbuiltChilderen)
        {
            child.GetComponent<Collider>().enabled = false;
            child.GetComponent<ConstructionPiece>().SwapXrayMaterial(true);
        }
    }

    private void GatherChilderen()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            if(transform.GetChild(i).gameObject.tag != "Socket")
            {
                unbuiltChilderen.Add(transform.GetChild(i).gameObject);

            }
        }
    }

    public bool Construct(GameObject material)
    {
        Debug.Log("In the building");
        if(!isBuilt)
        {
            for (int i = 0; i < unbuiltChilderen.Count; i++)
            {
                if (material.GetComponent<ConstructionMaterial>().conMatType == unbuiltChilderen[i].GetComponent<ConstructionPiece>().RequiredType)
                {
                    BuildChild(unbuiltChilderen[i]);
                    i = unbuiltChilderen.Count;
                    return true;
                }
            }
        }

        return false;
    }

    private void BuildChild(GameObject child)
    {
        child.GetComponent<Collider>().enabled = true;
        child.GetComponent<ConstructionPiece>().SwapXrayMaterial(false);
        child.GetComponent<HealthManager>().Heal();
        unbuiltChilderen.Remove(child);

        if(unbuiltChilderen.Count == 0)
        {
            isBuilt = true;
        }
        else
        {
            isBuilt = false;
        }
    }

    public void UnbuildChild(GameObject child)
    {
        child.GetComponent<Collider>().enabled = false;
        child.GetComponent<ConstructionPiece>().SwapXrayMaterial(true);
        unbuiltChilderen.Add(child);
    }

    public void EnableXRay(bool on)
    {
        
        if(on)
        {
            xrayCounter = 0.2f;
        }
        foreach (GameObject item in unbuiltChilderen)
        {
            item.GetComponent<MeshRenderer>().enabled = on;
        }
    }
}
