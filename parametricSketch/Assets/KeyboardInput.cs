using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyboardInput
{
    public static void UpdateKeyboardInput(ref Sketch.KeyboardInputModel model)
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
            AddDigit(model, 0);
        if (Input.GetKeyDown(KeyCode.Keypad1))
            AddDigit(model, 1);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            AddDigit(model, 2);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            AddDigit(model, 3);
        if (Input.GetKeyDown(KeyCode.Keypad4))
            AddDigit(model, 4);
        if (Input.GetKeyDown(KeyCode.Keypad5))
            AddDigit(model, 5);
        if (Input.GetKeyDown(KeyCode.Keypad6))
            AddDigit(model, 6);
        if (Input.GetKeyDown(KeyCode.Keypad7))
            AddDigit(model, 7);
        if (Input.GetKeyDown(KeyCode.Keypad8))
            AddDigit(model, 8);
        if (Input.GetKeyDown(KeyCode.Keypad9))
            AddDigit(model, 9);
        if (Input.GetKeyDown(KeyCode.Backspace))
            RemoveDigit(model);
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            InvertDirection(model);
        if (Input.GetKeyDown(KeyCode.Tab))
            SetNextAxis(model);
    }

    private static void SetNextAxis(Sketch.KeyboardInputModel model)
    {
        model.SetNextAxis();
    }

    private static void InvertDirection(Sketch.KeyboardInputModel model)
    {
        model.IsDirectionNegative = !model.IsDirectionNegative;
    }

    private static void RemoveDigit(Sketch.KeyboardInputModel model)
    {
        if (!model.ActiveInputInMM.HasValue)
            return;

        if (model.ActiveInputInMM.Value < 10)
        {
            model.ActiveInputInMM = null;
            return;
        }

        model.ActiveInputInMM = model.ActiveInputInMM.Value / 10;
    }

    private static void AddDigit(Sketch.KeyboardInputModel model, int digit)
    {
        if (!model.ActiveInputInMM.HasValue)
        {
            model.ActiveInputInMM = digit;
            return;
        }

        var currentValue = model.ActiveInputInMM.Value;
        model.ActiveInputInMM = currentValue * 10 + digit;
    }
}