using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimation : MonoBehaviour
{
    // スケルトン関連 ---------------------------------------------------
    private GameObject SkeletonObj = null;        // スケルトンオブジェクト
    private SkeletonMovement SkeletonMovement;
    private SkeletonHealth SkeletonHealth;

    private Animator animator;
    private float m_delayToIdle = 0.0f;
    // ------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        SkeletonObj = this.gameObject;
        SkeletonMovement = SkeletonObj.GetComponent<SkeletonMovement>();
        SkeletonHealth   = SkeletonObj.GetComponent<SkeletonHealth>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 死亡時アニメーション
        if (SkeletonHealth.Health <= 0.0f)
        {
            animator.SetTrigger("Death");
        }

        // 被ダメージアニメーション
        else if (SkeletonHealth.IsReceiveDameged)
        {
            animator.SetTrigger("TakeHit");

            SkeletonHealth.IsReceiveDameged = false;
        }

        // 歩きアニメーション
        else if (!SkeletonMovement.IsMovable)
        {
            m_delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }

        // 待機アニメーション
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                animator.SetInteger("AnimState", 0);
        }
    }
}
