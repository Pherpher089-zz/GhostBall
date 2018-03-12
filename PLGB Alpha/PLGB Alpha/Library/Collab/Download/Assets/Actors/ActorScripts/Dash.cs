using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour
{

    public Vector3 moveDirection;
    public float maxDashTime = 1.0f;
    public float dashSpeed = 1.0f;
    public float dashStoppingSpeed = 0.1f;
    ActorCharacter actorCharacter;
    

    private float currentDashTime;

    void Awake()
    {
        actorCharacter = GetComponent<ActorCharacter>();
    }
    void Start()
    {
        currentDashTime = maxDashTime;
    }

    public void DashAction(bool dash)
    {
        if (dash)
        {
            AudioSource audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>().enchant;
            audio.Play();
            currentDashTime = 0.0f;
        }
        if (currentDashTime < maxDashTime)
        {
            //moveDirection = new Vector3(0, 0, dashSpeed);
            GetComponent<Rigidbody>().AddForce(transform.forward * dashSpeed * 100, ForceMode.VelocityChange);
            currentDashTime += dashStoppingSpeed;
        }
        //else
        //{
        //    moveDirection = Vector3.zero;
        //}
        //actorCharacter.Move(moveDirection * Time.deltaTime, false, false);
    }
}
