using System;
using System.Collections.Generic;
using System.Threading;
using Animation;
using Animation.AnimationImporters;
using Character;
using Types;
using UnityEngine;

namespace Scene
{
    public class SceneController : MonoBehaviour
    {
        private AnimationManager _animationManager;
        private bool _animationsLoaded;
        private bool _animationLoading;
        private GameObject _player;
        private PlayerController _playerController;

        private Dictionary<string, Color> _modelColorLookup;
        private Dictionary<string, string> _modelTextLookup;


        private void Start()
        {
            Debug.Log("scene===");
            _animationManager = new AnimationManager();
            _animationLoading = false;
            _animationsLoaded = false;

            _player = GameObject.Find("/Player");
            _playerController = _player.GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (!_animationLoading)
            {
                _animationLoading = true;
                Thread thread = new Thread(_animationManager.LoadAllAnimationsIntoCache);
                thread.Start();
            }
            else
            {
                
            }
        }

        private void OnGUI()
        {
            Debug.Log("scene gui===");
            if (!_animationsLoaded)
            {
                int modelsLoaded = AtlasManager.Instance.ModelsLoaded;
                int modelTotal = AtlasManager.Instance.ModelTotal;
                var text = AtlasManager.Instance;

                // Loading message
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height),
                    "加载所有Sprite中..." + Math.Floor((double) modelsLoaded / modelTotal * 100) + "%");
                Debug.Log("modelsLoaded===" + modelsLoaded);
                Debug.Log("modelTotal===" + modelTotal);
                if (modelsLoaded == modelTotal) return;

                // The sprites are all cached. Lets initialize the scene.
                InitializeCharacter();
                InitiallizeDemoDNA();
                _animationsLoaded = true;
                _playerController.enabled = true;
            }
            else { Debug.Log("===todo"); }
        }

        void InitializeCharacter()
        {
            Player.CharacterDNA = new CharacterDNA();
            Player.AnimationDNA = new AnimationDNA();
        }

        void InitiallizeDemoDNA()
        {
            // this will populate the UI
            _modelColorLookup[DNABlockType.Back] = Color.red;
            _modelColorLookup[DNABlockType.Neck] = Color.red;
            _modelTextLookup[DNABlockType.Back] = "back_female_cape";
            _modelTextLookup[DNABlockType.Neck] = "neck_female_capeclip";
            _modelTextLookup[DNABlockType.Chest] = "chest_female_tightdress";
            _modelTextLookup[DNABlockType.Feet] = "feet_female_shoes";
            _modelTextLookup[DNABlockType.Hair] = "hair_female_shoulderr";
            _modelTextLookup[DNABlockType.Hands] = "hands_female_cloth";
            _modelTextLookup[DNABlockType.Legs] = "legs_female_pants";
            _modelTextLookup[DNABlockType.Body] = "body_female_dark";

            // this will update the characterDNA, flagging it as dirty and causing the first frame to animate
            Player.CharacterDNA.UpdateBlock(DNABlockType.Neck, _modelTextLookup[DNABlockType.Neck],
                _modelColorLookup[DNABlockType.Neck]);
            Player.CharacterDNA.UpdateBlock(DNABlockType.Back, _modelTextLookup[DNABlockType.Back],
                _modelColorLookup[DNABlockType.Back]);
            Player.CharacterDNA.UpdateBlock(DNABlockType.Chest, _modelTextLookup[DNABlockType.Chest],
                _modelColorLookup[DNABlockType.Chest]);
            Player.CharacterDNA.UpdateBlock(DNABlockType.Feet, _modelTextLookup[DNABlockType.Feet],
                _modelColorLookup[DNABlockType.Feet]);
            Player.CharacterDNA.UpdateBlock(DNABlockType.Hair, _modelTextLookup[DNABlockType.Hair],
                _modelColorLookup[DNABlockType.Hair]);
            Player.CharacterDNA.UpdateBlock(DNABlockType.Hands, _modelTextLookup[DNABlockType.Hands],
                _modelColorLookup[DNABlockType.Hands]);
            Player.CharacterDNA.UpdateBlock(DNABlockType.Legs, _modelTextLookup[DNABlockType.Legs],
                _modelColorLookup[DNABlockType.Legs]);
            Player.CharacterDNA.UpdateBlock(DNABlockType.Body, _modelTextLookup[DNABlockType.Body],
                _modelColorLookup[DNABlockType.Body]);
        }
    }
}