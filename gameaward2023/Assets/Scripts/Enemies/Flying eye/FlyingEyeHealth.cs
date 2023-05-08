using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeHealth : MonoBehaviour, IReceiveDamege
{
    private float Health = 50.0f;

    private bool IsReceiveDameged = false;
    private Animator animator;
    private new BoxCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReceiveDameged)
        {
            animator.Play("TakeHit", 0, 0);

            //animator.SetTrigger("TakeHit");

            IsReceiveDameged = false;
        }

        if (Health <= 0.0f)
        {
            collider.isTrigger = true;
            Health = 0.0f;
            animator.SetTrigger("Death");
            Destroy(gameObject, 2.0f);　　　//倒れた後にシーンから消滅させる
        }
    }

    public void ReceiveDamage(float damage)
    {
        Health -= damage;

        IsReceiveDameged = true;
    }
}
