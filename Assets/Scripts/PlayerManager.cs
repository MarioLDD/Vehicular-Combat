using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
[RequireComponent(typeof(Rigidbody), typeof(UserInput), typeof(ShieldSystem))]
[RequireComponent(typeof(HealthSystem), typeof(TimeDisplay), typeof(PlayerInput))]
[RequireComponent(typeof(FlamesSystem))]

public class PlayerManager : MonoBehaviour
{
    private Camera cameraPlayer;
    private GameObject player;
    private Weapon weapon;
    private GameObject flamesPoint;
    public Weapon Weapon { get { return weapon; } }
    public GameObject FlamesPoint { get { return flamesPoint; } }


    [SerializeField] private int score;
    public int Score { get { return score; } set { score = value; } }
    [SerializeField] TMP_Text scoreText;
    // Start is called before the first frame update

    private void Awake()
    {
        player = this.gameObject;

        weapon = GetComponentInChildren<Weapon>();

        for (int i = 0; i < player.transform.childCount; i++)
        {
            Transform hijo = player.transform.GetChild(i);

            // Compara el nombre del hijo con el nombre que estás buscando
            if (hijo.name == "FlamesPoint")
            {
                // Has encontrado el hijo, puedes acceder a él.
                flamesPoint = hijo.gameObject;
                // Realiza las operaciones que necesites con 'hijoGameObject'.
                break; // Puedes salir del bucle una vez que encuentres el hijo.
            }
        }
    }
    void Start()
    {
        cameraPlayer = GetComponentInChildren<Camera>();
        cameraPlayer.transform.SetParent(null);

        UpdateScoreText();
    }
    public void WinScore(int addScore)
    {
        score += addScore;
        UpdateScoreText();
        //Debug.Log("suma");
    }
    public void LoseScore(GameObject killerPlayer)
    {
        //int subtractScore = 0;
        //if (score > 0)

        int subtractScore = score / 2;
        score -= subtractScore;

        if (killerPlayer.TryGetComponent(out PlayerManager playerManager))
        {
            playerManager.WinScore(subtractScore + 500);
        }
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
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
