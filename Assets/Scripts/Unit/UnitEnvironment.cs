using System;
using UnityEngine;

namespace Ziggurat
{
    [RequireComponent(typeof(Animator))]
    public class UnitEnvironment : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Collider _swordCollider;

        /// <summary>
        /// Событие, вызываемое по окончанию анимации
        /// </summary>
        public event EventHandler OnEndAnimation;

        public void AnimateMoveTo(Transform target)
        {
            transform.LookAt(target);
            _animator.SetFloat("Movement", 0.5f);
        }

        public void StopMoveAnimation()
        {
            _animator.SetFloat("Movement", 0f);
        }

        public void AnimateFastAttack()
        {
            var playingAmination = _animator.GetBool("Fast");
            if (!playingAmination)
            {
                StopMoveAnimation();
                _animator.SetBool("Fast", true);
            }
        }

        public void AnimateStrongAttack()
        {
            var playingAmination = _animator.GetBool("Strong");
            if (!playingAmination)
            {
                StopMoveAnimation();
                _animator.SetBool("Strong", true);
            }
        }

        public void AnimateDie()
        {
            var playingAmination = _animator.GetBool("Die");
            if (!playingAmination)
            {
                StopMoveAnimation();
                _animator.SetBool("Die", true);
            }
        }

        //Вызывается внутри анимаций для переключения атакующего коллайдера
        private void AnimationEventCollider_UnityEditor(int isActivity)
        {
            _swordCollider.enabled = isActivity != 0;
            if (isActivity != 0)
                Debug.Log("Sword collider activated");
            else
                Debug.Log("Sword collider deactivated");
        }

        //Вызывается внутри анимаций для оповещения об окончании анимации
        private void AnimationEventEnd_UnityEditor(string result)
        {
            //В конце анимации смерти особый аргумент и своя логика обработки
            if (result == "die") {
                Destroy(gameObject);
            };
            OnEndAnimation.Invoke(null, null);
        }

    }
}
