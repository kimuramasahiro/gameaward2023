using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimation : MonoBehaviour
{
    // �X�P���g���֘A ---------------------------------------------------
    private GameObject SkeletonObj = null;        // �X�P���g���I�u�W�F�N�g
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
        // ���S���A�j���[�V����
        if (SkeletonHealth.Health <= 0.0f)
        {
            animator.SetTrigger("Death");
        }

        // ��_���[�W�A�j���[�V����
        else if (SkeletonHealth.IsReceiveDameged)
        {
            animator.SetTrigger("TakeHit");

            SkeletonHealth.IsReceiveDameged = false;
        }

        // �����A�j���[�V����
        else if (!SkeletonMovement.IsMovable)
        {
            m_delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }

        // �ҋ@�A�j���[�V����
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                animator.SetInteger("AnimState", 0);
        }
    }
}
