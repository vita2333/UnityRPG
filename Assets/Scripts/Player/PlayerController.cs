using System.Collections.Generic;
using Animation.Actions;
using Animation;
using Types;
using UnityEngine;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        public float SpeedWalk = 1;
        public float SpeedRun = 2;
        public float SpeedAnimation = 1;
        public float SpeedCurrent = 1;

        private GameObject _playerObject;
        private AnimationRenderer _charAnimator;
        private AnimationManager _animationManager;
        private BaseAction _newAction;
        private string _lastDirection;

        private KeyCode _newInput;
        private KeyCode _lastInput;
        private bool _isStillMoving;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("play start===");
            // prepare charactor sprites
            _playerObject = GameObject.Find("/Player"); // todo ???
//            _playerObject = gameObject; // todo ???
            _charAnimator = gameObject.AddComponent<AnimationRenderer>();
            InitializeCharacterRenderers(_charAnimator);

            // set default
            _animationManager = new AnimationManager();
            _newAction = new IdleAction();
            _lastDirection = DirectionType.Down;
        }

        private void InitializeCharacterRenderers(AnimationRenderer charAnimator)
        {
            var spriteRenderers = new Dictionary<string, SpriteRenderer>();
            foreach (var blockKey in DNABlockType.TypeList)
            {
                GameObject blockObject = new GameObject(blockKey);
                blockObject.transform.parent = _playerObject.transform;
                spriteRenderers[blockKey] = blockObject.AddComponent<SpriteRenderer>();
            }

            charAnimator.InitializeSpriteRenderers(spriteRenderers);
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePositoning();
            UpdateAnimation();
        }

        void UpdatePositoning()
        {
        }

        void UpdateAnimation()
        {
            string newDirection = DirectionType.None;
            _newInput = KeyCode.None;
            if (newDirection == DirectionType.None) { newDirection = _lastDirection; }

            bool sameAction = _lastDirection == newDirection && _lastInput == _newInput;
            _charAnimator.UpdateAnimationTime(1 / SpeedAnimation);
            Debug.Log("Player.CharacterDNA.IsDirty===" + Player.CharacterDNA.IsDirty());
            if (!sameAction || Player.CharacterDNA.IsDirty())
            {
                AnimationManager.UpdateDNAForAction(Player.CharacterDNA, Player.AnimationDNA, _newAction, newDirection);
                _charAnimator.AnimateAction(Player.AnimationDNA, _newAction);
            }
            else if (!Input.anyKey && !_isStillMoving) { _charAnimator.ResetAnimation(); }

            _lastDirection = _newAction.Direction = newDirection;
            _lastInput = _newInput;
        }
    }
}