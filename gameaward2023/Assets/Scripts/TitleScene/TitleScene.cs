using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip sound1;
    AudioSource audioSource;
    private bool Only = true;
    void Start()
    {
        Only = true;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && Only)
        {
            audioSource.PlayOneShot(sound1);
            FadeManager.Instance.LoadScene("MainScene",1.0f);
            Only = false;
        }
    }
}
