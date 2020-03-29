using System.Collections.Generic;
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
//            _playerObject = GameObject.Find("/Player"); // todo ???
            _playerObject = gameObject; // todo ???
            _charAnimator = gameObject.AddComponent<AnimationRenderer>();
            InitializeCharacterRenderers(_charAnimator);
        
            // set default
            _animationManager=new AnimationManager();
            _newAction=new IdleAction();
            _lastDirection = DirectionType.Down;

        }

        private void InitializeCharacterRenderers(AnimationRenderer charAnimator)
        {
            var spriteRenderers = new Dictionary<string, SpriteRenderer>();
            foreach (var blockKey in DNABlockType.TypeList)
            {
                GameObject blockObject=new GameObject(blockKey);
                blockObject.transform.parent = _playerObject.transform;
                spriteRenderers[blockKey] = blockObject.AddComponent<SpriteRenderer>();
            }
            
            charAnimator.InitializeSpriteRenderers(spriteRenderers);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void UpdatePositoning()
        {
            
        }

        void UpdateAnimation()
        {
        }
    }
}