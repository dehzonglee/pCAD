using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LinesUI : MonoBehaviour
    {
        [SerializeField] private LineUI _pointUIPrefab;

        public void UpdateUI(List<Sketch.LineModel> lineModels, GeometryStyle.LineStyleSet styleSet)
        {
            while (_uiPool.Count > lineModels.Count)
            {
                var uiToDestroy = _uiPool[0];
                _uiPool.Remove(uiToDestroy);
                Destroy(uiToDestroy.gameObject);
            }

            while (_uiPool.Count < lineModels.Count)
            {
                var newUI = Instantiate(_pointUIPrefab, transform);
                _uiPool.Add(newUI);
            }

            for (var i = 0; i < lineModels.Count; i++)
            {
                var model = lineModels[i];
                var p0 = model.P0;
                var p1 = model.P1;
                var style = model.IsBaked ? styleSet.DefaultStyle : styleSet.FocusStyle;
                _uiPool[i].UpdateUI(CoordinateTupleToVector3(p0), CoordinateTupleToVector3(p1), style);
            }
        }

        private static Vector3 CoordinateTupleToVector3(Vec<Coordinate> tuple)
        {
            return new Vector3(tuple.X.Value, tuple.Y.Value, tuple.Z.Value);
        }

        private readonly List<LineUI> _uiPool = new List<LineUI>();
    }
}