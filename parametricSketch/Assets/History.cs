using System;
using System.Collections.Generic;
using UnityEngine;

public class History
{
    public History(Action<Sketch.Model.Serialization> historyPositionChangedHandler)
    {
        _historyPositionChangedHandler = historyPositionChangedHandler;
    }

    public void AddToHistory(Sketch.Model.Serialization currentModel)
    {
        var json = JsonUtility.ToJson(currentModel);
        _historyPosition = _history.Count;
        System.IO.File.WriteAllText($"{Application.persistentDataPath}/Serial{_historyPosition}.json", json);
        _history.Add(json);
    }

    public Sketch.Model.Serialization Redo()
    {
        if (_historyPosition > 0)
            _historyPosition--;
        return JsonUtility.FromJson<Sketch.Model.Serialization>(_history[_historyPosition]);
    }

    private List<string> _history = new List<string>();
    private int _historyPosition;
    private Action<Sketch.Model.Serialization> _historyPositionChangedHandler;
}