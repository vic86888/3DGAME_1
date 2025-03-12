using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mashroom_bigger : MonoBehaviour
{
    public Vector3 scaleMultiplier = new Vector3(0.5f, 0.5f, 0.5f); // 增大的比例
    public float speedMultiplier = 0.2f; // 速度增加比例
    public float jumpMultiplier = 0.2f; // 跳躍力增加比例
    public mashroommanager a;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 確保碰撞的是角色
        {
            // 讓角色變大
            other.transform.localScale += scaleMultiplier;

            // 嘗試取得 PlayerController 腳本
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.moveSpeed += speedMultiplier; // 增加移動速度
                player.jumpForce += jumpMultiplier; // 增加跳躍力
            }

            if (other.transform.localScale.magnitude < 0.1f)
            {
                player.ResetPosition(); // 正確地重置玩家位置
            }

            gameObject.SetActive(false); // 禁用蘑菇物件
        }
        else // 確保碰撞的是球
        {
            // 嘗試取得 MoveBall_Target 腳本
            MoveBall_Target ball = other.GetComponent<MoveBall_Target>();
            if (ball != null)
            {
                // 確認當前目標是否是這個蘑菇
                if (ball.targets[ball.currentTargetIndex] == transform)
                {
                    Debug.Log($"球已接觸目標 {gameObject.name}！");

                    // 清除球的速度
                    Rigidbody ballRb = other.GetComponent<Rigidbody>();
                    if (ballRb != null)
                    {
                        ballRb.velocity = Vector3.zero; // 清除線性速度
                        ballRb.angularVelocity = Vector3.zero; // 清除角速度


                        // **新增功能：讓球變大**
                        other.transform.localScale += scaleMultiplier;

                        // **新增功能：讓球變快**
                        ball.moveSpeed += speedMultiplier;

                        // 通知球移動到下一個目標
                        ball.currentTargetIndex++;
                    }

                    // 禁用蘑菇物件（或其他行為）
                    gameObject.SetActive(false);
                }
            }
        }
    }

}
