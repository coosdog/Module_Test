using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class PlayerMove : MonoBehaviour
{
    private float moveSpeed = 3f;
    private float sprintSpeed = 3f;
    // public float drag = 10f;
    private float rotationSpeed = 10f;
    private Rigidbody rb;
    private Animator animator;
    [SerializeField]
    private Transform cameraTransform;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //rb.drag = drag;

        animator = GetComponent<Animator>();

        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        Move();
        UpdateAnimator();
    }

    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 desiredMoveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized * moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift)&& desiredMoveDirection != Vector3.zero)
        {
            animator.SetBool("IsRun", true);
            desiredMoveDirection *= sprintSpeed;
        }
        else
        {
            animator.SetBool("IsRun", false);
        }

        if (desiredMoveDirection != Vector3.zero)
        {
            Vector3 currentVelocity = rb.velocity;
            Vector3 newVelocity = new Vector3(desiredMoveDirection.x, currentVelocity.y, desiredMoveDirection.z);
            rb.velocity = newVelocity;

            RotatePlayer(desiredMoveDirection);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        /*
        if (desiredMoveDirection != Vector3.zero)
        {
            rb.velocity = desiredMoveDirection;
            RotatePlayer(desiredMoveDirection);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        */
    }

    private void RotatePlayer(Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        bool isWalking = rb.velocity.magnitude > 0;
        animator.SetBool("IsWalking", isWalking);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        float lineLength = 5.0f; // 원하는 선의 길이

        // 현재 위치에서 아래로 제한된 거리만큼 선을 그림
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + Vector3.down * lineLength;

        Gizmos.DrawLine(startPosition, endPosition);
    }
}