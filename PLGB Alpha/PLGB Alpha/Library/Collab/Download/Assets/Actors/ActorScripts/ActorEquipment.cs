using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorEquipment : MonoBehaviour {
    public GameObject equipedItem;
    public bool hasItem;
    public float grabDistance = 4;
    private GameObject newItem;
    public Transform socket;
    private List<GameObject> grabableItems = new List<GameObject>();

    public void Start()
    {
        hasItem = false;
        socket = transform.Find("HandSocket");

        if (equipedItem)
        {
            GameObject newItem = Instantiate(equipedItem, null);
            EquipItem(newItem);
        }   
    }

    public void EquipItem(GameObject item)
    {
        AudioSource audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>().ballBaounce;
        audio.Play();
        hasItem = true;
        equipedItem = item;
        equipedItem.transform.position = socket.position;
        equipedItem.transform.rotation = socket.rotation;
        equipedItem.transform.parent = socket;
        equipedItem.GetComponent<Item>().OnEquipt(this.gameObject);
    }

    public void UnequiptItem()
    {
        
        hasItem = false;
        equipedItem.GetComponent<Item>().OnUnequipt();
        equipedItem.transform.parent = null;
        equipedItem = null;
    }

    public void UnequiptItem(bool spendItem)
    {
        hasItem = false;
        equipedItem.GetComponent<Item>().OnUnequipt();
        Destroy(equipedItem.gameObject);

    }

    public void SpendItem()
    {
        UnequiptItem(true);
    }

    /// <summary>
    /// Finds all not equiped items in the sceene that are close enough to the player to grab and
    /// adds them to the grabableItems list. This funtion also returns the closest
    /// </summary>
    GameObject GatherAllItemsInScene()
    {
        Item[] allItems = GameObject.FindObjectsOfType<Item>();
        GameObject closestItem = null;
        float closestDist = 5;

        foreach (Item item in allItems)
        {
            if(!item.isEquiped)
            {
                float currentItemDist =  Vector3.Distance(transform.position, item.gameObject.transform.position);

                if (currentItemDist < grabDistance)
                {
                    if(currentItemDist < closestDist)
                    {
                        //TODO check for player direction as well to stop players from picking up unintended items

                        closestDist = currentItemDist;
                        closestItem = item.gameObject;
                    }

                    grabableItems.Add(item.gameObject);//TODO a list?
                }
            }
        }

        if(grabableItems.Count <=0)
            return null;
        else
            return closestItem;
    }

    public bool GrabItem()
    {
        newItem = GatherAllItemsInScene();

        if (hasItem)
        {
            UnequiptItem();
        }

        if (newItem != null)
        {
            EquipItem(newItem);
            hasItem = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GrabItem(GameObject item)
    {
        if (hasItem)
        {
            UnequiptItem();
        }

        if (item != null)
        {
            EquipItem(item);
            hasItem = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
