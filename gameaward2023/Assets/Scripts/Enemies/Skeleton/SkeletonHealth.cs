using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHealth : MonoBehaviour, IReceiveDamege
{
    public float Health = 50.0f;

    public bool IsReceiveDameged = false;
    private new BoxCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // HPが0以下の場合
        if(Health <= 0.0f)
        {
            collider.isTrigger = true;      // 当たり判定をなくす
            Health = 0.0f;                  // HPを0で固定する
            Destroy(gameObject, 2.0f);　　　// 倒れた後にシーンから消滅させる
        }
    }

    public void ReceiveDamage(float damage)
    {
        Health -= damage;

        IsReceiveDameged = true;
    }
}
