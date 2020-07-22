using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class PointsUI : MonoBehaviour
    {
 [FormerlySerializedAs("_rectangleFillingUIPrefab")] [SerializeField] private PointUI _pointUIPrefab;

        public void UpdateUI(List<Sketch.PointModel> pointModels, GeometryStyle.PointStyleSet styleSet)
        {
            while (_uiPool.Count > pointModels.Count)
            {
                var uiToDestroy = _uiPool[0];
                _uiPool.Remove(uiToDestroy);
                Destroy(uiToDestroy.gameObject);
            }

            while (_uiPool.Count < pointModels.Count)
            {
                var newFillingUI = Instantiate(_pointUIPrefab, transform);
                _uiPool.Add(newFillingUI);
            }

            for (var i = 0; i < pointModels.Count; i++)
            {
                var pointModel = pointModels[i];
                var p0 = pointModel.P0;
                var style = pointModel.IsBaked ? styleSet.DefaultStyle : styleSet.FocusStyle;
                _uiPool[i].UpdateUI(CoordinateTupleToVector3(p0),  style);
            }
        }

        private static Vector3 CoordinateTupleToVector3(Vec<Coordinate> tuple)
        {
            return new Vector3(tuple.X.Value, tuple.Y.Value, tuple.Z.Value);
        }

        private readonly List<PointUI> _uiPool = new List<PointUI>();

    }
}