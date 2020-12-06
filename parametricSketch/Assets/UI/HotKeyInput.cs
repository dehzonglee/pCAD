using UnityEngine;

namespace UI
{
    public static class HotKeyInput
    {
        public static Command? Update()
        {
            if (Input.GetKeyDown(TransformKey))
                return Command.Transform;
            if (Input.GetKeyDown(DrawPointKey))
                return Command.DrawPoint;
            if (Input.GetKeyDown(DrawLineKey))
                return Command.DrawLine;
            if (Input.GetKeyDown(DrawRectKey))
                return Command.DrawRect;
            if (Input.GetKeyDown(UndoKey))
                return Command.Undo;
            if (Input.GetKeyDown(RedoKey))
                return Command.Redo;
            return null;
        }



        private const KeyCode TransformKey = KeyCode.Alpha1;
        private const KeyCode DrawLineKey = KeyCode.Alpha3;
        private const KeyCode DrawPointKey = KeyCode.Alpha2;
        private const KeyCode DrawRectKey = KeyCode.Alpha4;
        private const KeyCode UndoKey = KeyCode.Z;
        private const KeyCode RedoKey = KeyCode.Y;
    }
}