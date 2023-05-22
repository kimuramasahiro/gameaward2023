using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private GameObject PlayerData;
    private PlayerMovement PlayerStatus;
    public string RetryStage;
    public string NextStage;
    public AudioClip WarpSound;
    public AudioClip RetrySound;
    AudioSource SceneAudioSource;
    private bool OnlyClear;
    public float FadeTime = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        SceneAudioSource = GetComponent<AudioSource>();
        PlayerData = GameObject.Find("Player");
        PlayerStatus = PlayerData.GetComponent<PlayerMovement>();
        OnlyClear = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStatus.IsTouched)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                FadeManager.Instance.LoadScene(RetryStage, 1.0f);
                SceneAudioSource.PlayOneShot(RetrySound);
            }
        }
        if(PlayerStatus.ClearCheck&&!PlayerStatus.IsTouched && !OnlyClear)
        {

            FadeManager.Instance.LoadScene(NextStage,FadeTime);
            SceneAudioSource.PlayOneShot(WarpSound);
            OnlyClear = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FadeManager.Instance.LoadScene("TitleScene", 1.0f);
        }

    }
}
