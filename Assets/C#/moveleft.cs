using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public float moveSpeed = 2f;    // 平台移動速度
    public float moveRange = 3f;   // 平台移動範圍

    private Vector3 startPosition; // 平台起始位置
    private bool movingRight = true; // 是否向右移動

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
}
