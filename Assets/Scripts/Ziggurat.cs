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

        public UnitModel UnitModel
        {
            get => _unitModel.Copy();
        }

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

        public void UpdateUnitParams(
            float hp,
            float speed,
            float fastAttackDamage,
            float strongAttackDamage,
            float critChance,
            float fastStrongAttackRatio)
        {
            _unitModel.HP = hp;
            _unitModel.Speed = speed;
            _unitModel.FastAttackDamage = fastAttackDamage;
            _unitModel.StrongAttackDamage = strongAttackDamage;
            _unitModel.CritChance = critChance;
            _unitModel.FastStrongAttackRatio = fastStrongAttackRatio;
        }
    }
}