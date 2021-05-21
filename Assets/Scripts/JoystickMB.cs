using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMB : MonoBehaviour
{
    public static JoystickMB Instance;

    public bool isActive;

    public GameObject JoystickCircle, JoystickDot;

    private Rigidbody SpoonRigidbody;

    private float moveSpeed;

    private Touch oneTouch;

    private Vector3 touchPosition;

    private Vector3 moveDirection;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        JoystickCircle.SetActive(false);
        JoystickDot.SetActive(false);
        moveSpeed =2f;
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
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(oneTouch.position.x,
   oneTouch.position.y, 12.52f));
            touchPosition.z = 0.0f;
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
    }

    void ActivateJoystick()
    {
        JoystickCircle.SetActive(true);
        JoystickDot.SetActive(true);
        JoystickCircle.transform.position = touchPosition;
        JoystickDot.transform.position = touchPosition;
    }

    void DeactivateJoystick()
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

    void MoveSpoon()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, transform.position + moveDirection * moveSpeed, Time.deltaTime);
        newPosition.z = -0.39f;
        transform.position = newPosition;

        JoystickDot.transform.position = touchPosition;

        JoystickDot.transform.position = new Vector2(
            Mathf.Clamp(JoystickDot.transform.position.x,
            JoystickCircle.transform.position.x - 0.61f,
            JoystickCircle.transform.position.x + 0.61f),
             Mathf.Clamp(JoystickDot.transform.position.y,
            JoystickCircle.transform.position.y - 0.61f,
            JoystickCircle.transform.position.y + 0.61f)
            );

        moveDirection = (JoystickDot.transform.position - JoystickCircle.transform.position).normalized;
        //SpoonRigidbody.velocity = moveDirection * moveSpeed;
    }
}
