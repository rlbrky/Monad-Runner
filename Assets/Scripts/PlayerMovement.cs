using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [Header("General")]
    public Transform[] Lanes;
    public GameObject GameOverUI;

    [Header("Jump")]
    public float coyoteTime;
    public float jumpHeight;
    public int gravityMultiplier = 1;
    public float maxYSpeed = -10f;
    [Header("Stats")]
    public float playerSpeed = 2f;
    public float laneSwapTime = 2f;
    public float getFasterTimer = 5f;
    public float playerColRollHeight;

    private Vector3 forward;
    private PlayerInputs inputActions;
    private Coroutine onGoingRoutine;
    private CapsuleCollider playerCol;

    private int laneSpecifier = 0;
    private int startGravityMult;
    private float gravity;
    private float ySpeed;
    private float? lastGroundedTime;
    private float? jumpPressedTime;
    private float? lastSpedUpTime;
    private float playerColStartHeight;

    private bool isRolling;
    public bool isGrounded;

    [Header("Slope Mech")]
    [SerializeField] private float maxGroundAngle = 120;
    [SerializeField] private float playerHeight = 0.5f;
    [SerializeField] private float playerHeightPadding = 0.05f;
    [SerializeField] private bool debug;
    [SerializeField] private LayerMask groundMask;
    private float groundAngle;
    private RaycastHit hitInfo;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        inputActions = new PlayerInputs();
        inputActions.Inputs.Enable();
        inputActions.Inputs.MoveRight.started += MoveRight;
        inputActions.Inputs.MoveLeft.started += MoveLeft;
        inputActions.Inputs.Crouch.started += Crouch;
        inputActions.Inputs.Jump.started += Jump;

        lastSpedUpTime = Time.time;
        laneSpecifier = 1;
        playerCol = GetComponent<CapsuleCollider>();
        playerColStartHeight = playerCol.height;
        startGravityMult = gravityMultiplier;
    }

    private void Update()
    {
        gravity = Physics.gravity.y * gravityMultiplier;
        ySpeed += gravity * Time.deltaTime;
        if(ySpeed < maxYSpeed)
            ySpeed = maxYSpeed;

        CalculateForward();
        CalculateGroundAngle();
        CheckForGround();
        ApplyGravity();
        DrawDebugLines();

        //Speed up the game after some time passes.
        if (Time.time - lastSpedUpTime > getFasterTimer)
        {
            playerSpeed += 1;
            lastSpedUpTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (groundAngle >= maxGroundAngle) return;
        transform.Translate(forward * playerSpeed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Obstacle")
        {
            GameOverUI.SetActive(true);
            Time.timeScale = 0;
            inputActions.Inputs.Disable();
            inputActions = null;
        }
    }

    private void MoveRight(InputAction.CallbackContext context)
    {
        if (laneSpecifier < 2)
        {
            laneSpecifier++;
            onGoingRoutine = StartCoroutine(SwapLane(Lanes[laneSpecifier].position.x));
        }
    }

    private void MoveLeft(InputAction.CallbackContext context)
    {
        if (laneSpecifier > 0)
        {
            laneSpecifier--;
            onGoingRoutine = StartCoroutine(SwapLane(Lanes[laneSpecifier].position.x));
        }
    }
    
    private void Crouch(InputAction.CallbackContext context)
    {
        if (!isRolling)
        {
            //TO DO: Play roll anim.
            isRolling = true;
            //playerCol.height = playerColRollHeight;
            transform.localScale = new Vector3(1, 0.5f, 1);
            gravityMultiplier = 10;
            StartCoroutine(FixColliderHeight());
        }

    }

    private void Jump(InputAction.CallbackContext context)
    {
        jumpPressedTime = Time.time;
    }

    private IEnumerator SwapLane(float posX)
    {
        float time = 0;
        float startPos = transform.position.x;

        while (time <= laneSwapTime)
        {
            transform.position = new Vector3(Mathf.Lerp(startPos, posX, time / laneSwapTime), transform.position.y, transform.position.z);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }

    private IEnumerator FixColliderHeight()
    {
        yield return new WaitForSeconds(1);
        gravityMultiplier = startGravityMult;
        //playerCol.height = playerColStartHeight;
        transform.localScale = Vector3.one;
        isRolling = false;
    }

    /// <summary>
    /// Calculates the forward vector for climbing ramps.
    /// </summary>
    private void CalculateForward()
    {
        if (!isGrounded)
        {
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(hitInfo.normal, -transform.right);
    }

    /// <summary>
    /// Calculates the angle between ground and player.
    /// </summary>
    private void CalculateGroundAngle()
    {
        if (!isGrounded)
        {
            groundAngle = 90;
            return;
        }

        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    private void CheckForGround()
    {
        if (ySpeed > 0) return;

        if(Physics.Raycast(transform.position, -Vector3.up, out hitInfo, playerHeight + playerHeightPadding, groundMask))
        {
            if(Vector3.Distance(transform.position, hitInfo.point) < playerHeight)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * playerHeight, 5 * Time.deltaTime);
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// Applies gravity and handles jumping functionality.
    /// </summary>
    private void ApplyGravity()
    {
        if (isGrounded)
        {
            //transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
            lastGroundedTime = Time.time;
            //forward.y = 0;
            ySpeed = 0;
        }
        else
        {
            forward.y = ySpeed;
        }

        //Jump functionality.
        if (Time.time - lastGroundedTime <= coyoteTime)
        {
            if (Time.time - jumpPressedTime <= coyoteTime)
            {
                isGrounded = false;
                ySpeed = Mathf.Sqrt(jumpHeight * -1.2f * gravity);
                jumpPressedTime = null;
                lastGroundedTime = null;
            }
        }
    }

    private void DrawDebugLines()
    {
        if (!debug) return;

        Debug.DrawLine(transform.position, transform.position + forward * playerHeight * 2, Color.red);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * playerHeight, Color.magenta);
    }
}
