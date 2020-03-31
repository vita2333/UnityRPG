using System;
using System.Collections.Generic;
using Core;
using Types;
using UnityEditor.U2D;
using UnityEngine;

namespace Animation
{
    public class AtlasManager : MonoBehaviour
    {
        public static AtlasManager Instance;

        private readonly Dictionary<string, Dictionary<string, Sprite>> _atlasLookup =
            new Dictionary<string, Dictionary<string, Sprite>>();

        [HideInInspector] public List<Sprite> SpriteList = new List<Sprite>();

        public List<string> ModelList = new List<string>();
        public int ModelsLoaded;
        public int ModelsTotal;

        private void Start()
        {
            Debug.Log("atlas start===");
            // Initialize atlas dictionaries for all block types
            foreach (var blockType in DNABlockType.TypeList)
            {
                _atlasLookup[blockType] = new Dictionary<string, Sprite>();
            }

            // Sort each sprite in the spritelist into respective dictionary
            foreach (var sprite in SpriteList)
            {
                string blockType = sprite.name.Split('_')[0].ToUpper();
                try
                {
                    // eg: BACK
                    _atlasLookup[blockType][sprite.name] = sprite;
                }
                catch (Exception e)
                {
                    Debug.Log($"Failed to load sprite for atlas key; {blockType} sprite name {sprite.name}");
                }
            }

//            Instance = this; todo ???
            Instance = GetComponent<AtlasManager>();
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