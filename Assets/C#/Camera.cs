using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // 目标对象（主角）
    public Vector3 offset;      // 摄像机与目标的初始偏移量
    public float rotationSpeed = 5f; // 视角旋转速度
    public float minYAngle = -20f; // 摄像机的最小俯仰角度
    public float maxYAngle = 60f;  // 摄像机的最大俯仰角度

    private float currentYaw = 0f; // 当前水平方向的旋转角度
    private float currentPitch = 0f; // 当前垂直方向的旋转角度

    void LateUpdate()
    {
        if (target != null)
        {
            // 获取鼠标输入
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // 更新旋转角度
            currentYaw += mouseX * rotationSpeed;
            currentPitch -= mouseY * rotationSpeed;

            // 限制俯仰角度范围
            currentPitch = Mathf.Clamp(currentPitch, minYAngle, maxYAngle);

            // 计算旋转后的偏移量
            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
            Vector3 rotatedOffset = rotation * offset;

            // 设置摄像机的位置和朝向
            transform.position = target.position + rotatedOffset;
            transform.LookAt(target); // 让摄像机始终朝向目标
        }
    }
}