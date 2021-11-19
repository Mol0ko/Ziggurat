using UnityEngine;

namespace Ziggurat
{
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private SkinnedMeshRenderer _meshRender;
        [SerializeField]
        private Material _redMaterial;
        [SerializeField]
        private Material _greenMaterial;
        [SerializeField]
        private Material _blueMaterial;
        [SerializeField, Range(1f, 15f)]
        private float _speed = 5f;

        private ArmyType _armyType = ArmyType.red;
        private Transform _defaultMoveTarget;
        private Transform _moveTarget;
        private bool _moving = false;

        public void SetData(ArmyType armyType, Transform defaultMoveTarget)
        {
            _armyType = armyType;
            _defaultMoveTarget = defaultMoveTarget;
        }

        private void Start()
        {
            switch (_armyType)
            {
                case ArmyType.red:
                    _meshRender.material = _redMaterial;
                    break;
                case ArmyType.green:
                    _meshRender.material = _greenMaterial;
                    break;
                case ArmyType.blue:
                    _meshRender.material = _blueMaterial;
                    break;
            };

            StartMove(_defaultMoveTarget);
        }

        private void Update()
        {
            if (_moving)
            {
                var step = _speed * Time.deltaTime;
                var target = _moveTarget != null ? _moveTarget : _defaultMoveTarget;
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);

                var diff = transform.position - target.position;
                if (diff.magnitude < 5)
                    StopMove();
            }
        }

        private void StartMove(Transform direction)
        {
            transform.LookAt(direction);
            _moving = true;
            _animator.SetFloat("Movement", 0.5f);
        }

        private void StopMove()
        {
            _moving = false;
            _animator.SetFloat("Movement", 0f);
        }
    }
}