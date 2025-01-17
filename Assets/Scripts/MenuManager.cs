using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu_Panel;
    [SerializeField] private GameObject selectPlayer_Panel;

    private void Start()
    {
        mainMenu_Panel.SetActive(true);
        selectPlayer_Panel.SetActive(false);
    }
    public void SelectPlayer()
    {
        mainMenu_Panel.SetActive(false);
        selectPlayer_Panel.SetActive(true);
    }

    public void StarGame()
    {
        SceneManager.LoadScene("CombatArenaOld");
    }

    public void TwoPlayer()
    {
        SceneManager.LoadScene("TwoPlayerCombatArena");
    }
    public void FourPlayer()
    {
        SceneManager.LoadScene("FourPlayerCombatArena");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
