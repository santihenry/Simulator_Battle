using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckWinnerTeam : MonoBehaviour
{
    public TextMeshProUGUI winText;
    public int blueMembers, redMembers;
    public ParticleSystem particles;
    public static CheckWinnerTeam instance;

    void Awake()
    {
        instance = this;
        particles.Stop();
    }


    GUIStyle style;
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
            GUI.Label(new Rect(Screen.width / 2, 50, 200, 50), (blueMembers > redMembers)  ? "BLUE WIN" : "RED WIN", style);
            particles.Play();
            BattleManager.Instance.finishBattle = true;
        }

        if (BattleManager.Instance.finishBattle)
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 200, 50), "REPLAY"))
            {
                BattleManager.Instance.Replay();
            }
        }


    }
}
