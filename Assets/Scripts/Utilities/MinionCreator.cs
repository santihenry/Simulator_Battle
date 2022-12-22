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
       
}
