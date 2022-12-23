using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckWinnerTeam : MonoBehaviour
{
    public int blueMembers, redMembers;
    public static CheckWinnerTeam instance;

    void Awake()
    {
        instance = this;
    }

    GUIStyle style;
    GUIStyle style2;
    [Min(30)]public int fontSize;
    private void OnGUI()
    {
        style = new GUIStyle();
        style.fontSize = fontSize;
        style.alignment = TextAnchor.LowerCenter;
        style.fontStyle = FontStyle.Bold;        

        if (blueMembers <= 0 || redMembers <= 0)
        {
            style.normal.textColor = (blueMembers > redMembers) ? Color.blue : Color.red;
            GUI.Label(new Rect(Screen.width / 2, 100, 200, 50), (blueMembers > redMembers)  ? "BLUE WIN" : "RED WIN", style);
            BattleManager.Instance.finishBattle = true;
        }

        if (BattleManager.Instance.finishBattle)
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 200, 50), "REPLAY"))
            {
                BattleManager.Instance.Replay();
            }

            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 55, 200, 50), "EXIT"))
            {
                BattleManager.Instance.QuitGame();
            }
        }

        style2 = new GUIStyle();
        style2.fontSize = 45;
        style2.alignment = TextAnchor.MiddleCenter;
        style2.fontStyle = FontStyle.Bold;
        style2.normal.textColor = Color.white;

        GUI.Label(new Rect(0, 10, Screen.width, 30), $"BLUE: {blueMembers} | RED: {redMembers}", style2);

    }
}
