using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class DodgeBall : Tool {

    public bool hasOwner = false;
    public Throw ownerThrow;
    public Teams team = Teams.None;
    TrailRenderer trail;
    Material ballMat;
    Renderer ballRenderer;
    public GameObject character = null;
    float origiMoveSpeed;
    Vector3 origiPos;
    PlayersManager playersManager;
    public Material redTeamMat;
    public Material blueTeamMat;
  
    private void Start()
    {
        playersManager = GameObject.FindWithTag("GameController").GetComponent<PlayersManager>();
        trail = GetComponent<TrailRenderer>();
        ballRenderer = GetComponent<MeshRenderer>();
        ballMat = ballRenderer.material;
    }


    private void Update()
    {
        if(!hasOwner && ballRenderer.material!= ballMat)
        {

        }
        if(character)
        {
            if(ballRenderer != character.GetComponent<ActorUserControl>())
            {
                ballRenderer.material = character.transform.Find("CharacterBody").GetComponent<MeshRenderer>().material;
            }
            if(character.GetComponent<ActorEquipment>().hasItem && character.GetComponent<ActorEquipment>().equipedItem != this.gameObject && hasOwner)
            {
                hasOwner = false;
            }
        } 

        foreach(ActorUserControl player in playersManager.playerList)
        {
            if(playerNumber == player.PlayerNum)
            {
                character = player.gameObject;
                ballRenderer.material = character.transform.Find("CharacterBody").GetComponent<MeshRenderer>().material;
                if(playersManager.GetComponent<GameController>().teams)
                {
                    if (player.team == Teams.Red)
                    {
                        trail.material = redTeamMat;

                    }
                    else
                    {
                        trail.material = blueTeamMat;
                    }
                }
                else
                {
                    trail.material = ballRenderer.material;
                }
                
            }
        }

        if (playerNumber == PlayerNumber.Other)
        {
            ballRenderer.material = ballMat;
            trail.material = ballMat;
        }

    }

    public override void OnEquipt(GameObject _character)
    {
        this.character = _character;
        origiMoveSpeed = _character.GetComponent<ActorCharacter>().m_MoveSpeedMultiplier;
        base.OnEquipt(character);
        ballRenderer.material = _character.transform.Find("CharacterBody").GetComponent<MeshRenderer>().material;
        trail.material = ballRenderer.material;
        hasOwner = true;
        ownerThrow = _character.GetComponent<Throw>();

        ownerThrow.thrownObject = GetComponent<Rigidbody>();
        
        team = this.character.GetComponent<ActorUserControl>().team;
    }

    public override void OnUnequipt()
    {
        base.OnUnequipt();
    }

    private void OnCollisionEnter(Collision collision)
    {
       
       
        if (collision.collider.gameObject.GetComponent<HealthManager>())
        {
            if (collision.collider.gameObject.GetComponent<ActorUserControl>().PlayerNum != playerNumber && playerNumber != PlayerNumber.Other)
            {
                if(collision.collider.GetComponent<ActorUserControl>().team != team || team == Teams.None)
                {
                collision.collider.GetComponent<HealthManager>().TakeDamage(100, playerNumber);
                }
                
            }
        }
    }
}
