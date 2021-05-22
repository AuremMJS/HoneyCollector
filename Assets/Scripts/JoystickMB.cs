using UnityEngine;

// Class to Joystick implementation
public class JoystickMB : MonoBehaviour
{
    // Singleton instance
    public static JoystickMB Instance;

    // Is the joystick active
    public bool isActive;

    // Speed of movement of spoon
    public float movementSpeed = 2f;

    // Reference to circle and dot of the joystick
    public GameObject JoystickCircle, JoystickDot;

    // Reference to the touch
    private Touch oneTouch;

    // Reference to the touch position
    public Vector3 touchPosition;

    // Direction of movement of the spoon
    public Vector3 moveDirection;

    // Singleton initialization in awake
    void Awake()
    {
        Debug.Assert(Instance == null, "Cannot create another instance of Singleton class");
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        if(Application.isEditor)
        {
            GetComponent<JoyStickMouseMB>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            // Keep the spoon away from honey comb when joystick is not active
            Vector3 newPosition = transform.position;
            newPosition.z = -0.74f;
            transform.position = newPosition;

            // Deactivate the joystick
            if (SpoonMB.Instance.honeyLevelScaleValue >= 1.0f)
                DeactivateJoystick();
            return;
        }

        // Touch detection
        if (Input.touchCount > 0)
        {
            // Get touch and set touch position
            oneTouch = Input.GetTouch(0);
            SetTouchPosition(oneTouch.position);

            // Process the touch
            ProcessTouchPhase();
        }
    }

    // Init
    public void Init()
    {
        isActive = true;
        JoystickCircle.SetActive(false);
        JoystickDot.SetActive(false);
    }
	
    // Process touch
    void ProcessTouchPhase()
    {
        switch (oneTouch.phase)
        {
            case TouchPhase.Began:
                ActivateJoystick();
                break;

            case TouchPhase.Moved:
                MoveSpoon();
                break;
            case TouchPhase.Stationary:
                moveDirection = Vector3.zero;
                break;
            case TouchPhase.Ended:
                DeactivateJoystick();
                break;
        }
    }

    // Set touch position
    public void SetTouchPosition(Vector3 position)
    {
        touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x,
 position.y, 12.52f));
        touchPosition.z = 0.0f;
    }

    // Activate joystick
    public void ActivateJoystick()
    {
        JoystickCircle.SetActive(true);
        JoystickDot.SetActive(true);
        JoystickCircle.transform.position = touchPosition;
        JoystickDot.transform.position = touchPosition;
    }

    // Deactivate joystick
    public void DeactivateJoystick()
    {
        Vector3 newPosition = transform.position;
        newPosition.z = -0.74f;
        transform.position = newPosition;
        JoystickCircle.SetActive(false);
        JoystickDot.SetActive(false);
        isActive = false;
        SpoonMB.Instance.PourHoneyToJar();
        moveDirection = Vector3.zero;
    }

    // Move spoon based on joystick
    public void MoveSpoon()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, transform.position + moveDirection * movementSpeed, Time.deltaTime);
        newPosition.z = -0.39f;
        transform.position = newPosition;

        JoystickDot.transform.position = touchPosition;
        ClampJoystickDotPosition();

        moveDirection = (JoystickDot.transform.position - JoystickCircle.transform.position).normalized;
    }

    // Clamp joystick dot position within circle
    void ClampJoystickDotPosition()
    {
        JoystickDot.transform.position = new Vector2(
            Mathf.Clamp(JoystickDot.transform.position.x,
            JoystickCircle.transform.position.x - 0.61f,
            JoystickCircle.transform.position.x + 0.61f),
             Mathf.Clamp(JoystickDot.transform.position.y,
            JoystickCircle.transform.position.y - 0.61f,
            JoystickCircle.transform.position.y + 0.61f)
            );
    }
}
