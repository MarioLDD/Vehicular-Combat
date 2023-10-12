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
    [SerializeField] private GameObject menu;
    void Start()
    {
        Cursor.visible = false;
        menu.SetActive(false);
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
                if (winningPlayer == null)
                {
                    foreach (var player in players)
                    {
                        int score = player.GetComponent<PlayerManager>().Score;
                        if (score > hightScore)
                        {
                            hightScore = score;
                            winningPlayer = player;
                        }
                    }
                    winningPlayer.transform.Find("HUD_Canvas/Victory_Panel").gameObject.SetActive(true);
                    menu.SetActive(true);
                    Time.timeScale = 0;
                    Debug.Log("El ganador es " + winningPlayer.name);
                }
            }
        }





        //lo siguiente funcionaba pero tenia un error
        //if (currentTime > 0)
        //{
        //    currentTime -= Time.unscaledDeltaTime;

        //    if (currentTime < 1)
        //    {
        //        foreach (var player in players)
        //        {
        //            int score = player.GetComponent<PlayerManager>().Score;
        //            if (score > hightScore)
        //            {
        //                hightScore = score;
        //                winningPlayer = player;
        //                player.transform.Find("HUD_Canvas/Victory_Panel").gameObject.SetActive(true);
        //                menu.SetActive(true);
        //                Time.timeScale = 0;
        //            }
        //        }
        //        Debug.Log("El ganador es " + winningPlayer.name);
        //    }
        //}
    }

    private void ShowMenu()
    {

    }

}