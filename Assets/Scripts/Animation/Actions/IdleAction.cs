using Animation.AnimationImporters;

namespace Animation.Actions
{
    public class IdleAction : BaseAction
    {
        public override string AnimatonTag => "hu";

        public override IAnimationImporter GetAnimationImporter()
        {
            throw new System.NotImplementedException();
        }
    }
}