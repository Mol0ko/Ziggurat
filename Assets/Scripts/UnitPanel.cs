using UnityEngine;
using UnityEngine.UI;

namespace Ziggurat
{
    public class UnitPanel : MonoBehaviour
    {
        [SerializeField]
        private Unit _selectedUnit;
        [SerializeField]
        private Text _armyTypeText;
        [SerializeField]
        private InputField _hpInput;
        [SerializeField]
        private InputField _speedInput;
        [SerializeField]
        private InputField _fastAttackInput;
        [SerializeField]
        private InputField _strongAttackInput;
        [SerializeField]
        private InputField _critChanceInput;
        [SerializeField]
        private InputField _fastStrongAttackRatioInput;
        [SerializeField]
        private GameObject _uiBlocker;

        private bool opened = false;
        private float _closedXPosition = 1237f;
        private float _openedXPosition = 963f;
        private float lerpTimeElapsed = 0.7f;
        private float lerpDuration = 0.7f;

        private void Start()
        {
            OnUnitSelected();
        }

        void Update()
        {
            if (lerpTimeElapsed < lerpDuration)
            {
                var startXPosition = opened ? _closedXPosition : _openedXPosition;
                var endXPosition = opened ? _openedXPosition : _closedXPosition;
                var lerpXPosition = Mathf.Lerp(startXPosition, endXPosition, Mathf.Min(1f, lerpTimeElapsed / lerpDuration));
                transform.position = new Vector2(lerpXPosition, transform.position.y);
                lerpTimeElapsed += Time.deltaTime;
                if (lerpTimeElapsed >= lerpDuration)
                    _uiBlocker.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    var selectedObject = hit.transform.gameObject;
                    if (selectedObject.name.Contains("RPGHeroPolyart"))
                    {
                        _selectedUnit = selectedObject.GetComponent<Unit>();
                        OnUnitSelected();
                        if (!opened)
                            OpenClose();
                        Debug.Log("Select unit: " + _selectedUnit.Model.ArmyType.ToString());
                    }
                }
            }
        }

        public void OpenClose()
        {
            _uiBlocker.SetActive(true);
            opened = !opened;
            lerpTimeElapsed = 0f;
        }

        public void Apply()
        {
            var hp = Mathf.Max(1, float.Parse(_hpInput.text));
            var speed = Mathf.Max(1, float.Parse(_speedInput.text));
            var fastAttackDamage = Mathf.Max(0.1f, float.Parse(_fastAttackInput.text));
            var strongAttackDamage = Mathf.Max(0.1f, float.Parse(_strongAttackInput.text));
            var critChance = Mathf.Min(1f, Mathf.Max(0f, float.Parse(_critChanceInput.text)));
            var fastStrongAttackRatio = Mathf.Min(1f, Mathf.Max(0f, float.Parse(_fastStrongAttackRatioInput.text)));
            _selectedUnit?.UpdateUnitParams(
                hp,
                speed,
                fastAttackDamage,
                strongAttackDamage,
                critChance,
                fastStrongAttackRatio
            );
            OpenClose();
        }

        private void OnUnitSelected()
        {
            if (_selectedUnit != null)
            {
                var unitModel = _selectedUnit.Model;
                _armyTypeText.text = unitModel.ArmyType.ToString();
                _hpInput.text = unitModel.HP.ToString();
                _speedInput.text = unitModel.Speed.ToString();
                _fastAttackInput.text = unitModel.FastAttackDamage.ToString();
                _strongAttackInput.text = unitModel.StrongAttackDamage.ToString();
                _critChanceInput.text = unitModel.CritChance.ToString();
                _fastStrongAttackRatioInput.text = unitModel.FastStrongAttackRatio.ToString();
            }
            else
            {
                _armyTypeText.text = "";
                _hpInput.text = "";
                _speedInput.text = "";
                _fastAttackInput.text = "";
                _strongAttackInput.text = "";
                _critChanceInput.text = "";
                _fastStrongAttackRatioInput.text = "";
            }
        }
    }
}