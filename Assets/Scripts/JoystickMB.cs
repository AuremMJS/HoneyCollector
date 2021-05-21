using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMB : MonoBehaviour
{
    public static JoystickMB Instance;

    public bool isActive;

    public float movementSpeed = 2f;

    public GameObject JoystickCircle, JoystickDot;

    private Rigidbody SpoonRigidbody;
    
    private Touch oneTouch;

    public Vector3 touchPosition;

    public Vector3 moveDirection;

    void Awake()
    {
        Debug.Assert(Instance == null, "Cannot create another instance of Singleton class");
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = -0.74f;
            transform.position = newPosition;
            if (SpoonMB.Instance.honeyLevelScaleValue >= 1.0f)
                DeactivateJoystick();
            return;
        }
        if (Input.touchCount > 0)
        {
            oneTouch = Input.GetTouch(0);
            SetTouchPosition(oneTouch.position);
            ProcessTouchPhase();
        }
    }

    public void Init()
    {
        isActive = true;
        JoystickCircle.SetActive(false);
        JoystickDot.SetActive(false);
    }
	
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

    public void SetTouchPosition(Vector3 position)
    {
        touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x,
 position.y, 12.52f));
        touchPosition.z = 0.0f;
    }

    public void ActivateJoystick()
    {
        JoystickCircle.SetActive(true);
        JoystickDot.SetActive(true);
        JoystickCircle.transform.position = touchPosition;
        JoystickDot.transform.position = touchPosition;
    }

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

    public void MoveSpoon()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, transform.position + moveDirection * movementSpeed, Time.deltaTime);
        newPosition.z = -0.39f;
        transform.position = newPosition;

        JoystickDot.transform.position = touchPosition;
        ClampJoystickDotPosition();

        moveDirection = (JoystickDot.transform.position - JoystickCircle.transform.position).normalized;
    }

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
