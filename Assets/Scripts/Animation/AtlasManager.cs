using System;
using System.Collections.Generic;
using Core;
using Types;
using UnityEngine;

namespace Animation
{
    public class AtlasManager : MonoBehaviour
    {
        public static AtlasManager Instance;

        /**
         * Dictionary for blockType => { spriteName => Sprite }
         * eg: "BODY" => { "waist_female_platemail_sh_t_2" => Sprite }
         */
        private readonly Dictionary<string, Dictionary<string, Sprite>> _atlasLookup =
            new Dictionary<string, Dictionary<string, Sprite>>();

        [HideInInspector] public List<Sprite> SpriteList = new List<Sprite>();

        /**
         * Model is the same as CharacterDNABlock's ModelKey.
         * eg: "body_female_light"
         */
        public List<string> ModelList = new List<string>();
        public int ModelsLoaded;
        public int ModelsTotal;

        private void Start()
        {
            // Initialize atlas dictionaries for all block types
            foreach (string blockType in DNABlockType.TypeList)
            {
                _atlasLookup[blockType] = new Dictionary<string, Sprite>();
            }

            // Sort each sprite in the spriteList into respective dictionary
            foreach (var sprite in SpriteList)
            {
                string blockType = sprite.name.Split('_')[0].ToUpper();
                try
                {
                    // eg: BACK
                    _atlasLookup[blockType][sprite.name] = sprite;
                }
                catch (Exception)
                {
                    Debug.Log($"Failed to load sprite for atlas key; {blockType} sprite name {sprite.name}");
                }
            }

            Instance = GetComponent<AtlasManager>(); // same as  Instance = this;
        }

        public void IncrementModelLoaded()
        {
            Instance.ModelsLoaded++;
        }

        public Sprite GetSprite(string nameSprite)
        {
            if (Instance == null)
            {
                throw new Exception("Sprites not loaded into the AtlasManager! " +
                                    "Add the LPCAtlasManagerEditor script to a GameObject and click Load.");
            }

            var collection = Instance._atlasLookup[nameSprite.Split('_')[0].ToUpper()];
            Sprite item = collection.SafeGet(nameSprite);
            if (item == null)
            {
                Debug.Log("MISSING SPRITE!");
                throw new Exception("MISSING SPRITE!");
            }

            return collection[nameSprite];
        }
    }
}