using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class History
{
    public History(Action<Sketch.Model.Serialization> historyPositionChangedHandler)
    {
        _historyPositionChangedHandler = historyPositionChangedHandler;
    }

    public void AddToHistory(Sketch.Model.Serialization currentModel)
    {
        //remove steps that lie ahead of current position
        _history = _history.Take(_historyPosition + 1).ToList();

        var json = JsonUtility.ToJson(currentModel);
        _historyPosition = _history.Count;
        System.IO.File.WriteAllText($"{Application.persistentDataPath}/Serial{_historyPosition}.json", json);
        _history.Add(json);
    }

    public Sketch.Model.Serialization Undo()
    {
        if (_historyPosition > 0)
            _historyPosition--;
        return JsonUtility.FromJson<Sketch.Model.Serialization>(_history[_historyPosition]);
    }

    
    public Sketch.Model.Serialization Redo()
    {
        if (_historyPosition < _history.Count -1)
            _historyPosition++;
        return JsonUtility.FromJson<Sketch.Model.Serialization>(_history[_historyPosition]);
    }
    
    private List<string> _history = new List<string>();
    private int _historyPosition;
    private Action<Sketch.Model.Serialization> _historyPositionChangedHandler;
}