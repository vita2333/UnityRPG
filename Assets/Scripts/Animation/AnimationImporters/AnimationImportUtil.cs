using System.Collections.Generic;
using Types;
using UnityEngine;

namespace Animation
{
    public class AnimationImportUtil
    {
        public AnimationDNABlock BuildAnimation(SingleAnimationImporter singleAnimationImporter, string spritesheetKey,
            string direction)
        {
            string animationKey = spritesheetKey + "_" + singleAnimationImporter.TagName;

            // fetch all frames for an model/action/direction into a list
            var spriteList = new List<Sprite>();
            for (int i = 0; i < singleAnimationImporter.NumberOfFrames; i++)
            {
                string spriteKey = animationKey + "_" + i;
                Sprite sprite = AtlasManager.Instance.GetSprite(spriteKey);
                spriteList.Add(sprite);
            }

            // get the model key from the animation key
            string modelKey = animationKey.Split('_')[0].ToUpper();
            int sortingOrder = DNABlockType.GetSortingOrder(modelKey, direction);

            // create a new animation block
            return new AnimationDNABlock(animationKey, spriteList, direction, sortingOrder);
        }
    }
}