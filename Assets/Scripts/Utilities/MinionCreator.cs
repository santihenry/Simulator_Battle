using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionCreator : MonoBehaviour
{
    public EnemyMinion prefab;
    public List<EnemyMinion> enemyList = new List<EnemyMinion>();
    public EnemyMinion lastCreated;
    public int index=0;
    public int color;
    public string[] team = new string[2] { "blue", "red" };
    public GameObject lead;

    public Vector3[] bluePositions = new Vector3[10]
    {
        new Vector3(27.874f,0,59.4f),
        new Vector3(24.916f,0,59.4f),
        new Vector3(21.958f,0,59.4f),
        new Vector3(19f,0,59.4f),
        new Vector3(16.042f,0,59.4f),

        new Vector3(27.874f,0,62.15f),
        new Vector3(24.916f,0,62.15f),
        new Vector3(21.958f,0,62.15f),
        new Vector3(19f,0,62.15f),
        new Vector3(16.042f,0,62.15f),
    };
    public Vector3[] redPositions = new Vector3[10]
    {
        new Vector3(16,0,-10.3f),
        new Vector3(18.958f,0,-10.3f),
        new Vector3(21.916f,0,-10.3f),
        new Vector3(24.874f,0,-10.3f),
        new Vector3(27.832f,0,-10.3f),

        new Vector3(16,0,-13.05f),
        new Vector3(18.958f,0,-13.05f),
        new Vector3(21.916f,0,-13.05f),
        new Vector3(24.874f,0,-13.05f),
        new Vector3(27.832f,0,-13.05f),
    };



    public void RemoveNPC(int c)
    {
        enemyList.RemoveAt(enemyList.Count - 1);
        DestroyImmediate(lastCreated.gameObject, true);
        if (enemyList.Count > 0)
            lastCreated = enemyList[enemyList.Count - 1];
        if (index > 0) index--;
    }

    public void AddNPC(int c)
    {
        var position = c == 0 ? bluePositions : redPositions;
        lastCreated = Instantiate(prefab, position[index], transform.rotation);
        lastCreated.GetComponent<Flocking>().leaderToFollow = gameObject.transform;
        lastCreated.team = (EnemyTeam)color;
        enemyList.Add(lastCreated);
        if (index < 10) index++;
    }


    public bool gui;
    GUIStyle styleBlue;
    GUIStyle style;
    GUIStyle styleTitle;
    private void OnGUI()
    {
        if (!gui) return;
        if (BattleManager.Instance.startBattle) return;

        styleTitle = new GUIStyle();
        styleTitle.normal.textColor = Color.black;
        styleTitle.fontSize = 30;

        GUI.Label(new Rect(5, 5, 120, 30), "BattleSettings",styleTitle);

        style = new GUIStyle();
        style.fontSize = 20;

        if(color == 0)
        {
            style.normal.textColor = Color.blue;

            GUI.Label(new Rect(5, 40, 120, 30), "Blue Team", style);

            if (BattleManager.Instance.cantA < 10)
                if (GUI.Button(new Rect(5, 70, 120, 20), "Add"))
                {
                    var position = color == 0 ? bluePositions : redPositions;
                    lastCreated = Instantiate(prefab, position[index], transform.rotation);
                    lastCreated.GetComponent<Flocking>().leaderToFollow = lead.transform;
                    lastCreated.team = (EnemyTeam)color;
                    enemyList.Add(lastCreated);
                    if (index < 10) index++;

                    CheckWinnerTeam.instance.blueMembers += 1;
                    BattleManager.Instance.cantA = enemyList.Count;
                }

            if (BattleManager.Instance.cantA > 0)
                if (GUI.Button(new Rect(5, 95, 120, 20), "Remove"))
                {
                    enemyList.RemoveAt(enemyList.Count - 1);
                    DestroyImmediate(lastCreated.gameObject, true);
                    if (enemyList.Count > 0)
                        lastCreated = enemyList[enemyList.Count - 1];
                    if (index > 0) index--;

                    CheckWinnerTeam.instance.blueMembers -= 1;
                    BattleManager.Instance.cantA = enemyList.Count;
                }
        }
        else
        {
            style.normal.textColor = Color.red;

            GUI.Label(new Rect(5, 130, 120, 30), "Red Team", style);

            if (BattleManager.Instance.cantB < 10)
                if (GUI.Button(new Rect(5, 160, 120, 20), "Add"))
                {
                    var position = color == 0 ? bluePositions : redPositions;
                    lastCreated = Instantiate(prefab, position[index], transform.rotation);
                    lastCreated.GetComponent<Flocking>().leaderToFollow = lead.transform;
                    lastCreated.team = (EnemyTeam)color;
                    enemyList.Add(lastCreated);
                    if (index < 10) index++;
                    CheckWinnerTeam.instance.redMembers += 1;
                    BattleManager.Instance.cantB = enemyList.Count;
                }


            if (BattleManager.Instance.cantB > 0)
                if (GUI.Button(new Rect(5, 185, 120, 20), "Remove"))
                {
                    enemyList.RemoveAt(enemyList.Count - 1);
                    DestroyImmediate(lastCreated.gameObject, true);
                    if (enemyList.Count > 0)
                        lastCreated = enemyList[enemyList.Count - 1];
                    if (index > 0) index--;

                    CheckWinnerTeam.instance.redMembers -= 1;
                    BattleManager.Instance.cantB = enemyList.Count;
                }
        }


        if (BattleManager.Instance.CanPlay)
        {
            if (GUI.Button(new Rect(5, 230, 120, 30), "Start Battle"))
            {
                BattleManager.Instance.StartBattle();// = true;
            }
        }
    }


}
