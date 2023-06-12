using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneChange : MonoBehaviour
{
    private GameObject PlayerData;
    private PlayerMovement PlayerStatus;
    public string RetryStage;
    public string NextStage;
    public AudioClip WarpSound;
    public AudioClip FailedSound;
    public AudioClip RetrySound;
    AudioSource SceneAudioSource;
    private bool OnlyClear;
    private bool OnlyFailed;
    private bool OnlyCheck;
    public float FadeTime = 1.5f;
    // Start is called before the first frame update
    private Controller _gameInputs;    //gamepad

    void Start()
    {
        SceneAudioSource = GetComponent<AudioSource>();
        PlayerData = GameObject.Find("Player");
        PlayerStatus = PlayerData.GetComponent<PlayerMovement>();

        //Input Actionインスタンス生成
        _gameInputs = new Controller();

        //Actionイベント登録
        _gameInputs.Submit.Submit.performed += OnSelect;

        _gameInputs.Enable();

        OnlyClear = false;
        OnlyFailed = false;
        OnlyCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStatus.IsTouched)
        {
            if (!OnlyFailed)
            {
                SceneAudioSource.PlayOneShot(FailedSound);
                OnlyFailed = true;
            }
        }
        if (PlayerStatus.ClearCheck && !PlayerStatus.IsTouched && !OnlyClear)
        {
            FadeManager.Instance.LoadScene(NextStage, FadeTime);
            SceneAudioSource.PlayOneShot(WarpSound);
            OnlyClear = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FadeManager.Instance.LoadScene("TitleScene", 1.0f);
        }

    }
    private void OnSelect(InputAction.CallbackContext context)
    {

        if (PlayerStatus.IsTouched && !OnlyCheck)
        {
            FadeManager.Instance.LoadScene(RetryStage, 1.0f);
            SceneAudioSource.PlayOneShot(RetrySound);
            OnlyCheck = true;
        }
    }
}
