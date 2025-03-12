using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;          // 水平移動速度
    public float jumpForce = 10f;         // 每次跳躍的力量
    public int maxJumps = 3;              // 最大跳躍次數

    [Header("Other Settings")]
    public float minHeight = -10f;        // 定義最低高度
    public Vector3 initialPosition = new Vector3(0, 5, 0); // 初始位置
    public Vector3 playerScale = new Vector3(1f, 1f, 1f);   // 重置時的大小

    private Rigidbody rb;
    private int jumpCount = 0;
    private bool canMove = false;
    private bool isReversed = false;
    private GameObject lastPlatform;

    private float startSpeed;
    private float startJumpForce;

    private mover3 mover;                // 處理 Reverse 平台邏輯
    private PlatformMover1 platformMover;  // 處理 Slow 平台邏輯
    public mover4 mover4;                // 另一個敵人控制器
    public mover4_mushroom mover4_Mushroom;
    private mashroommanager mash;

    private CameraFollow01 cameraFollow01; // **改為 CameraFollow01**

    private static readonly Vector3 INVALID_RESPAWN_POINT = new Vector3(-1f, -1f, -1f);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraFollow01 = FindObjectOfType<CameraFollow01>(); // **獲取 CameraFollow01**
        mash = FindObjectOfType<mashroommanager>();

        startSpeed = moveSpeed;
        startJumpForce = jumpForce;
        GameManager.Instance.respawnPoint = initialPosition;
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
    }

    void Update()
    {
        // 當玩家未啟動移動時，僅保留重力方向的分量
        if (!canMove)
        {
            Vector3 gravityUp = -Physics.gravity.normalized;
            Vector3 verticalVelocity = Vector3.Project(rb.velocity, gravityUp);
            rb.velocity = verticalVelocity;
            return;
        }

        HandleMovement();
        HandleJump();
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();


        }

        if (transform.position.y < minHeight)
        {
            ResetPosition();
        }
    }

    /// <summary>
    /// **根據 CameraFollow01 計算的方向來移動**
    /// </summary>
    private void HandleMovement()
    {
        // 依據 CameraFollow01 計算的 forward/right
        var (forward, right) = cameraFollow01.GetMovementDirections();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 組合移動方向
        Vector3 moveDirection = (forward * verticalInput + right * horizontalInput);
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        // 根據是否反向移動決定速度
        float adjustedSpeed = isReversed ? -moveSpeed : moveSpeed;

        // 保留垂直速度
        Vector3 gravityUp = -Physics.gravity.normalized;
        Vector3 verticalVelocity = Vector3.Project(rb.velocity, gravityUp);
        Vector3 horizontalVelocity = moveDirection * adjustedSpeed;

        rb.velocity = horizontalVelocity + verticalVelocity;
    }

    /// <summary>
    /// 處理跳躍行為，使跳躍方向永遠與當前重力相反
    /// </summary>
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            Vector3 jumpDir = -Physics.gravity.normalized;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(rb.velocity, jumpDir);
            rb.velocity = horizontalVelocity + jumpDir * jumpForce;
            jumpCount++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        switch (tag)
        {
            case "Platform":
                HandleStandardPlatformCollision(collision.gameObject);
                break;
            case "Slow":
                HandleSlowPlatformCollision(collision.gameObject);
                break;
            case "Reverse":
                HandleReversePlatformCollision(collision.gameObject);
                break;
            case "Can't_Jump":
                HandleCantJumpCollision(collision.gameObject);
                break;
            case "Die":
                ResetPosition();
                break;
            case "Finish":
                SceneManager.LoadScene("Scene2");
                break;
            case "GravityCube":
                // 根據碰撞點法線改變全域重力方向
                Vector3 contactNormal = collision.contacts[0].normal;
                Vector3 newGravityDirection = -contactNormal.normalized;
                Physics.gravity = newGravityDirection * 10;
                jumpCount = 0;
                break;
        }
    }

    private void HandleStandardPlatformCollision(GameObject platform)
    {
        lastPlatform = platform;
        ResetMovementFlags();
    }

    private void HandleSlowPlatformCollision(GameObject platform)
    {
        lastPlatform = platform;
        ResetMovementFlags();
        platformMover = platform.GetComponent<PlatformMover1>();
        moveSpeed = 2f;
    }

    private void HandleReversePlatformCollision(GameObject platform)
    {
        lastPlatform = platform;
        moveSpeed = startSpeed;
        ResetMovementFlags();
        mover = platform.GetComponent<mover3>();
        isReversed = true;
    }

    private void HandleCantJumpCollision(GameObject platform)
    {
        lastPlatform = platform;
        ResetMovementFlags();
        jumpCount += 3;  // 強制將跳躍次數設為上限
    }

    /// <summary>
    /// 重置平台碰撞後的通用參數
    /// </summary>
    private void ResetMovementFlags()
    {
        jumpCount = 0;
        canMove = true;
        isReversed = false;
    }

    /// <summary>
    /// 重置玩家位置與狀態
    /// </summary>
    public void ResetPosition()
    {
        // 若 GameManager 提供了有效的重生點，則更新 initialPosition
        if (GameManager.Instance.respawnPoint != INVALID_RESPAWN_POINT)
        {
            initialPosition = GameManager.Instance.respawnPoint;
        }

        if (lastPlatform != null)
        {
            switch (lastPlatform.tag)
            {
                case "Platform":
                    transform.position = initialPosition;
                    break;
                case "Slow":
                case "Reverse":
                    RespawnAtPlatform(lastPlatform);
                    break;
                default:
                    transform.position = initialPosition;
                    break;
            }
        }
        else
        {
            transform.position = initialPosition;
        }

        // 重置敵人狀態
        mover?.ResetEnemies();
        platformMover?.ResetEnemies();
        mover4?.ResetEnemies();
        mover4_Mushroom?.ResetEnemies();
        mash?.restart();

        // 重置 Rigidbody 狀態
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        jumpCount = 0;
        canMove = false;

        transform.localScale = playerScale;
        moveSpeed = startSpeed;
        jumpForce = startJumpForce;

        Physics.gravity = new Vector3(0f, -9.81f, 0f);
    }

    /// <summary>
    /// 根據平台指定的重生點重生，若無指定則使用平台中心向上偏移1個單位
    /// </summary>
    /// <param name="platform">碰撞的平台</param>
    private void RespawnAtPlatform(GameObject platform)
    {
        RespawnPoint respawnPoint = platform.GetComponent<RespawnPoint>();
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.respawnPosition;
        }
        else
        {
            transform.position = platform.transform.position + Vector3.up;
        }
    }
}