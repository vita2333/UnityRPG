using Animation.AnimationImporters;

namespace Animation.Actions
{
    
    public abstract class BaseAction
    {
        public abstract string AnimatonTag { get; }
        
        public abstract IAnimationImporter GetAnimationImporter();
    }
}