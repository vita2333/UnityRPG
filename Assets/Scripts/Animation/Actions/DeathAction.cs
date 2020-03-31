
namespace Animation.Actions
{
    public class DeathAction : BaseAction
    {
        public DeathAction()
        {
            NumberOfFrames = 6;
        }

        public override IAnimationImporter GetAnimationImporter()
        {
            var animationTag = AnimationTag;
            var spriteStartIndex = NumberOfFrames;
            var stopOnFinalFrame = StopOnLastFrame;
            return new SingleAnimationImporter(animationTag, NumberOfFrames, spriteStartIndex, stopOnFinalFrame);
        }

        public override string AnimationTag => "hu";
        public override bool StopOnLastFrame => true;
    }
}
