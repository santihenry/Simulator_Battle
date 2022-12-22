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
}
