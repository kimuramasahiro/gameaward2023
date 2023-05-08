using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCtrlCommon : MonoBehaviour
{
    //マップ探索用
    ElementGenerator elementGenerator;                      //オブジェクト生成用
    [SerializeField] GameObject objMapTip;                  //マップチップリソース
    GameObject objMapTipFinal;                              //生成したマップチップ

    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private PlayerMovement PlayerMovement;

    private Vector2 PlayerCurrentPos = Vector2.zero;
    // ------------------------------------------------------------------

    void Awake()
    {
        //マップ探索用
        elementGenerator = GameObject.Find("StageMake").GetComponent<ElementGenerator>();

        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
    }

    void Start()
    {
        ////マップチップ生成
        //objMapTipFinal = elementGenerator.GenerateObjMap2D(gameObject, objMapTip);

        ////プレイヤー以外は初期は非表示
        //if (CompareTag("Player") == false)
        //{
        //    Color color = objMapTipFinal.GetComponent<Image>().color;
        //    color.a = 0;
        //    objMapTipFinal.GetComponent<Image>().color = color;
        //}
    }

    void Update()
    {
        ////マップチップ更新
        //elementGenerator.UpdateMap2D(gameObject, objMapTipFinal);

        ////プレイヤーはマップ探索する
        //if (CompareTag("Player") == true)
        //{
        //    // プレイヤー座標格納
        //    PlayerCurrentPos.x = PlayerObj.transform.position.x;
        //    PlayerCurrentPos.y = PlayerObj.transform.position.z;

        //    //elementGenerator.SearchMap((int)(PlayerCurrentPos.x + 0.5f), (int)(PlayerCurrentPos.y + 0.5f));
        //}
    }
}