using UnityEngine;


[RequireComponent (typeof (HealthManager))]
[RequireComponent (typeof (Collider))]
[RequireComponent(typeof(ActorCharacter))]

public class ActorManager : Manager {

    public Transform actorEyes;
    PlayersManager playersManager;
    HealthManager healthManager;
    GameObject deathEffectPrefab;
    GameController gameController;

    CamShake camShake;
    public void Awake()
    {
        actorEyes = transform.Find("Eyes");
        playersManager = GameObject.FindWithTag("GameController").GetComponent<PlayersManager>();

        deathEffectPrefab = Resources.Load("DeathEffect") as GameObject;
        healthManager = GetComponent<HealthManager>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        camShake = Camera.main.GetComponent<CamShake>();
    }

    public override void KillObject()
    {
        if(gameController.gameMode == GameMode.FirstTo5)
        {
            gameController.AddPoints(healthManager.lastAttacker);
        }
        else if(gameController.gameMode == GameMode.TeamDeathMatch)
        {
            gameController.AddPoints(healthManager.lastAttacker);
            gameController.deadCharacterTeam = GetComponent<ActorUserControl>().team;
        }

        playersManager.HandleActorDeth(GetComponent<ActorUserControl>());
        Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        camShake.DoShake(0.3f);
        if(GetComponent<ActorEquipment>() && GetComponent<ActorEquipment>().hasItem)
        {
            GetComponent<ActorEquipment>().UnequiptItem();
        }
        
        base.KillObject();

    }
}
