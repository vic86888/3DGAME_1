using UnityEngine;

public class TrapUp : MonoBehaviour
{
    public Transform player;        // 玩家对象

    public Transform slowcube;
    public Vector3 size = new Vector3(1, 1, 20); // 刺的大小 (1,1,20)
    public float flyHeight = 5f;    // 刺向上飞的高度
    public float flySpeed = 2f;     // 刺向上飞的速度
    public float flyDuration = 3f;  // 刺飞行后停留的时间（3秒）

    private Vector3 currentposition;  // 刺的初始位置
    private bool isFlying = false;  // 是否正在飞行
    private float flyTimer = 0f;    // 记录飞行时间

    void Start()
    {

    }

    void Update()
    {
        currentposition = slowcube.transform.position;
        currentposition.y += 0.5f;
        if (IsPlayerAbove() && !isFlying) // 玩家触发飞行
        {
            isFlying = true;
            flyTimer = 0f; // 重置计时器
            Debug.Log("玩家在刺上方，触发飞行！");
        }

        if (isFlying)
        {
            // 刺向上飞行
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up * flyHeight, flySpeed * Time.deltaTime);

            // 计时器累加
            flyTimer += Time.deltaTime;

            // 如果飞行时间超过设定的3秒，返回初始位置
            if (flyTimer >= flyDuration)
            {
                isFlying = false; // 停止飞行状态
                transform.position = currentposition; // 返回初始位置
                Debug.Log("返回初始位置！");
            }
        }
    }

    // 检测玩家是否在刺的上方
    bool IsPlayerAbove()
    {
        if (player == null)
            return false;

        Vector3 playerPos = player.position;

        // 检查玩家的 x 和 z 坐标是否在刺的范围内
        bool inXRange = playerPos.x >= transform.position.x - size.x / 2 && playerPos.x <= transform.position.x + size.x / 2;
        bool inZRange = playerPos.z >= transform.position.z - size.z / 2 && playerPos.z <= transform.position.z + size.z / 2;

        // 检查玩家是否在刺的上方 (y 坐标)
        bool aboveSpike = playerPos.y > transform.position.y;

        return inXRange && inZRange && aboveSpike;
    }

    public void ResetToStart()
    {
        transform.position = currentposition; // 回到初始位置
        Debug.Log($"{gameObject.name} 已重置到起始位置！");
        isFlying = false;
    }
}
