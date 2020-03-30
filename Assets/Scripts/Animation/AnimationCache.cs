using System.Collections.Generic;
using System.Linq;
using Core;
using Types;

namespace Animation
{
    public class AnimationCache
    {
        private static AnimationCache _instance;
        public static AnimationCache Instance => _instance ?? (_instance = new AnimationCache());

        private readonly Dictionary<string, Dictionary<string, AnimationDNABlock>> _cacheLookup =
            DNABlockType.TypeList.ToDictionary(bt => bt.ToLower(), v => new Dictionary<string, AnimationDNABlock>());


        public void Add(string animationKey, AnimationDNABlock animationDnaBlock)
        {
            string cacheKey = animationKey.Split('_').FirstOrDefault();
            var cache = _cacheLookup.SafeGet(cacheKey);
            cache[animationKey] = animationDnaBlock;
        }

        public AnimationDNABlock Get(string animationKey)
        {
            AnimationDNABlock returnVal;
            string cacheKey = animationKey.Split('_').FirstOrDefault();
            var cache = _cacheLookup.SafeGet(cacheKey);
            cache.TryGetValue(animationKey, out returnVal);
            return returnVal;
        }
    }
}