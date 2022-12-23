using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public bool startBattle;
    public int cantA;
    public int cantB;

    public Animator Red;
    public Animator Blue;

    public bool CanPlay
    {
        get
        {
            return cantA > 0 && cantB > 0; 
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Red.enabled = false;
        Blue.enabled = false;
    }


    public void StartBattle()
    {
        startBattle = true;
        Red.enabled = true;
        Blue.enabled = true;
    }


}
