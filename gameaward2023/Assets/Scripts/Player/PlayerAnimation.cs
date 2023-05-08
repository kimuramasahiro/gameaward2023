using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private PlayerMovement PlayerMovement;
    private PlayerAttack PlayerAttack;
    // ------------------------------------------------------------------

    // アニメーション管理 -----------------------------------------------
    private Animator m_animator;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;

    private bool m_noBlood = false;            // 血の表現(true:あり false:なし) 
    // ------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
        PlayerAttack = PlayerObj.GetComponent<PlayerAttack>();

        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // アニメーション再生時間
        m_timeSinceAttack += Time.deltaTime;

        // 常に地面についている状態
        m_animator.SetBool("Grounded", true);

        // 死亡アニメーション
        if (Input.GetKeyDown("e"))
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }

        // 攻撃アニメーション
        else if (Input.GetKeyDown("f") && m_timeSinceAttack > 0.5f)
        {
            PlayerAttack.IsAttacking = true;

            m_currentAttack = 1;

            // アニメーション再生時間が1秒超えるとコンボ終了
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // 再生するアニメーションのTriggerを設定
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // タイマーリセット
            m_timeSinceAttack = 0.0f;
        }

        else if (PlayerMovement.IsMoving)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        // アニメーションを再生していないときはIdle状態にする
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

        // 攻撃して当たっていなかったらfalse
        if (m_timeSinceAttack > 0.1f)
        {
            PlayerAttack.IsAttacking = false;
        }


        //else if (Input.GetKeyDown("f") && m_timeSinceAttack > 0.25f)
        //{
        //    // 攻撃アニメーションを更新する
        //    m_currentAttack++;

        //    // 攻撃アニメーションが一周したら始めに戻す
        //    if (m_currentAttack > 3)
        //        m_currentAttack = 1;

        //    // Reset Attack combo if time since last attack is too large
        //    if (m_timeSinceAttack > 1.0f)
        //        m_currentAttack = 1;

        //    // 再生するアニメーションのTriggerを設定
        //    m_animator.SetTrigger("Attack" + m_currentAttack);

        //    // タイマーリセット
        //    m_timeSinceAttack = 0.0f;
        //}
    }
}
