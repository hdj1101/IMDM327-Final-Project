using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI elements

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    public Transform playerBody;
    public float interactionDistance = 5f; // Max distance for interacting with buttons (Didnt really use this)

    public Image cursorImage;
    public Sprite cursorNonSprite;
    public Sprite cursorYesSprite;

    public bool watching = false;

    private bool isCursorLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCursorToggle();

        if (isCursorLocked)
        {
            CameraMovement();
            HandleInteraction();
        }
    }

    void CameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(-mouseY, mouseX, 0);

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleInteraction()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Button button = hit.collider.GetComponent<Button>();

            if (button != null)
                watching = true;
            else watching = false;

            if (watching)
            {
                cursorImage.sprite = cursorYesSprite;

                if (Input.GetMouseButtonDown(0)) // Left mouse button
                {
                    button.ToggleButton();
                }
            }
            else
            {
                cursorImage.sprite = cursorNonSprite;
            }
        }
        else
        {
            cursorImage.sprite = cursorNonSprite;
        }
    }

    void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorLocked = !isCursorLocked;

            if (isCursorLocked)
            {
                LockCursor();
            }
            else
            {
                UnlockCursor();
            }
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
