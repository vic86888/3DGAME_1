using Unity.VisualScripting;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    public float moveSpeed = 2f;    // 平台移動速度

    private Vector3 startPosition; // 平台起始位置
    private bool movingRight = true; // 是否向右移動

    public float Platform_xpositon = 0f;

    public float edge = 20f;

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
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            // 超出右邊界，切換方向
            if (transform.position.x >= Platform_xpositon + edge)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            // 超出左邊界，切換方向
            if (transform.position.x <= Platform_xpositon - edge)
            {
                movingRight = true;
            }
        }

        // 調試：檢查平台位置和移動方向
        // Debug.Log($"Platform Position: {transform.position.x}, Moving Right: {movingRight}");
    }
}
