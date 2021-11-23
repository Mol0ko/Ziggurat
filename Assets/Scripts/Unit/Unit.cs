using UnityEngine;

namespace Ziggurat
{
    public class Unit : MonoBehaviour
    {
        #region SerializeField

        [SerializeField]
        private UnitEnvironment _unitEnvironment;
        [SerializeField]
        private SkinnedMeshRenderer _meshRender;
        [SerializeField]
        private Material _redMaterial;
        [SerializeField]
        private Material _greenMaterial;
        [SerializeField]
        private Material _blueMaterial;

        #endregion

        public UnitModel Model { get; private set; }
        private IUnitOpponentManager _opponentManager;
        public Unit Opponent { get; private set; }
        private Transform _defaultMoveTarget;

        public void SetData(UnitModel model, Transform defaultMoveTarget, IUnitOpponentManager opponentManager)
        {
            Model = model;
            _defaultMoveTarget = defaultMoveTarget;
            _opponentManager = opponentManager;
        }

        private void Start()
        {
            switch (Model.ArmyType)
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
            Model.State = UnitState.Move;
            _unitEnvironment.AnimateMoveTo(_defaultMoveTarget);
        }

        private void Update()
        {
            if (Model.State == UnitState.Dead)
                return;

            if (Model.HP <= 0 && Model.State != UnitState.Dead)
            {
                Model.State = UnitState.Dead;
                _unitEnvironment.OnEndAnimation += OnEndDieAnimation;
                _unitEnvironment.AnimateDie();
                return;
            }

            if (Model.State == UnitState.Move)
            {
                var step = Model.Speed * Time.deltaTime;
                var moveTarget = Opponent != null ? Opponent.transform : _defaultMoveTarget;
                transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, step);
            }

            if (Opponent == null)
            {
                var distanceToCenter = transform.position - _defaultMoveTarget.transform.position;
                if (distanceToCenter.magnitude < 25)
                {
                    Opponent = _opponentManager.GetNextOpponentFor(this);
                    if (Opponent != null)
                        _unitEnvironment.AnimateMoveTo(Opponent.transform);
                }
            }
            else if (Model.State != UnitState.FastAttack)
            {
                var distanceToOpponent = transform.position - Opponent.transform.position;
                if (distanceToOpponent.magnitude < 1.5)
                {
                    Model.State = UnitState.FastAttack;
                    AttackOpponent();
                }
                else
                {
                    Model.State = UnitState.Move;
                    _unitEnvironment.AnimateMoveTo(Opponent.transform);
                }
            }
        }

        private void AttackOpponent()
        {
            _unitEnvironment.OnEndAnimation += OnEndAttackAnimation;
            _unitEnvironment.AnimateFastAttack();
        }

        private void OnEndAttackAnimation(object sender, System.EventArgs e)
        {
            _unitEnvironment.OnEndAnimation -= OnEndAttackAnimation;
            Model.State = UnitState.Idle;
        }

        private void OnEndDieAnimation(object sender, System.EventArgs e)
        {
            _unitEnvironment.OnEndAnimation -= OnEndDieAnimation;
            _opponentManager.OnUnitDied(this);
        }
    }
}