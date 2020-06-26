using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public static class KeyboardInput
{
    public class Model
    {
        public int? ActiveInputInMM
        {
            get => _inputInMM[_activeInputIndex];
            set => _inputInMM[_activeInputIndex] = value;
        }

        public bool IsDirectionNegative;

        public float? XInM => _inputInMM[0] * 0.01f * (IsDirectionNegative ? -1f : 1f);
        public float? ZInM => _inputInMM[1] * 0.01f * (IsDirectionNegative ? -1f : 1f);

        public Axis.GenericVector<float?> InputVector => new Axis.GenericVector<float?>(XInM, null, ZInM);

        public Axis.GenericVector<bool> SelectionVector =>
            new Axis.GenericVector<bool>(_activeInputIndex == 0, false, _activeInputIndex == 1);

        private int _activeInputIndex = 0;
        private readonly int?[] _inputInMM = new int?[2];

        public void SetNextAxis()
        {
            _activeInputIndex++;
            _activeInputIndex %= _inputInMM.Length;
        }

        public void Reset()
        {
            _inputInMM[0] = null;
            _inputInMM[1] = null;
        }
    }


    public static void UpdateKeyboardInput(ref Model model)
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

    private static void SetNextAxis(Model model)
    {
        model.SetNextAxis();
    }

    private static void InvertDirection(Model model)
    {
        model.IsDirectionNegative = !model.IsDirectionNegative;
    }

    private static void RemoveDigit(Model model)
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

    private static void AddDigit(Model model, int digit)
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