using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class RectanglesUI : MonoBehaviour
    {
        [SerializeField] private RectangleUI rectangleUIPrefab;
        [SerializeField] private RectangleUI2D rectangleUI2DPrefab;

        public void UpdateUI(List<Sketch.RectangleModel> rectangleModels)
        {
            var validRectangles = rectangleModels.Where(rm => rm.P0.HasValue && rm.P1.HasValue).ToList();
            while (_uiPool.Count > validRectangles.Count)
            {
                var rectangleToDestroy = _uiPool[0];
                _uiPool.Remove(rectangleToDestroy);
                Destroy(rectangleToDestroy.ui.gameObject);
                Destroy(rectangleToDestroy.ui2D.gameObject);
            }

            while (_uiPool.Count < validRectangles.Count)
            {
                var newUI = Instantiate(rectangleUIPrefab, transform);
                var newUI2D = Instantiate(rectangleUI2DPrefab, transform);
                _uiPool.Add((newUI,newUI2D));
                newUI.Initialize();
            }

            for (var i = 0; i < rectangleModels.Count; i++)
            {
                var rectangleModel = validRectangles[i];
                var p0 = rectangleModel.P0.Value;
                var p1 = rectangleModel.P1.Value;

                _uiPool[i].ui.UpdateUI(p0.x.Value, p1.x.Value, p0.y.Value, p1.y.Value, p0.z.Value, p1.z.Value);
                _uiPool[i].ui2D.UpdateCoordinates((p0.x.Value, p1.x.Value), (p0.z.Value, p1.z.Value));
            }
        }

        private readonly List<(RectangleUI ui,RectangleUI2D ui2D)> _uiPool = new List<(RectangleUI ui,RectangleUI2D ui2D)>();
    }
}