using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [Header("General")]
    public Transform[] Lanes;
    public GameObject GameOverUI;

    [Header("Stats")]
    public float coyoteTime;
    public float jumpHeight;
    public int gravityMultiplier = 1;
    public float playerSpeed = 2f;
    public float laneSwapTime = 2f;
    public float getFasterTimer = 5f;
    public float playerColRollHeight;

    private PlayerInputs inputActions;
    private Coroutine onGoingRoutine;
    private Vector3 playerVelocity;
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
        playerVelocity.z = 1 * playerSpeed;

        if (isGrounded)
        {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
            lastGroundedTime = Time.time;
            playerVelocity.y = 0;
            ySpeed = 0;
        }
        else
            playerVelocity.y = ySpeed;

        if (Time.time - lastGroundedTime <= coyoteTime)
        {
            if (Time.time - jumpPressedTime <= coyoteTime)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                jumpPressedTime = null;
                lastGroundedTime = null;
                isGrounded = false;
            }
        }

        if (Time.time - lastSpedUpTime > getFasterTimer)
        {
            playerSpeed += 1;
            lastSpedUpTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(playerVelocity * Time.deltaTime, Space.World);
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
            playerCol.height = playerColRollHeight;
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
        playerCol.height = playerColStartHeight;
        isRolling = false;
    }
}
