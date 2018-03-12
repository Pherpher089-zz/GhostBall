using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Processor : MonoBehaviour {

    public Transform addedResourceSocket;
    public List<GameObject> processableObjects;
    public Slider slider;
    public Canvas sliderCanvas;
    PlayersManager playerManager;
    ConstructionStateController constStateCont;
    public GameObject constMat;
    public float processValue = 0;
    public float speed;


    private void Awake()
    {
        playerManager = GameObject.FindWithTag("GameController").GetComponent<PlayersManager>();
        constStateCont = GetComponent<ConstructionStateController>();

        sliderCanvas = addedResourceSocket.GetChild(0).gameObject.GetComponent<Canvas>();
        slider = sliderCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value = Processing();
       if( Processing() > 1)
       {
            
       }
    }

    public bool AddMaterial(GameObject mat)
    {
        if(constMat == null)
        {
            constMat = Instantiate(mat, addedResourceSocket);
            constMat.GetComponent<Rigidbody>().isKinematic = true;
            constMat.GetComponent<Collider>().isTrigger = true;
            return true;
        }

        return false;
    }

    private float Processing()
    {

        return processValue = 0;
    }
}
