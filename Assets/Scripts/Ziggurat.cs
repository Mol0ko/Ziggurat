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
                unitObject.GetComponent<Unit>().SetData(_armyType, _centerObject);
                Debug.Log("Create unit: " + _armyType);
                yield return new WaitForSeconds(3);
            }
        }
    }
}