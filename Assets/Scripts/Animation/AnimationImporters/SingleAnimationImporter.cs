using System.Collections.Generic;

namespace Animation.AnimationImporters
{
    public class SingleAnimationImporter : IAnimationImporter
    {
        public string TagName { get; }
        public int NumberOfFrames { get; }
        public int SpriteStartIndex { get; }
        public bool StopOnFinalFrame { get; }

        public SingleAnimationImporter(string tagName, int numberOfFrames, int spriteStartIndex, bool stopOnFinalFrame)
        {
            TagName = tagName;
            NumberOfFrames = numberOfFrames;
            SpriteStartIndex = spriteStartIndex;
            StopOnFinalFrame = stopOnFinalFrame;
        }


        public List<AnimationDNABlock> ImportAnimations(string spritesheetKey, string direction)
        {
            var animationList = new List<AnimationDNABlock>();
            var builder = new AnimationImportUtil();
            animationList.Add(builder.BuildAnimation(this, spritesheetKey, direction));
            return animationList;
        }
    }
}