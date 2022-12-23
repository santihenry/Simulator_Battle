using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public bool startBattle;
    public int cantA;
    public int cantB;

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
    }





}
