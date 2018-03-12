using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : MonoBehaviour {

    public Throw throwRef;
    public ActorEquipment actorEquipment;
    public bool isReturning;
    public float zipSpeed = 10;
    float counter = 0;
    LineRenderer line;
    void Awake()
    {
        line = GetComponent<LineRenderer>();
        throwRef = GetComponent<Throw>();
        actorEquipment = GetComponent<ActorEquipment>();
    }

    public void ReturnBall(bool input)
    {
        isReturning = input;
        if (actorEquipment.hasItem == false)
        {
            if (input && throwRef.thrownObject != null && throwRef.thrownObject.GetComponent<DodgeBall>().ownerThrow == throwRef)
            {
                RaycastHit hit;
                Vector3 dir = throwRef.thrownObject.position - transform.position;
                Ray ray = new Ray(transform.position + transform.forward, dir);
                line.SetPosition(0, transform.position + new Vector3(0, 1, 1));
                line.SetPosition(1, throwRef.thrownObject.position);
                if (Physics.Raycast(ray, out hit, 100000))
                {
                    if (hit.collider.gameObject == throwRef.thrownObject.gameObject)
                    {
                        counter += Time.deltaTime;
                        throwRef.thrownObject.velocity = throwRef.thrownObject.velocity * 0.8f;
                       
                        dir = dir.normalized;
                        throwRef.thrownObject.velocity += -dir * counter * zipSpeed;
                        return;
                    }
                } 
            }
            else
            {

                counter = 0;
            }
        }
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.gameObject == throwRef.thrownObject.gameObject && isReturning == true)
        //{
        //    actorEquipment.EquipItem(throwRef.thrownObject.gameObject);
        //}
    }
}
