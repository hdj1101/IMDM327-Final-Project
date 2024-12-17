using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public string buttonColor;
    public bool isPressed = false;
    public int id;

    public Transform buttonHead;
    public Vector3 pressedPosition;
    public Vector3 defaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = buttonHead.localPosition;
        pressedPosition = defaultPosition + Vector3.down * 0.2f;
    }

    // Update is called once per frame
    void Update(){}

    public void ToggleButton()
    {
        isPressed = !isPressed;

        buttonHead.localPosition = isPressed ? pressedPosition : defaultPosition;

        ButtonManager.Instance.OnButtonPressed(this);
    }

    public void ResetButton()
    {
        if (isPressed)
        {
            isPressed = false;
            buttonHead.localPosition = defaultPosition;
        }
    }
}
