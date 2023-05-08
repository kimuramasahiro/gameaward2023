using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalEnemy_Anim : MonoBehaviour
{
    // スケルトン関連 ---------------------------------------------------
    private GameObject SkeletonObj = null;        // スケルトンオブジェクト
    private VerticalEnemy_Move VerticalEnemy_Move;

    private Animator animator;
    private float m_delayToIdle = 0.0f;
    // ------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        SkeletonObj = this.gameObject;
        VerticalEnemy_Move = SkeletonObj.GetComponent<VerticalEnemy_Move>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 歩きアニメーション
        if (!VerticalEnemy_Move.IsMovable)
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
