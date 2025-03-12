using UnityEngine;

public class CameraFollow01 : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float rotationSpeed = 5f;
    public float minYAngle = -20f;
    public float maxYAngle = 60f;

    private float currentYaw = 180f; // 初始水平角度
    private float currentPitch = 0f;
    private Quaternion cameraRotation;
    private Vector3 referenceUp;    // 當前作為參考的重力上方向
    private bool isUpsideDown = false;

    void Start()
    {
        // 初始化時以目前 Physics.gravity 為參考重力上方向
        referenceUp = -Physics.gravity.normalized;
        cameraRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // 取得最新的重力上方向（用於 LookRotation 的 up）
            Vector3 gravityUp = -Physics.gravity.normalized;

            // 無論有無滑鼠輸入，都重新計算相機旋轉，這樣才能立刻反映 currentYaw 的變化
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            currentYaw += mouseX * rotationSpeed;
            currentPitch -= mouseY * rotationSpeed;
            currentPitch = Mathf.Clamp(currentPitch, minYAngle, maxYAngle);

            // 使用 referenceUp 作為 yaw 的旋轉軸
            Quaternion yawRotation = Quaternion.AngleAxis(currentYaw, referenceUp);
            // 根據 yawRotation 計算出右向量，作為 pitch 的旋轉軸
            Vector3 right = yawRotation * Vector3.right;
            Quaternion pitchRotation = Quaternion.AngleAxis(currentPitch, right);
            cameraRotation = yawRotation * pitchRotation;

            // 根據旋轉後的 cameraRotation 計算偏移，得到相機位置
            Vector3 rotatedOffset = cameraRotation * offset;
            transform.position = target.position + rotatedOffset;

            // 設定相機旋轉，使其看向目標，並以最新重力方向作為 up 向量
            transform.rotation = Quaternion.LookRotation(target.position - transform.position, gravityUp);
        }
    }

    public (Vector3 forward, Vector3 right) GetMovementDirections()
    {
        Vector3 gravityUp = -Physics.gravity.normalized;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, gravityUp).normalized;
        Vector3 right = Vector3.Cross(gravityUp, forward).normalized;
        return (forward, right);
    }

}
