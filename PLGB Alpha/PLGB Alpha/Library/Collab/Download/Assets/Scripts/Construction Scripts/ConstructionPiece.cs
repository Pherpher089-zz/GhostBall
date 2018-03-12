using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HealthManager))]

public class ConstructionPiece : Manager
{
    public ConstructionMaterialType RequiredType;
    ConstructionStateController controller;
    MeshRenderer meshRender;
    Material currentMat;
    Material xRayMat;

    private void Awake()
    {
        controller = transform.parent.GetComponent<ConstructionStateController>();
        meshRender = GetComponent<MeshRenderer>();
        currentMat = meshRender.material;
        xRayMat = Resources.Load("Build_xRay_mat") as Material;
    }

    public void SwapXrayMaterial(bool xRay)
    {
        if(xRay)
        {
            meshRender.material = xRayMat;
        }
        else
        {
            meshRender.material = currentMat;
        }
    }

    public override void KillObject()
    {
        controller.UnbuildChild(this.gameObject);
    }
}
