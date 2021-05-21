using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickMouseMB : MonoBehaviour
{
    bool mouseMoved;

    private Vector3 startPosition;

    JoystickMB joystick;

    // Start is called before the first frame update
    void Start()
    {
        joystick = JoystickMB.Instance;
        joystick.Init();
        mouseMoved = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!joystick.isActive)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = -0.74f;
            transform.position = newPosition;
            if (SpoonMB.Instance.honeyLevelScaleValue >= 1.0f)
            {
                joystick.DeactivateJoystick();
                mouseMoved = false;
            }
            return;
        }

        if (!OnMouseDown())
            if (!OnMouseHold())
                OnMouseUp();
    }

    bool OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            joystick.SetTouchPosition(Input.mousePosition);
            startPosition = joystick.touchPosition;

            joystick.ActivateJoystick();
            return true;
        }
        return false;
    }

    bool OnMouseHold()
    {
        if (Input.GetMouseButton(0))
        {

            joystick.SetTouchPosition(Input.mousePosition);
            mouseMoved = (startPosition - joystick.touchPosition).sqrMagnitude > 5;

            if (mouseMoved)
                joystick.MoveSpoon();
            else
            {
                joystick.moveDirection = Vector3.zero;
                joystick.JoystickDot.transform.position = joystick.JoystickCircle.transform.position;
            }
            return true;
        }
        return false;
    }

    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            joystick.DeactivateJoystick();
            mouseMoved = false;
        }
    }
}
