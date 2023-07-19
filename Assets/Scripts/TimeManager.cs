using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject timePanel;
    [SerializeField] private int min;
    [SerializeField] private int seg;
    
    void Start()
    {
        StartCoroutine(StartTime());
    }

    // Update is called once per frame
    void Update()
    {

    }


    public IEnumerator StartTime()
    {
        timePanel.SetActive(true);

        float currentTime = (min * 60) + seg;

        while (currentTime > 0)
        {
            currentTime -= Time.unscaledDeltaTime;

            if (currentTime < 1)
            {
                Debug.Log("Se acabo el tiempo");               
            }

            int tempMin = Mathf.FloorToInt(currentTime / 60);
            int tempSeg = Mathf.FloorToInt(currentTime % 60);
            timeText.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
            yield return null;
        }
    }
}