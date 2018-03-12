using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour {

    public int health;              //The current value of health
    public int maxHealth;           //The maximum health value
    Manager manager;                //A ref to the manager component
    UnityEvent m_zeroHitPoints;     //The event that is triggered when hitpoints are 0 or less
    public GameObject hitParticle;
    public Transform effectTrans;
    public PlayerNumber lastAttacker = PlayerNumber.Other;
    private void Awake()
    {
        manager = GetComponent<Manager>();
    }

    private void Start()
    {
        InitiateHealth();
        InitiateEvents();

        m_zeroHitPoints.AddListener(manager.KillObject);
    }

    private void InitiateEvents()
    {
        if (m_zeroHitPoints == null)
        {
            m_zeroHitPoints = new UnityEvent();
        }
    }

    public void InitiateHealth()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Die()
    {
        if (health <= 0)
        {
            AudioSource audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>().killSplat;
            audio.Play();
            AudioSource audio3 = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>().splat;
            audio3.Play();
            AudioSource audio2 = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>().croudChear;
            audio2.Play();
            health = 0;
            if(hitParticle != null)
            {
                Instantiate(hitParticle, transform.position,transform.rotation,null);
            }
            m_zeroHitPoints.Invoke();
        }
    }
    public void TakeDamage(int damage, PlayerNumber attacker)
    {
        health -= damage;
        lastAttacker = attacker;
    }

    public void Heal()
    {
        health = maxHealth;
    }

    private void LateUpdate()
    {
        Die();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("hit");

    //    if(collision.collider.gameObject.tag == "Tool")
    //    {
    //        if(collision.collider.gameObject.GetComponent<Tool>().isAttacking)
    //        {
    //                            TakeDamage(3);
    //        }
    //    }
    //}
}
