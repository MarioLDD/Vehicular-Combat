using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private int min;
    [SerializeField] private int seg;
    private float currentTime;
    public float CurrentTime { get { return currentTime; } }
    private GameObject[] players;
    private int hightScore = 0;
    private GameObject winningPlayer;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        currentTime = (min * 60) + seg;

    }

    // Update is called once per frame
    void Update()
    {

        if (currentTime > 0)
        {
            currentTime -= Time.unscaledDeltaTime;

            if (currentTime < 1)
            {
                foreach (var player in players)
                {
                    int score = player.GetComponent<PlayerManager>().Score;
                    if(score > hightScore)
                    {
                        hightScore = score;
                        winningPlayer = player;
                    }
                }
                Debug.Log("El ganador es " + winningPlayer.name);
            }
        }
    }
}
