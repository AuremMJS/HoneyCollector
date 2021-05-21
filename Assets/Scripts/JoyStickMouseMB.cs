using UnityEngine;

// Class to simulate joystick behaviour using mouse in PC
public class JoyStickMouseMB : MonoBehaviour
{
    // Has the mouse moved
    bool mouseMoved;

    // Start position of the mouse movement
    private Vector3 startPosition;

    // Reference to joystick singleton instance
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
            // Keep the spoon away from honey comb when joystick is not active
            Vector3 newPosition = transform.position;
            newPosition.z = -0.74f;
            transform.position = newPosition;
            
            // Deactivate the joystick
            if (SpoonMB.Instance.honeyLevelScaleValue >= 1.0f)
            {
                joystick.DeactivateJoystick();
                mouseMoved = false;
            }
            return;
        }

        // Mouse Events
        if (!OnMouseDown())
            if (!OnMouseHold())
                OnMouseUp();
    }

    // When mouse is pressed down
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

    // When mouse is held and moved
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

    // When mouse is released up
    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            joystick.DeactivateJoystick();
            mouseMoved = false;
        }
    }
}
