using System.Collections.Generic;
using System.Linq;
using Types;

namespace Animation
{
    public class AnimationDNA
    {
        public Dictionary<string, AnimationDNABlock> DNABlocks { get; }=
        DNABlockType.TypeList.ToDictionary(bt => bt, v => new AnimationDNABlock());
    }
}