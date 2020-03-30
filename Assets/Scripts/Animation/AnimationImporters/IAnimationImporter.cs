using System.Collections.Generic;

namespace Animation.AnimationImporters
{
    public interface IAnimationImporter
    {
        List<AnimationDNABlock> ImportAnimations(string spritesheetKey, string direction);
    }
}