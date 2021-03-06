using System.Collections.Generic;
using Types;
using UnityEngine;

namespace Animation
{
    public class AnimationDNABlock
    {
        /**
         * Full name of the animation frame, including direction.
         * eg: "body_male_light_sl_l"
         */
        public string AnimationKey { get; }
        public List<Sprite> SpriteList { get; }
        public int SortingOrder { get; }

        public string Direction { get; }

        /**
         * Controlling  whether set sprite info in SpriteRenderer
         */
        public bool Enabled;
        public bool IsDirty;
        public Color SpriteColor { get; private set; }

        public AnimationDNABlock()
        {
            AnimationKey = "UNKNOWN";
            Direction = DirectionType.Down;
            Enabled = false;
            IsDirty = false;
            SortingOrder = -99;
        }

        public AnimationDNABlock(string animationKey, List<Sprite> spriteList, string direction, int sortingOrder)
        {
            AnimationKey = animationKey;
            SpriteList = spriteList;
            Direction = direction;
            SortingOrder = sortingOrder;
            Enabled = true;
            IsDirty = true;
        }

        public void UpdateSpriteColor(Color spriteColor)
        {
            SpriteColor = spriteColor;
            IsDirty = true;
        }
    }
}