using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveball2 : MonoBehaviour
{
    private Vector3 startPosition; // 保存敌人的初始位置

    public Transform player;           // 玩家物件的 Transform
    public Transform[] targets;        // 周围需要接触的四个物件
    public float moveSpeed = 4.5f;     // 球的移動速度
    private Rigidbody rb;

    private int currentTargetIndex = 0; // 当前目标的索引
    private bool isChasingTargets = false; // 是否正在追逐目标

    void Start()
    {
        startPosition = transform.position; // 初始化位置
        rb = GetComponent<Rigidbody>();
        
        // 一開始就開始追逐玩家
        isChasingTargets = true;
    }

    void Update()
    {
        if (isChasingTargets)
        {
            ChasePlayer(); // 直接開始追逐玩家
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0); // 只清除 X 和 Z 軸速度
        rb.angularVelocity = Vector3.zero; // 清除角速度
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    public void ResetToStart()
    {
        transform.position = startPosition; // 回到初始位置
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        moveSpeed = 4.5f;

        // 重置狀態
        currentTargetIndex = 0;
        isChasingTargets = true;

        rb.velocity = Vector3.zero; // 清除物體的線性速度
        rb.angularVelocity = Vector3.zero; // 清除物體的角速度
        Debug.Log($"{gameObject.name} 已重置到起始位置！");
    }
}
