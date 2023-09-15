using System;
using UnityEngine;
using UnityEngine.Events;

namespace Volorf.GazeTrigger
{
    [Serializable]
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down,
        Forward,
        Back
    }

    public class PalmMenuTrigger : MonoBehaviour
    {
        [SerializeField] private Direction direction;
        
        [Space(5)]
        [Header("Elements")] 
        public Transform vrController;
        public Transform cameraTrans;

        [Space(5)] [Header("Trigger Settings")] 
        [SerializeField] private float lookAtTrigger = 0.8f;
        [SerializeField] private float lookAtSecondTrigger = 0.9f;
        [SerializeField] private float lookAwayTrigger = 0.6f;
        [SerializeField] private float lookAwaySecondTrigger = 0.7f;
        [SerializeField] private bool hideAtStart;

        [Space(5)] [Header("Events")] 
        public UnityEvent onShow;
        public UnityEvent onHide;
        public UnityEvent onSecondShow;
        public UnityEvent onSecondHide;

        [Space(5)] 
        [Header("Debug Dot")] 
        [SerializeField] private bool usePalmMenuDotProd;
        public PamlMenuDotProdEvent pamlMenuDotProd;

        // PRIVATE
        private bool _hasBeenShown;
        private bool _hasBeenShownForSecond;

        

        private Vector3 _vrInputDir;

        private void Start()
        {
            if (!hideAtStart) onHide.Invoke();
        }

        private void Update()
        {
            
            switch (direction)
            {
                case Direction.Right: _vrInputDir = vrController.transform.right; break;
                case Direction.Left: _vrInputDir = vrController.transform.right * -1; break;
                case Direction.Up: _vrInputDir = vrController.transform.up; break;
                case Direction.Down: _vrInputDir = vrController.transform.up * -1; break;
                case Direction.Forward: _vrInputDir = vrController.transform.forward; break;
                case Direction.Back: _vrInputDir = vrController.transform.forward * -1; break;
            }
            
            Vector3 dirOfVRController = _vrInputDir;
            Vector3 dirToHead = cameraTrans.position - vrController.position;
            float dotProd = Vector3.Dot(dirOfVRController, dirToHead.normalized);
            
            if (usePalmMenuDotProd) pamlMenuDotProd.Invoke(dotProd);

            if (dotProd > lookAtTrigger)
            {
                if (!_hasBeenShown)
                {
                    onShow.Invoke();
                    _hasBeenShown = true;
                }
            }

            if (dotProd > lookAtSecondTrigger)
            {
                if (!_hasBeenShownForSecond)
                {
                    onSecondShow.Invoke();
                    _hasBeenShownForSecond = true;
                }
            }

            if (dotProd < lookAwayTrigger)
            {
                if (_hasBeenShown)
                {
                    onHide.Invoke();
                    _hasBeenShown = false;
                }
            }

            if (dotProd < lookAwaySecondTrigger)
            {
                if (_hasBeenShownForSecond)
                {
                    onSecondHide.Invoke();
                    _hasBeenShownForSecond = false;
                }
            }
        }
    }

    [Serializable]
    public class PamlMenuDotProdEvent : UnityEvent<float>
    {
    }
}
