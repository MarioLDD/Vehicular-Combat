using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
[RequireComponent(typeof(Rigidbody), typeof(UserInput), typeof(ShieldSystem))]
[RequireComponent(typeof(HealthSystem), typeof(TimeDisplay), typeof(PlayerInput))]

public class PlayerManager : MonoBehaviour
{
    private Camera cameraPlayer;
    private GameObject player;
    private Weapon weapon;
    public Weapon Weapon { get { return weapon; } }


    [SerializeField] private int score;
    public int Score { get { return score; } set { score = value; } }
    [SerializeField] TMP_Text scoreText;
    // Start is called before the first frame update

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
    }
    void Start()
    {
        player = this.gameObject;
        cameraPlayer = GetComponentInChildren<Camera>();
        cameraPlayer.transform.SetParent(null);

        scoreText.text = "Score: " + score;
    }
    public void WinScore(int addScore)
    {
        score = score + addScore;
        scoreText.text = "Score: " + score;
        //Debug.Log("suma");
    }
    public void LoseScore(GameObject killerPlayer)
    {
        //int subtractScore = 0;
        //if (score > 0)

        int subtractScore = score / 2;

        score = score - subtractScore;
        if (killerPlayer.TryGetComponent(out PlayerManager playerManager))
        {

            playerManager.WinScore(subtractScore + 500);
        }
        scoreText.text = "Score: " + score;
    }

    private void OnEnable()
    {
        cameraPlayer.gameObject.SetActive(true);

    }
    private void OnDisable()
    {
        cameraPlayer.gameObject.SetActive(false);
    }
}
