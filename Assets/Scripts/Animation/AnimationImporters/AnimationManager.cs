using System.Collections.Generic;
using Animation.Actions;
using Character;
using Types;
using UnityEngine;

namespace Animation
{
    public class AnimationManager
    {
        private readonly List<BaseAction> _directionalActions;


        public AnimationManager()
        {
            // All directional actions
            _directionalActions = new List<BaseAction>
            {
                new SlashAction(),
                new SpellcastAction(),
                new ThrustAction(),
                new WalkAction(),
                new ShootAction(),
                new DeathAction()
            };
        }

        public void LoadAllAnimationsIntoCache()
        {
            //Builds all animations from sprites and adds them to the cache.
            //This should be called when a scene is FIRST loaded, before initializing characters.
            var modelList = AtlasManager.Instance.ModelList;
            foreach (string model in modelList)
            {
                LoadAnimationIntoCache(model);
                AtlasManager.Instance.IncrementModelLoaded();
            }
        }

        private void LoadAnimationIntoCache(string modelKey)
        {
            // Builds the animation object for all model, action, direction
            // combinations. Object is then added to the AnimationCache.
            AnimationCache animationCache = AnimationCache.Instance;
            foreach (BaseAction directionalAction in _directionalActions)
            {
                // Use the respective importer for the action 
                IAnimationImporter animationImporter = directionalAction.GetAnimationImporter();
                var newAnimations = animationImporter.ImportAnimations(modelKey, DirectionType.Down);
                foreach (AnimationDNABlock newAnimation in newAnimations)
                {
                    string animationKey =
                        $"{modelKey}_{directionalAction.AnimationTag}_{DirectionType.GetAnimationForDirection(newAnimation.Direction)}";
                    animationCache.Add(animationKey, newAnimation);
                }
            }

            // The "Idle" action reuses the first image of the death animation, so we need
            // to import the first frame of all death sprites without a direction tag.
            BaseAction deathAnimation = new DeathAction();
            var deathImporter = deathAnimation.GetAnimationImporter();
            var deathAnimations = deathImporter.ImportAnimations(modelKey, DirectionType.None);

            foreach (AnimationDNABlock newAnimation in deathAnimations)
            {
                string animationKey = $"{modelKey}_{deathAnimation.AnimationTag}";
                animationCache.Add(animationKey, newAnimation);
            }
        }


        public static void UpdateDNAForAction(CharacterDNA characterDNA, AnimationDNA animationDNA,
            BaseAction actionAnimation, string newDirection)
        {
            foreach (var blockType in DNABlockType.TypeList)
            {
                CharacterDNABlock characterDnaBlock = characterDNA.DNABlocks[blockType];
                if (characterDnaBlock.Enabled)
                {
                    animationDNA.DNABlocks[blockType] =
                        GetAnimation(characterDnaBlock.ModelKey, actionAnimation, newDirection);
                    AnimationDNABlock animationDnaBlock = animationDNA.DNABlocks[blockType];
                    if (animationDnaBlock == null)
                    {
                        Debug.Log($"Block not found: {blockType}");
                        continue;
                    }

                    animationDnaBlock.UpdateSpriteColor(characterDnaBlock.ItemColor);
                    animationDnaBlock.Enabled = true;
                }
                else
                {
                    // Disable the animation slot if the character slot isnt enabled
                    animationDNA.DNABlocks[blockType].Enabled = false;
                }

                characterDnaBlock.IsDirty = false;
            }
        }

        private static AnimationDNABlock GetAnimation(string modelKey, BaseAction actionAnimation, string direction)
        {
            // Fetches an animation from the animation store/cache
            AnimationCache animationCache = AnimationCache.Instance;
            string animationKey = modelKey;
            animationKey = direction.Equals(DirectionType.None)
                ? $"{animationKey}_{actionAnimation.AnimationTag}"
                : $"{animationKey}_{actionAnimation.AnimationTag}_{DirectionType.GetAnimationForDirection(direction)}";
            return animationCache.Get(animationKey);
        }
    }
}