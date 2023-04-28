using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogSlide : MonoBehaviour
{
    [SerializeField] public float FogSpeed;     //スクロールスピード
    //[SerializeField] public float RestartPos = 1600;     //再生場所
    [SerializeField] public Vector3 startPos;  //開始時の座標格納
    public float aaa = 0;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;  //開始時の座標格納
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-FogSpeed * Time.deltaTime, 0, 0);  //一定速度で移動

        if (transform.position.x < -200) //画面サイズ分ずれたら
        {
            transform.position = new Vector3(startPos.x+200, startPos.y, startPos.z);   //反対側に飛ばす。Y・Z軸は開始時と同じ
        }
        aaa = transform.position.x;
    }
}