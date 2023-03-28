using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionAtk : MonoBehaviour
{
    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private PlayerAttack PlayerAttack;

    public float fDamage = 10.0f;
    // ------------------------------------------------------------------

    // private bool isEnemyTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerAttack = PlayerObj.GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(PlayerAttack.IsAttacking)
        {
            // インターフェイスを取得
            var hit = collision.gameObject.GetComponent<IReceiveDamege>();

            // 触れた相手の名前を表示
            hit.ReceiveDamage(fDamage);

            collision.transform.root.GetComponent<TakeDamage>().Damage(collision);

            PlayerAttack.IsAttacking = false;
        }
    }

    public float GetAttackDamage()
    {
        return fDamage; 
    }
}
