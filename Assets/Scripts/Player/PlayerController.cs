using System.Collections.Generic;
using Animation;
using Animation.Actions;
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
            // prepare charactor sprites
            _playerObject = GameObject.Find("/Player"); // same as      _playerObject = gameObject;

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
                blockObject.transform.position += Vector3.right * 3;
                blockObject.transform.localScale = new Vector3(6, 6, 6);

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
            if (Input.GetKey(KeyCode.LeftShift)) { SpeedCurrent = SpeedRun; }
            else { SpeedCurrent = SpeedWalk; }

            SpeedAnimation = SpeedCurrent;

            float moveAmount = SpeedCurrent * Time.deltaTime;
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            float absHorizontal = Mathf.Abs(moveHorizontal);
            float absVertical = Mathf.Abs(moveVertical);

            bool isDiagnonal = absHorizontal > 0 && absVertical > 0;
            if (isDiagnonal)
            {
                moveAmount *= 0.75f; // account for diagonal movement speed increase
            }

            if (moveHorizontal > 0) { gameObject.transform.position += moveAmount * absHorizontal * Vector3.right; }
            else if (moveHorizontal < 0) { gameObject.transform.position += moveAmount * absHorizontal * Vector3.left; }

            if (moveVertical > 0) { gameObject.transform.position += moveAmount * absVertical * Vector3.up; }
            else if (moveVertical < 0) { gameObject.transform.position += moveAmount * absVertical * Vector3.down; }

            _isStillMoving = absHorizontal > 0 || absVertical > 0;
        }

        void UpdateAnimation()
        {
            string newDirection;
            HanleInput(out newDirection);


            // continue using the last direction when the character stops moving
            if (newDirection == DirectionType.None) { newDirection = _lastDirection; }

            bool sameAction = _lastDirection == newDirection && _lastInput == _newInput;

            _charAnimator.UpdateAnimationTime(1 / SpeedAnimation);

            if (!sameAction || Player.CharacterDNA.IsDirty())
            {
                AnimationManager.UpdateDNAForAction(Player.CharacterDNA, Player.AnimationDNA, _newAction, newDirection);
                _charAnimator.AnimateAction(Player.AnimationDNA, _newAction);
            }
            else if (!Input.anyKey && !_isStillMoving) { _charAnimator.ResetAnimation(); }

            _lastDirection = _newAction.Direction = newDirection;
            _lastInput = _newInput;
        }

        void HanleInput(out string newDirection)
        {
            newDirection = DirectionType.None;
            if (Input.GetKeyDown(KeyCode.W) && _newInput != KeyCode.W)
            {
                newDirection = DirectionType.Up;
                _newAction = new WalkAction();
                _newInput = KeyCode.W;
            }
            else if (Input.GetKeyDown(KeyCode.A) && _newInput != KeyCode.A)
            {
                newDirection = DirectionType.Left;
                _newAction = new WalkAction();
                _newInput = KeyCode.A;
            }
            else if (Input.GetKeyDown(KeyCode.S) && _newInput != KeyCode.S)
            {
                newDirection = DirectionType.Down;
                _newAction = new WalkAction();
                _newInput = KeyCode.S;
            }
            else if (Input.GetKeyDown(KeyCode.D) && _newInput != KeyCode.D)
            {
                newDirection = DirectionType.Right;
                _newAction = new WalkAction();
                _newInput = KeyCode.D;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                _newAction = new SlashAction();
                _newInput = KeyCode.Space;
                SpeedAnimation = 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                _newAction = new ThrustAction();
                _newInput = KeyCode.F;
                SpeedAnimation = 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                _newAction = new SpellcastAction();
                _newInput = KeyCode.R;
                SpeedAnimation = 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                _newAction = new ShootAction();
                _newInput = KeyCode.E;
                SpeedAnimation = 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                _newAction = new DeathAction();
                _newInput = KeyCode.X;
                SpeedAnimation = 1.0f;
            }
            else { _newInput = KeyCode.None; }
        }
    }
}