using UnityEngine;

public class mover3 : MonoBehaviour
{
    public float moveSpeed = 2f;    // 平台移動速度
    public float moveRange = 3f;   // 平台移動範圍

    private Vector3 startPosition; // 平台起始位置
    private bool movingRight = true; // 是否向右移動

    public MoveBall ballScript; // 連結球物件上的 MoveBall 腳本
    public MoveBall ballScript1; // 連結球物件上的 MoveBall 腳本
    public MoveBall ballScript2; // 連結球物件上的 MoveBall 腳本

    void Start()
    {
        // 記錄起始位置
        startPosition = transform.position;
    }

    void Update()
    {
        // 平台移動
        if (movingRight)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            // 超出右邊界，切換方向
            if (transform.position.x >= startPosition.x + moveRange)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            // 超出左邊界，切換方向
            if (transform.position.x <= startPosition.x - moveRange)
            {
                movingRight = true;
            }
        }

        // 調試：檢查平台位置和移動方向
        // Debug.Log($"Platform Position: {transform.position.x}, Moving Right: {movingRight}");
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 檢查碰撞物件是否是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("玩家碰到了平台，通知球開始追逐！");

            // 呼叫球的 StartChasing 方法
            if (ballScript != null)
            {
                ballScript.StartChasing();
                ballScript1.StartChasing();
                ballScript2.StartChasing();
            }
            else
            {
                Debug.LogError("未找到任何帶有 MoveBall 腳本的球物件！");
            }
        }
    }

    public void ResetEnemies()
    {
        // 重置所有敵人
        ballScript?.ResetToStart();
        ballScript1?.ResetToStart();
        ballScript2?.ResetToStart();
    }
}
