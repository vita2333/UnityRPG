using System.Collections.Generic;

namespace Animation
{
    public interface IAnimationImporter
    {
        List<AnimationDNABlock> ImportAnimations(string spritesheetKey, string direction);
    }
}