using UnityEngine;

public class WaterBlock : MonoBehaviour
{
    public float speedMultiplier = 2f; // 增加移动速度
    public float waterDrag = 3f; // 增加阻力（降低下落速度）

    private float originalSpeed;
    private float originalDrag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (player != null && rb != null)
            {
                // 记录原始状态
                originalSpeed = player.moveSpeed;
                originalDrag = rb.drag;

                // 增加速度 & 设置阻力
                player.moveSpeed *= speedMultiplier;
                rb.drag = waterDrag;

                player.jumpCount = 0;
                player.canjump = false;
                player.jumpForce = 16;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (player != null && rb != null)
            {
                // 恢复原始状态
                player.moveSpeed = originalSpeed;
                rb.drag = originalDrag;

                player.canjump = true;
                player.moveSpeed = 7;
                player.jumpForce = 8;
            }
        }
    }
}
