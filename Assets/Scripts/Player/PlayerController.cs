using Animation.Actions;
using Animation.AnimationImporters;
using Types;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speedWalk = 1;
        public float speedRun = 2;
        public float speedAnimation = 1;
        public float speedCurrent = 1;
    
        private GameObject _playerObject;

        private AnimationRenderer _charAnimator;

        private AnimationManager _animationManager;

        private BaseAction _newAction;

        private string _lastDirection;
        
        // Start is called before the first frame update
        void Start()
        {
            // prepare charactor sprites
            _playerObject = GameObject.Find("/Player"); // todo ???
            _charAnimator = gameObject.AddComponent<AnimationRenderer>();
            InitializeCharacterRenderers(_charAnimator);
        
            // set default
            _animationManager=new AnimationManager();
            _newAction=new IdleAction();
            _lastDirection = DirectionType.Down;

        }

        private void InitializeCharacterRenderers(AnimationRenderer charAnimator)
        {
            Debug.Log("InitializeCharacterRenderers==="+charAnimator);
        }

        // Update is called once per frame
        void Update()
        {
        }
    
    
    }
}