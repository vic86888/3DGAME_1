using UnityEngine;

public class PlatformMover1 : MonoBehaviour
{
    public float moveSpeed = 2f;    // 平台移動速度
    public float moveRange = 3f;   // 平台移動範圍

    private Vector3 startPosition; // 平台起始位置
    private bool movingRight = true; // 是否向右移動

    public TrapUp trapUp;

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
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // 向前
            // 超出右邊界，切換方向
            if (transform.position.z >= startPosition.z + moveRange)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime); // 向後
            // 超出左邊界，切換方向
            if (transform.position.z <= startPosition.z - moveRange)
            {
                movingRight = true;
            }
        }

    }

    public void ResetEnemies(){
        trapUp?.ResetToStart();
    }
}
