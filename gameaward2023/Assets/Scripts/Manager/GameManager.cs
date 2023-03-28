using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // タップ対象のオブジェクト
    private GameObject TargetObj = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // タップ検出処理
        if(Input.GetMouseButtonDown(0))
        {
            // タップしたブロックの座標を表示
            GetBlockTapPos();
        }
    }

    private void GetBlockTapPos()
    {
        // タップした方向にカメラからRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(ray,out hit))
        {
            // Rayに当たる位置に存在するオブジェクトを取得
            TargetObj = hit.collider.gameObject;
        }

        // 対象オブジェクト(マップブロック)が存在する場合の処理
        if(TargetObj != null)
        {
            // ブロック選択時処理
            SelectBlock(TargetObj.GetComponent<MapBlock>());
        }
    }

    private void SelectBlock(MapBlock targetBlock)
    {
        Debug.Log("ブロックの座標 : " + targetBlock.transform.position);
    }
}
