using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;
    public Vector3 playerInput;

    public CharacterController player;

    public float playerSpeed;
    public Vector3 movePlayer;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;
    public bool grounded;
    public float duraciondash;

    public bool isDashing;
    public float dashcd;
    public float dashingcd;
    private int dashAttempts;
    private float dashStartTime;

    [SerializeField] ParticleSystem DashParticles;

    public Camera mainCamera;
    public Vector3 camForward;
    public Vector3 camRight;
    public Vector3 inputVector = Vector3.zero;
    public Transform playerPosition;
    public float enemySpeed;
    public float vision;
    public float turnSpeed = 10f;
    public bool lockon;
    public GameObject marcado;
    public bool marcando;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        player.Move(movePlayer * Time.deltaTime);
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");

        playerInput = new Vector3(inputVector.x, 0, inputVector.z);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        if (isDashing == false)
        {


            movePlayer = movePlayer * playerSpeed;
        }

        //movePlayer = movePlayer * playerSpeed;

        player.transform.LookAt(player.transform.position + movePlayer);

        SetGravity();
        SetJump();
        HandleDash();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    void SetGravity()
    {
        if (player.isGrounded)
        {
            grounded = true;
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;

        }

        else
        {
            grounded = false;
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
    }

    void SetJump()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
        }
    }

    void HandleDash()
    {
        bool isTryingToDash = Input.GetKeyDown(KeyCode.LeftShift);

        dashingcd += Time.deltaTime;

        if (isTryingToDash && !isDashing && dashingcd >= dashcd)
        {
            if (dashAttempts <= 500)
            {
                OnStartDash();
                DashParticles.Play();
                dashingcd = 0;
            }
        }

        if (isDashing)
        {
            if (Time.time - dashStartTime <= duraciondash)
            {
                if (playerInput.Equals(Vector3.zero))
                {
                    player.Move(transform.forward * 30f * Time.deltaTime);
                }
                else
                {
                    player.Move(transform.forward * 20f * Time.deltaTime);
                }
                //anim.SetBool("Dash", true);

            }
            else
            {
                OnEndDash();
            }

        }
    }

    void OnStartDash()
    {
        isDashing = true;
        dashStartTime = Time.time;
        //dashAttempts += 1;
    }

    void OnEndDash()
    {
        isDashing = false;
        dashStartTime = 0;
        //anim.SetBool("Dash", false);
    }


}
