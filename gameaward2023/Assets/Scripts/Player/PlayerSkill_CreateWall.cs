using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_CreateWall : MonoBehaviour
{
    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private PlayerMovement PlayerMovement;
    // ------------------------------------------------------------------

    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // プレイヤーが動いていないとき
        if(!PlayerMovement.IsMoving)
        {

        }
    }
}
