using Animation;

namespace Animation.Actions
{
    public class IdleAction : BaseAction
    {
        public IdleAction()
        {
            NumberOfFrames = 1;
        }

        public override string AnimationTag => "hu";
        public override bool StopOnLastFrame => false;

        public override IAnimationImporter GetAnimationImporter()
        {
            var animatonTag = AnimationTag;
            var SpriteStartIndex = 0;
            var stopOnFinalFrame = StopOnLastFrame;
            return new SingleAnimationImporter(animatonTag, NumberOfFrames, SpriteStartIndex, stopOnFinalFrame);
        }
    }
}