using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange1 : MonoBehaviour
{
    private GameObject PlayerData;
    private PlayerMovement PlayerStatus;
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
                FadeManager.Instance.LoadScene("TitleScene", 1.0f);
            }
        }
        if(PlayerStatus.ClearCheck)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                FadeManager.Instance.LoadScene("TitleScene", 1.0f);
            }
        }

    }
}
