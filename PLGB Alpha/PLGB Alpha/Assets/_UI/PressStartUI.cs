using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PressStartUI : MonoBehaviour {

    public TextMeshProUGUI p1Text;
    public TextMeshProUGUI p2Text;
    public TextMeshProUGUI p3Text;
    public TextMeshProUGUI p4Text;
    public string pressStart;
    public string playerReady;
    ActivePlayersManager activePlayersManager;

    void Awake()
    {
        activePlayersManager = FindObjectOfType(typeof (ActivePlayersManager)) as ActivePlayersManager;
    }

    void Update()
    {
        if(activePlayersManager.activePlayers.Player_1_Active)
        {
            p1Text.text = playerReady;
        }
        else
        {
            p1Text.text = pressStart;
        }

        if (activePlayersManager.activePlayers.Player_2_Active)
        {
            p2Text.text = playerReady;
        }
        else
        {
            p2Text.text = pressStart;
        }

        if (activePlayersManager.activePlayers.Player_3_Active)
        {
            p3Text.text = playerReady;
        }
        else
        {
            p3Text.text = pressStart;
        }

        if (activePlayersManager.activePlayers.Player_4_Active)
        {
            p4Text.text = playerReady;
        }
        else
        {
            p4Text.text = pressStart;
        }
    }
}
