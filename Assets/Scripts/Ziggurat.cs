using System;
using System.Collections;
using UnityEngine;

namespace Ziggurat
{
    public enum ArmyType { red, green, blue }

    public class Ziggurat : MonoBehaviour
    {
        [SerializeField]
        private ArmyType _armyType;
        [SerializeField]
        private GameObject _unitPrefab;
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
                unitComponent.SetData(_armyType, _centerObject, _allUnitsManager);
                _allUnitsManager.OnCreateUnit(unitComponent);
                yield return new WaitForSeconds(3);
            }
        }
    }
}