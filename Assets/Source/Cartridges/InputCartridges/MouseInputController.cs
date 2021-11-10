using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalMouseInputController
{
    private static Vector2 mousePosition;
    private static KeyValue mouseButtonValue;
    private static Camera camera;

    public static void InitializeMouseInput()
    {
        mousePosition = Input.mousePosition;
        mouseButtonValue = KeyValue.IDLE;
    }

    public static void SetCamera(Camera cameraIn)
    {
        camera = cameraIn;
    }

    public static void MouseInputUpdate()
    {
        /* Pseudo
         * Get current key value
         * 
         * check Input.GetKey()
         *     | true | false
         *-----|------|---------
         * IDLE| PRES | IDLE
         *-----|------|---------
         * HELD| HELD | UP
         * ----|------|---------
         * PRES| HELD | UP
         * ----|------|---------
         * UP  | PRES | IDLE
         */

        mousePosition.x = Input.mousePosition.x;
        mousePosition.y = Input.mousePosition.y;
        KeyValue frameValue = mouseButtonValue;
        bool inputValue = Input.GetMouseButton(0);

        switch (frameValue)
        {
            case KeyValue.IDLE:
                frameValue = inputValue ? KeyValue.PRESSED : KeyValue.IDLE;
                break;
            case KeyValue.PRESSED:
                frameValue = inputValue ? KeyValue.HELD : KeyValue.UP;
                break;
            case KeyValue.HELD:
                frameValue = inputValue ? KeyValue.HELD : KeyValue.UP;
                break;
            case KeyValue.UP:
                frameValue = inputValue ? KeyValue.PRESSED : KeyValue.IDLE;
                break;
        }

        mouseButtonValue = frameValue;
    }

    public static bool MouseOverItem(RectTransform rect)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, mousePosition, camera);
    }

    public static KeyValue GetMouseClick()
    {
        return mouseButtonValue;
    }

    public static Vector3 GetMousePosition()
    {
        return mousePosition;
    }
}

public class MouseInputController : MonoBehaviour
{
    [SerializeField] private Camera ScreenSpaceCamera;

    // Start is called before the first frame update
    void Start()
    {
        GlobalMouseInputController.InitializeMouseInput();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        GlobalMouseInputController.MouseInputUpdate();
    }
}
