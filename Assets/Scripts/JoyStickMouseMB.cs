using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickMouseMB : MonoBehaviour
{
    public static JoyStickMouseMB Instance;

    public bool isActive;

    public GameObject JoystickCircle, JoystickDot;

    private Rigidbody SpoonRigidbody;

    private float moveSpeed;

    bool mouseMoved;

    private Vector3 touchPosition;

    private Vector3 startPosition;

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
        moveSpeed = 2f;
        mouseMoved = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            ActivateJoystick();
            startPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
   Input.mousePosition.y, 12.52f));
            startPosition.z = 0.0f;
        }

        else if (Input.GetMouseButton(0))
        {
           
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
 Input.mousePosition.y, 12.52f));
            touchPosition.z = 0.0f;
            mouseMoved = (startPosition - touchPosition).sqrMagnitude > 1;

            if (mouseMoved)
                MoveSpoon();
            else
            {
                moveDirection = Vector3.zero;
                JoystickDot.transform.position = JoystickCircle.transform.position;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
			DeactivateJoystick();
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
            JoystickCircle.SetActive(false);
            JoystickDot.SetActive(false);
            isActive = false;
            StartCoroutine(SpoonMB.Instance.PourHoneyToJarCoroutine());
            mouseMoved = false;

    }
    void MoveSpoon()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, transform.position + moveDirection * moveSpeed, Time.deltaTime);
        newPosition.z = -0.44f;
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
