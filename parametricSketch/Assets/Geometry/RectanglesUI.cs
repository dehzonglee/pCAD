using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class RectanglesUI : MonoBehaviour
    {
        [FormerlySerializedAs("rectangleFillingUI2DPrefab")] [SerializeField]
        private RectangleFillingUI rectangleFillingUIPrefab;

        [FormerlySerializedAs("rectangleOutlineUI2DPrefab")] [SerializeField]
        private RectangleOutlineUI rectangleOutlineUIPrefab;

        public void UpdateUI(List<Sketch.RectangleModel> rectangleModels, GeometryStyle.RectangleStyleSet styleSet)
        {
            var validRectangles = rectangleModels.Where(rm => rm.P0.HasValue && rm.P1.HasValue).ToList();
            while (_uiPool.Count > validRectangles.Count)
            {
                var rectangleToDestroy = _uiPool[0];
                _uiPool.Remove(rectangleToDestroy);
                Destroy(rectangleToDestroy.filling.gameObject);
                Destroy(rectangleToDestroy.outline.gameObject);
            }

            while (_uiPool.Count < validRectangles.Count)
            {
                var newFillingUI = Instantiate(rectangleFillingUIPrefab, transform);
                var newOutlineUI = Instantiate(rectangleOutlineUIPrefab, transform);
                _uiPool.Add(new UIComponents() {filling = newFillingUI, outline = newOutlineUI});
            }

            for (var i = 0; i < rectangleModels.Count; i++)
            {
                var rectangleModel = validRectangles[i];
                var p0 = rectangleModel.P0.Value;
                var p1 = rectangleModel.P1.Value;
                var style = rectangleModel.IsBaked ? styleSet.DefaultStyle : styleSet.FocusStyle;
                _uiPool[i].filling.UpdateUI(CoordinateTupleToVector3(p0), CoordinateTupleToVector3(p1),style);
                _uiPool[i].outline.UpdateUI(CoordinateTupleToVector3(p0), CoordinateTupleToVector3(p1),style);
            }
        }

        private static Vector3 CoordinateTupleToVector3((Coordinate x, Coordinate y, Coordinate z) tuple)
        {
            return new Vector3(tuple.x.Value, tuple.y.Value, tuple.z.Value);
        }

        private readonly List<UIComponents> _uiPool = new List<UIComponents>();

        private struct UIComponents
        {
            public RectangleFillingUI filling;
            public RectangleOutlineUI outline;
        }
    }
}