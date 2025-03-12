using UnityEngine;

public class mover4 : MonoBehaviour
{
    public MoveBall[] ballScripts; // 使用陣列來存放多個 MoveBall 元件

    void Start()
    {
        // 可以在這裡進行一些初始化操作（如果需要）
    }

    void Update()
    {
        // 如有需要，可以在 Update 中進行動態更新
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 檢查碰撞物件是否是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("玩家碰到了平台，通知球開始追逐！");

            // 呼叫每個球的 StartChasing 方法
            if (ballScripts != null && ballScripts.Length > 0)
            {
                foreach (MoveBall ball in ballScripts)
                {
                    if (ball != null)
                    {
                        ball.StartChasing(); // 呼叫 StartChasing 方法
                    }
                    
                }
            }
        }
    }

    public void ResetEnemies()
    {
        // 重置所有敵人
        if (ballScripts != null && ballScripts.Length > 0)
        {
            foreach (MoveBall ball in ballScripts)
            {
                if (ball != null)
                {
                    ball.ResetToStart(); // 呼叫 ResetToStart 方法
                }
            }
        }
    }
}
