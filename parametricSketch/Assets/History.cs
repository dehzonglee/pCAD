using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class History
{
    public History(Action<SketchModel.Serialization> historyPositionChangedHandler)
    {
        _historyPositionChangedHandler = historyPositionChangedHandler;
    }

    public void AddToHistory(SketchModel.Serialization currentModel)
    {
        //remove steps that lie ahead of current position
        _history = _history.Take(_historyPosition + 1).ToList();

        var json = JsonUtility.ToJson(currentModel);
        _historyPosition = _history.Count;
        System.IO.File.WriteAllText($"{Application.persistentDataPath}/Serial{_historyPosition}.json", json);
        _history.Add(json);
    }

    public SketchModel.Serialization Undo()
    {
        if (_historyPosition > 0)
            _historyPosition--;
        return JsonUtility.FromJson<SketchModel.Serialization>(_history[_historyPosition]);
    }

    
    public SketchModel.Serialization Redo()
    {
        if (_historyPosition < _history.Count -1)
            _historyPosition++;
        return JsonUtility.FromJson<SketchModel.Serialization>(_history[_historyPosition]);
    }
    
    private List<string> _history = new List<string>();
    private int _historyPosition;
    private Action<SketchModel.Serialization> _historyPositionChangedHandler;
}