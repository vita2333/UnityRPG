using System.Collections.Generic;
using Animation.Actions;
using Character;
using Types;

namespace Animation.AnimationImporters
{
    public class AnimationManager
    {
        private readonly List<BaseAction> _directionalActions;


        public AnimationManager()
        {
            // All directional actions
            _directionalActions = new List<BaseAction>
            {
                //todo
                new IdleAction()
            };
        }

        public void LoadAllAnimationsIntoCache()
        {
            //Builds all animations from sprites and adds them to the cache.
            //This should be called when a scene is FIRST loaded, before initializing characters.
            List<string> modelList = AtlasManager.Instance.ModelList;
            foreach (var model in modelList)
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
                IAnimationImporter animationImporter = directionalAction.GetAnimationImporter();
                var newAnimations = animationImporter.ImportAnimations(modelKey, DirectionType.Down);
                foreach (AnimationDNABlock newAnimation in newAnimations)
                {
                    string animationKey =
                        $"{modelKey}_{directionalAction.AnimatonTag}_{DirectionType.GetAnimationForDirection(newAnimation.Direction)}";
                    animationCache.Add(animationKey,newAnimation);
                }
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
                }
                else
                {
                    // Disable the animation slot if the character slot isnt enabled
                    animationDNA.DNABlocks[blockType].Enabled = false;
                }
            }
        }

//            private static AnimationDNABlock GetAnimation(string modelKey,BaseAction actionAnimation,string direction)
//            {
//                // Fetches an animation from the animation store/cache
//
//            }
    }
}