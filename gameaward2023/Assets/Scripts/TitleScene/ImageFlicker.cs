﻿using UnityEngine;
using UnityEngine.UI;

public class ImageFlicker : MonoBehaviour
{

    //インスペクターから設定するか、初期化時にGetComponentして、Imageへの参照を取得しておく。
    [SerializeField]
    Image img;

    [Header("1ループの長さ(秒単位)")]
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float duration = 1.0f;

    //スクリプトで指定したい場合は[SerializeField]をコメントアウトする。
    //ループ開始時の色を0～255までの整数で指定。
    //元画像が白の場合は、指定した色になる。ドット絵等の場合は、白色を指定すると元画像への影響なし。アルファ値ゼロで完全に透明。
    [Header("ループ開始時の色")]
    [SerializeField]
    Color32 startColor = new Color32(255, 255, 255, 255);
    //ループ終了(折り返し)時の色を0～255までの整数で指定。
    [Header("ループ終了時の色")]
    [SerializeField]
    Color32 endColor = new Color32(255, 255, 255, 10);


    //インスペクターから設定した場合は、GetComponentする必要がなくなる為、Awakeを削除しても良い。
    void Awake()
    {
        if (img == null)
            img = GetComponent<Image>();
    }

    void Update()
    {
        //Color.Lerpに開始の色、終了の色、0～1までのfloatを渡すと中間の色が返される。
        //Mathf.PingPongに経過時間を渡すと、0～1までの値が返される。
        img.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / duration, 1.0f));
    }
}