using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleScene : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip sound1;
    AudioSource audioSource;
    private bool Only = true;

    private Controller _gameInputs;    //gamepad
    void Start()
    {
        Only = true;
        audioSource = GetComponent<AudioSource>();
        //Input Actionインスタンス生成
        _gameInputs = new Controller();

        //Actionイベント登録
        _gameInputs.Submit.Submit.performed += OnSelect;

        _gameInputs.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Return) || && Only)
        //{
        //    audioSource.PlayOneShot(sound1);
        //    FadeManager.Instance.LoadScene("1_1",1.0f);
        //    Only = false;
        //}

        EndGame();
    }
    //ゲーム終了
    private void EndGame()
    {
        //Escが押された時
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }

    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (Only)
            audioSource.PlayOneShot(sound1);
            FadeManager.Instance.LoadScene("1_1", 1.0f);
            Only = false;
    }
}
