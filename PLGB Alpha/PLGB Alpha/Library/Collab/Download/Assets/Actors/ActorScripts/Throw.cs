using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour {

    ActorEquipment actorEquipment;
    public float forceModifier = 1;
    [Range(0,1)]public float upwardsModifier = 1;
    public ForceMode forceMode = ForceMode.Force;
    float timer = 0;
    public float minThrowForce = .04f;
    public Rigidbody thrownObject;

    private void Awake()
    {
        actorEquipment = GetComponent<ActorEquipment>();
    }

    private void Update()
    {
        if (actorEquipment.hasItem)
        {
            if (thrownObject.GetComponent<DodgeBall>())
            {
                thrownObject.GetComponent<DodgeBall>().hasOwner = true;
            }
        }

        if(thrownObject == null)
        {
            GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Tool");
            foreach(GameObject ball in allBalls)
            {
                if(ball.GetComponent<DodgeBall>() && ball.GetComponent<DodgeBall>().playerNumber == GetComponent<ActorUserControl>().PlayerNum)
                {
                    thrownObject = null;
                    thrownObject = ball.GetComponent<Rigidbody>();
                }
            }
        }
    }

    public void ThrowAction(bool input)
    {
        if(thrownObject)
        {
            if(thrownObject.GetComponent<Tool>().playerNumber == GetComponent<ActorUserControl>().PlayerNum)
            {
                if (input)
                {
                    timer += Time.deltaTime * 1;
                    
                }
                else if (timer != 0)
                {
                    float timeModifier;
                    if (timer < 1)
                    {
                        if (timer < minThrowForce)
                        {
                            timeModifier = minThrowForce;
                        }
                        else
                        {
                            timeModifier = timer;
                        }
                    }
                    else
                    {
                        timeModifier = 1;
                    }

                    if (actorEquipment.hasItem && actorEquipment.equipedItem.gameObject == thrownObject.gameObject)
                    {
                        AudioSource audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>().whoosh;
                        audio.Play();
                        actorEquipment.UnequiptItem();
                        thrownObject.AddForce((transform.forward + (transform.up * upwardsModifier)) * forceModifier * 2f * timeModifier, forceMode);
                        timer = 0;

                    }
                    else
                    {

                        thrownObject.velocity = thrownObject.velocity * 0.9f;
                        thrownObject.AddForce((transform.forward + (transform.up * upwardsModifier)) * forceModifier * timeModifier, forceMode);
                        timer = 0; 
                    }

                    if (thrownObject.GetComponent<DodgeBall>())
                    {
                        return;
                    }
                    thrownObject = null;
                }
            }
        }     
    }
}
