using System.Collections.Generic;
using UnityEngine;

namespace Animation.AnimationImporters
{
    public class AnimationRenderer : MonoBehaviour
    {
        private Dictionary<string, SpriteRenderer> _spriteRenderers;

        public void InitializeSpriteRenderers(Dictionary<string, SpriteRenderer> spriteRenderers)
        {
            _spriteRenderers = spriteRenderers;
        }
    }
}