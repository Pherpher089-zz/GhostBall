using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public virtual void KillObject()
    {
        GameObject.Destroy(this.gameObject);
    }
}
