using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall_Target : MonoBehaviour
{
    private Vector3 startPosition; // 保存敌人的初始位置

    public Transform player;           // 玩家物件的 Transform
    public Transform[] targets;        // 周围需要接触的四个物件
    public float moveSpeed = 4.5f;     // 球的移動速度
    private Rigidbody rb;

    public int currentTargetIndex = 0; // 当前目标的索引
    private bool isChasingPlayer = false; // 是否開始追逐玩家
    private bool isChasingTargets = false; // 是否正在追逐目标

    void Start()
    {
        startPosition = transform.position; // 初始化位置
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isChasingPlayer && player != null)
        {
            if (!isChasingTargets)
            {
                ChaseTargets(); // 移動到目標
            }
            else
            {
                ChasePlayer(); // 開始追逐玩家
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0); // 只清除 X 和 Z 軸速度
        rb.angularVelocity = Vector3.zero; // 清除角速度
    }

    private void ChaseTargets()
    {
        if (currentTargetIndex < targets.Length)
        {
            // 朝當前目標移動
            Transform target = targets[currentTargetIndex];
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // 當所有目標都完成後，開始追逐玩家
            isChasingTargets = true;
            Debug.Log("所有目标已完成，现在开始追逐玩家！");
            rb.velocity = Vector3.zero; // 清除物體的線性速度
            rb.angularVelocity = Vector3.zero; // 清除物體的角速度
        }
    }

    private void ChasePlayer()
    {
        // 球朝玩家移動
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    // 當平台觸發時，啟動追逐
    public void StartChasing()
    {
        isChasingPlayer = true;
        Debug.Log("球開始追逐玩家！");
    }

    public void ResetToStart()
    {
        transform.position = startPosition; // 回到初始位置
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        moveSpeed = 4.5f;

        // 重置狀態
        currentTargetIndex = 0;
        isChasingTargets = false;
        isChasingPlayer = false;

        rb.velocity = Vector3.zero; // 清除物體的線性速度
        rb.angularVelocity = Vector3.zero; // 清除物體的角速度
        Debug.Log($"{gameObject.name} 已重置到起始位置！");
    }
}
