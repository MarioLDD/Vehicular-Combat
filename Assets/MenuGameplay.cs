using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameplay : MonoBehaviour
{
    private bool start = false;
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Exit()
    {
        Application.Quit();
    }
    // Start is called before the first frame update
    //void Start()
    //{
    //    if (!start)
    //    {
    //        start = true;
    //        gameObject.SetActive(false);
    //    }
    //}
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
