using System;
using System.Collections.Generic;
using UnityEngine;

public static class KeyboardInput
{
    [Serializable]
    public class Model
    {
        public readonly Vec<bool> IsDirectionNegative = new Vec<bool>(false);
        public readonly Vec<Parameter> ParameterReferences = new Vec<Parameter>();
        public readonly Vec<DimensionInput> DimensionInput = new Vec<DimensionInput>();
        public Vec.AxisID? ActiveAxis = null;
    }

    public class DimensionInput
    {
        public int InMM;
        public float InM => InMM * 0.01f;
    }

    public static void UpdateKeyboardInput(ref Model model, List<Parameter> availableParameters)
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
            RemoveInputStep(model);
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            InvertDirection(model);
        if (Input.GetKeyDown(KeyCode.Tab))
            SetNextAxis(model);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            SelectNextParameter(model, availableParameters);
    }

    private static void SelectNextParameter(Model model, List<Parameter> availableParameters)
    {
        // there are no parameters to select from
        if (availableParameters.Count == 0)
            return;

        // no axis selected, default select x axis
        if (!model.ActiveAxis.HasValue)
            model.ActiveAxis = Vec.AxisID.X;

        // no parameter selected yet, select first in list  
        var currentlySelectedParameter = model.ParameterReferences[model.ActiveAxis.Value];
        if (currentlySelectedParameter == null)
        {
            model.ParameterReferences[model.ActiveAxis.Value] = availableParameters[0];
            return;
        }

        // get next in the list
        var selectedIndex = availableParameters.IndexOf(currentlySelectedParameter);
        selectedIndex++;
        selectedIndex %= availableParameters.Count;

        model.ParameterReferences[model.ActiveAxis.Value] = availableParameters[selectedIndex];
    }

    private static void SetNextAxis(Model model)
    {
        if (!model.ActiveAxis.HasValue)
            model.ActiveAxis = Vec.AxisID.X;
        else if (model.ActiveAxis.Value == Vec.AxisID.X)
            model.ActiveAxis = Vec.AxisID.Z;
        else // if ==Z
            model.ActiveAxis = Vec.AxisID.X;
    }

    private static void InvertDirection(Model model)
    {
        if (model.ActiveAxis == null)
            return;

        model.IsDirectionNegative[model.ActiveAxis.Value] = !model.IsDirectionNegative[model.ActiveAxis.Value];
    }

    private static void RemoveInputStep(Model model)
    {
        // there is no input, do nothing
        if (!model.ActiveAxis.HasValue)
            return;

        var a = model.ActiveAxis.Value;

        // there is a parameter referenced, remove it
        if (model.ParameterReferences[a] != null)
        {
            model.ParameterReferences[a] = null;
            model.ActiveAxis = null;
            return;
        }

        // there is no dimension input, do nothing
        if (model.DimensionInput[a] == null)
            return;

        // there is only one digit dimension input, remove it
        if (model.DimensionInput[a].InMM < 10)
        {
            model.DimensionInput[a] = null;
            model.ActiveAxis = null;
            return;
        }

        // there was is a multi digit input, shorten it 
        model.DimensionInput[model.ActiveAxis.Value].InMM /= 10;
    }

    private static void AddDigit(Model model, int digit)
    {
        // select first axis if nothing selected
        if (model.ActiveAxis == null)
            model.ActiveAxis = Vec.AxisID.X;

        var a = model.ActiveAxis.Value;

        // set the first digit
        if (model.DimensionInput[a] == null)
        {
            model.DimensionInput[a] = new DimensionInput() {InMM = digit};
            return;
        }

        // add another digit in the end
        var currentValue = model.DimensionInput[a].InMM;
        model.DimensionInput[a].InMM = currentValue * 10 + digit;
    }
}