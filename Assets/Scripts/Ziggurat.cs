using System;
using System.Collections;
using UnityEngine;

namespace Ziggurat
{
    public enum ArmyType { red, green, blue }

    public class Ziggurat : MonoBehaviour
    {
        [SerializeField]
        private UnitModel _unitModel;
        [SerializeField]
        private GameObject _unitPrefab;
        [SerializeField]
        private int _createUnitIntervalSeconds = 5;
        [SerializeField]
        private AllUnitsManager _allUnitsManager;
        [SerializeField]
        private Transform _centerObject;

        private void Start()
        {
            StartCoroutine(UnitGeneration());
        }

        private IEnumerator UnitGeneration()
        {
            while (enabled)
            {
                var unitObject = Instantiate(_unitPrefab, transform);
                unitObject.transform.localPosition = new Vector3(0, 10, 0);
                var unitComponent = unitObject.GetComponent<Unit>();
                unitComponent.SetData(_unitModel.Copy(), _centerObject, _allUnitsManager);
                _allUnitsManager.OnCreateUnit(unitComponent);
                yield return new WaitForSeconds(_createUnitIntervalSeconds);
            }
        }
    }
}