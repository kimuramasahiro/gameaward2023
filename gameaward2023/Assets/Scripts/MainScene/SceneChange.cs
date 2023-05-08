﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private GameObject PlayerData;
    private PlayerMovement PlayerStatus;
    public string RetryStage;
    public string NextStage;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GameObject.Find("Player");
        PlayerStatus = PlayerData.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStatus.IsTouched)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                FadeManager.Instance.LoadScene(RetryStage, 1.0f);
            }
        }
        if(PlayerStatus.ClearCheck&&!PlayerStatus.IsTouched)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                FadeManager.Instance.LoadScene(NextStage, 1.0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FadeManager.Instance.LoadScene("TitleScene", 1.0f);
        }

    }
}
