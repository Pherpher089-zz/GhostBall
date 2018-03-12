using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "StateBasedConstruction/ConstructionState")]
public class ConstructionState : ScriptableObject
{
    public Dictionary<GameObject, int> requiredObj = new Dictionary<GameObject, int>();

}
