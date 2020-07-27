using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class GeometryUIManager : MonoBehaviour
    {
        [SerializeField] private GeometryUI _rectangleFillingUIPrefab;

        public void UpdateUI(List<GeometryModel> geometryModel, GeometryStyleAsset.GeometryStyleSet styleSet)
        {
            UpdatePoolSize(geometryModel.Count);
            for (var i = 0; i < geometryModel.Count; i++)
            {
                _uiPool[i].UpdateUI(geometryModel[i], styleSet);
            }
        }

        private void UpdatePoolSize(int count)
        {
            while (_uiPool.Count > count)
            {
                var rectangleToDestroy = _uiPool[0];
                _uiPool.Remove(rectangleToDestroy);
                Destroy(rectangleToDestroy.gameObject);
            }

            while (_uiPool.Count < count)
            {
                var newUI = Instantiate(_rectangleFillingUIPrefab, transform);
                _uiPool.Add(newUI);
            }
        }

        private readonly List<GeometryUI> _uiPool = new List<GeometryUI>();
    }
}