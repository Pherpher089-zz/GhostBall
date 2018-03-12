using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameController))]
public class PointTracker : MonoBehaviour {

    GameController gameController;
    ActivePlayersManager m_activePlayersManager;
    public GUISkin m_player1Skin, m_player2Skin, m_player3Skin, m_player4Skin;
    private void Awake()
    {
        gameController = GetComponent<GameController>();
        m_activePlayersManager = GetComponent<ActivePlayersManager>();
    }

    void OnGUI()
    {
        if(!PauseControl.isPaused && gameController.pointTracking)
        {
            GUI.skin = m_player1Skin;
            string points;
            if (m_activePlayersManager.activePlayers.Player_1_Active)
            {
                points = string.Format(PlayerNumber.Player_1.ToString() + ":    {0:0}", gameController.PlayerPoints[0]);
                GUI.TextField(new Rect(30, 30, 120, 25), points);
            }

            GUI.skin = m_player2Skin;
            if (m_activePlayersManager.activePlayers.Player_2_Active)
            {
                points = string.Format(PlayerNumber.Player_2.ToString() + ":    {0:0}", gameController.PlayerPoints[1]);
                GUI.TextField(new Rect(30, 60, 120, 25), points);
            }

            GUI.skin = m_player3Skin;
            if (m_activePlayersManager.activePlayers.Player_3_Active)
            {
                points = string.Format(PlayerNumber.Player_3.ToString() + ":    {0:0}", gameController.PlayerPoints[2]);
                GUI.TextField(new Rect(30, 90, 120, 25), points);
            }

            GUI.skin = m_player4Skin;
            if (m_activePlayersManager.activePlayers.Player_4_Active)
            {
                points = string.Format(PlayerNumber.Player_4.ToString() + ":    {0:0}", gameController.PlayerPoints[3]);
                GUI.TextField(new Rect(30, 120, 120, 25), points);
            }
        }
    }
}
