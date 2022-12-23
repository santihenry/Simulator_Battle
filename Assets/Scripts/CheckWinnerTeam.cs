using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckWinnerTeam : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI winText;
    public int blueMembers, redMembers;
    public ParticleSystem particles;
    public static CheckWinnerTeam instance;

    void Awake()
    {
        instance = this;
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (!BattleManager.Instance.startBattle) return;

        if (blueMembers <= 0)
        {
            winText.text = "RED TEAM WINS";
            winText.color = Color.red;
            particles.Play();
        }
        if (redMembers <= 0)
        {
            winText.text = "BLUE TEAM WINS";
            winText.color = Color.blue;
            particles.Play();
        }
    }

    GUIStyle style;
    [Min(30)]public int fontSize;
    private void OnGUI()
    {
        style = new GUIStyle();
        style.fontSize = fontSize;
        style.alignment = TextAnchor.MiddleCenter;
        if (!BattleManager.Instance.startBattle)
        {
            GUI.Label(new Rect(Screen.width / 2, 50, 200, 50), "Blue Team",style);
        }
        if (blueMembers <= 0)
        {
            style.normal.textColor = Color.blue;
            GUI.Label(new Rect(Screen.width / 2, 50, 200, 50), "Blue Team",style);
        }
        if (redMembers <= 0)
        {
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2, 50, 200, 50), "Blue Team", style);
        }


    }
}
