using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance;

    // Dictionary to track pressed buttons by color
    private Dictionary<string, Button> pressedButtonsByColor = new Dictionary<string, Button>();
    
    // Dictionary to track button IDs
    private Dictionary<int, Button> buttonIdDictionary = new Dictionary<int, Button>();

    // Teal button is handled separately
    private bool isTealPressed = false;

    public AudioClip audioClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnButtonPressed(Button pressedButton)
    {
        string color = pressedButton.buttonColor;

        PlayButtonPressSound();

        if (color == "Teal")
        {
            HandleTealButtonPress(pressedButton);
            return;  // Don't process teal buttons in the usual way
        }

        if (pressedButtonsByColor.ContainsKey(color))
        {
            // If the same button is pressed again, reset it and remove it from the color dictionary
            if (pressedButtonsByColor[color] == pressedButton)
            {
                pressedButton.ResetButton();
                pressedButtonsByColor.Remove(color);
            }
            else
            {
                // Reset the previously pressed button of the same color and add the new button to the dictionary
                pressedButtonsByColor[color].ResetButton();
                pressedButtonsByColor[color] = pressedButton;
            }
        }
        else
        {
            // Add the button to the dictionary if no button of this color is pressed
            pressedButtonsByColor[color] = pressedButton;
        }

        buttonIdDictionary[pressedButton.id] = pressedButton;

        if (isTealPressed)
        {
            List<int> buttonIds = new List<int>();

            foreach (var entry in pressedButtonsByColor)
            {
                buttonIds.Add(entry.Value.id);
            }

            SoundPlayer.Instance.PlaySoundsBasedOnPressedButtons(buttonIds);
        }
    }

    // Handle the teal button press (Sound)
    private void HandleTealButtonPress(Button tealButton)
    {
        isTealPressed = !isTealPressed;

        foreach (var entry in pressedButtonsByColor)
        {
            entry.Value.ResetButton();
        }

        if (isTealPressed)
        {
            List<int> buttonIds = new List<int>();

            foreach (var entry in pressedButtonsByColor)
            {
                buttonIds.Add(entry.Value.id);
            }

            SoundPlayer.Instance.PlaySoundsBasedOnPressedButtons(buttonIds);
        }
        else
        {
            pressedButtonsByColor.Clear();
        }
    }

    private void PlayButtonPressSound()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
    }
}
