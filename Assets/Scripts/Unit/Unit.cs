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
        private float _speed;

        public ArmyType ArmyType { get; private set; } = ArmyType.red;
        private IUnitOpponentManager _opponentManager;
        private Unit _opponent;
        private Transform _defaultMoveTarget;
        private bool _moving = false;
        private bool _fighting = false;

        public void SetData(ArmyType armyType, Transform defaultMoveTarget, IUnitOpponentManager opponentManager)
        {
            _speed = 4f;
            ArmyType = armyType;
            _defaultMoveTarget = defaultMoveTarget;
            _opponentManager = opponentManager;
        }

        private void Start()
        {
            switch (ArmyType)
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

            StartMoveTo(_defaultMoveTarget);
        }

        private void Update()
        {
            if (_moving && !_fighting)
            {
                var step = _speed * Time.deltaTime;
                var moveTarget = _opponent != null ? _opponent.transform : _defaultMoveTarget;
                transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, step);
            }

            if (_opponent == null)
            {
                var distanceToCenter = transform.position - _defaultMoveTarget.transform.position;
                if (distanceToCenter.magnitude < 25)
                {
                    _opponent = _opponentManager.GetNextOpponentFor(this);
                    StartMoveTo(_opponent.transform);
                }
            }
            else
            {
                var distanceToOpponent = transform.position - _opponent.transform.position;
                if (distanceToOpponent.magnitude < 1.5)
                    FastAttack();
                else
                    StartMoveTo(_opponent.transform);
            }
        }

        private void StartMoveTo(Transform target)
        {
            transform.LookAt(target);
            _moving = true;
            _animator.SetFloat("Movement", 0.5f);
        }

        private void StopMove()
        {
            _moving = false;
            _animator.SetFloat("Movement", 0f);
        }

        private void FastAttack()
        {
            var playingFastAttackAmination = _animator.GetBool("Fast");
            if (!playingFastAttackAmination)
            {
                StopMove();
                _fighting = true;
                _animator.SetBool("Fast", true);
            }
        }
    }
}