using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Model;
using UI;
using UnityEngine;

public static class KeyboardInput
{
    public class Model
    {
        public readonly Parameters Parameters = new Parameters();
        public AxisID? ActiveAxis = null;

        public DimensionInput ActiveDimensionInput
        {
            get => ActiveAxis == null ? null : Parameters.InputInMM[ActiveAxis.Value];
            set
            {
                if (ActiveAxis == null)
                    ActiveAxis = AxisID.X;
                Parameters.InputInMM[ActiveAxis.Value] = value;
            }
        }

        public Parameter CurrentlyReferencesParameter =>
            ActiveAxis.HasValue ? Parameters.ParameterReferences[ActiveAxis.Value] : null;
    }

    public class Parameters
    {
        public readonly Vec<bool> IsDirectionNegative = new Vec<bool>(false);
        public readonly Vec<Parameter> ParameterReferences = new Vec<Parameter>(null);
        public readonly Vec<DimensionInput> InputInMM = new Vec<DimensionInput>(null);
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
        if (availableParameters.Count == 0)
            return;

        if (!model.ActiveAxis.HasValue)
            model.ActiveAxis = AxisID.X;

        var currentlySelectedParameter = model.Parameters.ParameterReferences[model.ActiveAxis.Value];
        if (currentlySelectedParameter == null)
        {
            model.Parameters.ParameterReferences[model.ActiveAxis.Value] = availableParameters[0];
            return;
        }

        //get next in list
        var selectedIndex = availableParameters.IndexOf(currentlySelectedParameter);
        selectedIndex++;
        selectedIndex %= availableParameters.Count;


        model.Parameters.ParameterReferences[model.ActiveAxis.Value] = availableParameters[selectedIndex];
    }

    private static void SetNextAxis(Model model)
    {
        if (!model.ActiveAxis.HasValue)
            model.ActiveAxis = AxisID.X;
        else if (model.ActiveAxis.Value == AxisID.X)
            model.ActiveAxis = AxisID.Z;
        else // if ==Z
            model.ActiveAxis = AxisID.X;
    }

    private static void InvertDirection(Model model)
    {
        if (model.ActiveAxis == null)
            return;

        model.Parameters.IsDirectionNegative[model.ActiveAxis.Value] =
            !model.Parameters.IsDirectionNegative[model.ActiveAxis.Value];
    }

    private static void RemoveInputStep(Model model)
    {
        if (!model.ActiveAxis.HasValue)
            return;

        if (model.Parameters.ParameterReferences[model.ActiveAxis.Value] != null)
        {
            model.Parameters.ParameterReferences[model.ActiveAxis.Value] = null;
            return;
        }

        if (model.ActiveDimensionInput == null)
            return;

        if (model.ActiveDimensionInput.InMM < 10)
        {
            model.ActiveDimensionInput = null;
            return;
        }

        model.ActiveDimensionInput.InMM /= 10;
    }

    private static void AddDigit(Model model, int digit)
    {
        if (model.ActiveDimensionInput == null)
        {
            model.ActiveDimensionInput = new DimensionInput() {InMM = digit};
            return;
        }

        var currentValue = model.ActiveDimensionInput.InMM;
        model.ActiveDimensionInput.InMM = currentValue * 10 + digit;
    }
}