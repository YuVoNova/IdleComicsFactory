using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField]
    private float clickTreshold;
    [SerializeField]
    private float dragMultiply;
    [SerializeField]
    private float rotationSpeed;

    private Touch touch;

    public bool clickFlag;
    public bool isJoystickActive;

    private Vector2 delta;
    private Vector2 clickCenter;
    private Vector2 direction2D;
    private Vector3 direction;
    private Vector3 targetDirection;

    private Quaternion targetRotation;

    [SerializeField]
    private float MovementSpeed;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        direction = Vector3.zero;
        targetDirection = Vector3.zero;

        clickFlag = false;
        isJoystickActive = false;


    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOn)
        {
            if (!GameManager.Instance.OnMenu)
            {
                if (Input.touches.Length > 0)
                {
                    touch = Input.touches[0];
                    delta = touch.deltaPosition;

                    if (touch.phase == TouchPhase.Began)
                    {
                        clickFlag = true;
                        isJoystickActive = false;
                    }
                    else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        if (clickFlag)
                        {
                            clickCenter = touch.position;

                            clickFlag = false;
                            isJoystickActive = true;

                            Player.Instance.Animator.SetBool("isRunning", true);
                        }

                        if (delta.magnitude > clickTreshold)
                        {
                            direction2D = touch.position - clickCenter;
                            direction = new Vector3(direction2D.x, 0.0f, direction2D.y);
                            direction = Vector3.Normalize(direction);
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        if (delta.magnitude < clickTreshold && clickFlag)
                        {
                            Click(touch);
                        }

                        clickFlag = false;
                        isJoystickActive = false;

                        Player.Instance.Animator.SetBool("isRunning", false);

                    }
                }
                else
                {
                    Player.Instance.Animator.SetBool("isRunning", false);

                    direction = transform.forward;
                }
            }
            else
            {

            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameOn)
        {
            transform.rotation = Quaternion.LookRotation(direction);

            if (isJoystickActive)
            {
                direction.y = 0f;
                rigidbody.velocity = direction * MovementSpeed * Time.fixedDeltaTime;
            }
            else
            {
                if (rigidbody.velocity.magnitude > 0.1f)
                {
                    rigidbody.velocity = Vector3.zero;
                }
            }
        }
    }

    private void Click(Touch touch)
    {

    }
}
