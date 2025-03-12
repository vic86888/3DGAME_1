using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mashroom_reset1 : MonoBehaviour
{
    public Vector3 new_initialPosition = new Vector3(0, 5, 0); // 初始位置
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 確保碰撞的是角色
        {
            // 嘗試取得 PlayerController 腳本
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // 恢復到初始大小
                other.transform.localScale = player.playerScale;

                // 恢復初始速度和跳躍力
                player.moveSpeed = 7f;  // 根據你的 PlayerController 預設值
                player.jumpForce = 8f; // 根據你的 PlayerController 預設值
                GameManager.Instance.SetRespawnPoint(new_initialPosition);
            }

            // 讓物件消失
            Destroy(gameObject);

            // 检查是否有父对象，如果有则销毁父对象
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
