using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class RectanglesUI : MonoBehaviour
    {
        [SerializeField] private Rectangle _rectanglePrefab;

        public void UpdateUI(List<Sketch.RectangleModel> rectangleModels)
        {
            foreach (var rectangle in _rectangles)
            {
                if (Application.isEditor)
                    Destroy(rectangle.gameObject);
                else
                    Destroy(rectangle.gameObject);
            }

            _rectangles.Clear();

            foreach (var rectangleModel in rectangleModels)
            {
                if (rectangleModel.P0.HasValue && rectangleModel.P1.HasValue)
                {
                    var p0 = rectangleModel.P0.Value;
                    var p1 = rectangleModel.P1.Value;

                    var newUI = Instantiate(_rectanglePrefab, transform);
                    _rectangles.Add(newUI);
                    newUI.Initialize();
                    newUI.UpdateUI(p0.x.Value, p1.x.Value, p0.y.Value, p1.y.Value, p0.z.Value,p1.z.Value);
                }
            }
        }

        private List<Rectangle> _rectangles = new List<Rectangle>();
    }
}