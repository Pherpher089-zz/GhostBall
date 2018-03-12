using UnityEngine;
using System.Collections;

public enum ConstructionMaterialType{Log, ChoppedLog, Plank, Iron}

public class ConstructionMaterial : Item
{
    public ConstructionMaterialType conMatType;

    public override void OnEquipt(GameObject character)
    {
        base.OnEquipt(character);
    }

    public override void OnUnequipt()
    {
        base.OnUnequipt();
    }
}
