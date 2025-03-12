using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;          // 水平移動速度
    public float jumpForce = 8f;         // 每次跳躍的力量
    public int maxJumps = 3;              // 最大跳躍次數

    [Header("Other Settings")]
    public float minHeight = -10f;        // 定義最低高度
    public Vector3 initialPosition = new Vector3(0, 5, 0); // 初始位置
    public Vector3 playerScale = new Vector3(1f, 1f, 1f);   // 重置時的大小

    private Rigidbody rb;
    public int jumpCount = 0;
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

    public PlatformManager plaform_Mamager;

    private Transform cameraTransform;

    private static readonly Vector3 INVALID_RESPAWN_POINT = new Vector3(-1f, -1f, -1f);
    public bool canjump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
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
    /// 根據攝影機方向與玩家輸入控制移動，並根據當前重力方向調整
    /// </summary>
    private void HandleMovement()
    {
        // 依據目前全局重力計算「上」的方向（跳躍方向）
        Vector3 gravityUp = -Physics.gravity.normalized;

        // 計算攝影機在水平方向（垂直於 gravityUp）上的前方與右方向量
        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, gravityUp).normalized;
        // 若 forward 為零向量則使用 cameraTransform.up 重新計算
        if (forward == Vector3.zero)
            forward = Vector3.ProjectOnPlane(cameraTransform.up, gravityUp).normalized;
        // 利用 cross 取得正確的右方向量
        Vector3 right = Vector3.Cross(gravityUp, forward).normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 組合移動方向（注意：這個方向是在平台平面上）
        Vector3 moveDirection = (forward * verticalInput + right * horizontalInput);
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        // 根據是否反向移動決定速度（例如 Reverse 平台）
        float adjustedSpeed = isReversed ? -moveSpeed : moveSpeed;

        // 保留沿著重力方向（垂直於平台）的速度分量
        Vector3 verticalVelocity = Vector3.Project(rb.velocity, gravityUp);
        // 以新計算的移動方向設定水平速度
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
            // 跳躍方向取決於當前重力（永遠與重力相反）
            Vector3 jumpDir = -Physics.gravity.normalized;
            // 保留水平方向（垂直於跳躍方向）的速度
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(rb.velocity, jumpDir);
            // 設定跳躍速度為 jumpForce（可依需求改為 additive 的效果）
            rb.velocity = horizontalVelocity + jumpDir * jumpForce;
            if (canjump)
            {
                jumpCount++; // 只有在 maxJumps 不是无限时才增加计数
            }

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
        plaform_Mamager?.Restart();

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