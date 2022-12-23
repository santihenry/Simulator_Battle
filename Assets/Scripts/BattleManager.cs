using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public bool startBattle;
    public bool finishBattle;
    public int cantA;
    public int cantB;

    public Animator Red;
    public Animator Blue;

    public float closeDoorDelay;

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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void StartBattle()
    {
        startBattle = true;
        Invoke("CloseDoor", closeDoorDelay);
    }

    void CloseDoor()
    {
        Red.enabled = true;
        Blue.enabled = true;
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }



}
