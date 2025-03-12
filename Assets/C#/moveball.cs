using UnityEngine;

public class MoveBall : MonoBehaviour
{
    private Vector3 startPosition; // 保存敌人的初始位置

    public Transform player;           // 玩家物件的 Transform
    public float moveSpeed = 4.5f;       // 球的移動速度
    private bool isChasing = false;    // 是否開始追逐玩家
    private Rigidbody rb;

    void Start()
    {
        startPosition = transform.position; // 初始化位置
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        // 如果啟動追逐，球就會朝玩家移動
        if (isChasing && player != null)
        {
            ChasePlayer();
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
        isChasing = true;
        Debug.Log("球開始追逐玩家！");
    }

    public void StopChasing()
    {
        isChasing = false;
        Debug.Log($"{gameObject.name} 停止追击！");
    }

    public void ResetToStart()
    {
        transform.position = startPosition; // 回到初始位置
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        moveSpeed = 4.5f;
        
        StopChasing(); // 停止追击
        // 清除物體的線性速度
        rb.velocity = Vector3.zero;
        

        // 清除物體的角速度
        rb.angularVelocity = Vector3.zero;
        Debug.Log($"{gameObject.name} 已重置到起始位置！");
    }
}
