using System;
using System.Collections.Generic;
using System.Linq;
using Animation.Actions;
using UnityEngine;

namespace Animation
{
    public class AnimationRenderer : MonoBehaviour
    {
        private float _passedTime;
        private bool _playing;
        private int _currentFrameNumber;
        public BaseAction CurrentAnimationAction { get; private set; }
        private Dictionary<string, SpriteRenderer> _spriteRenderers;

        private AnimationDNA _animationDNA;
        private bool _stopOnFinalFrame;
        private bool _stopNow;
        private float _totalAnimTimeInSeconds;
        
        
        private void Start()
        {
            _passedTime = 0;
            _playing = true;
            CurrentAnimationAction = new IdleAction();
        }

        private void Update()
        {
            Debug.Log("animation renderer update===");
            if (!_playing) return;
            bool hasAnimationKeys = _animationDNA?.DNABlocks?.Keys.Any() == true;
            if (!hasAnimationKeys) return;

            int currentFrameIndex = _currentFrameNumber % CurrentAnimationAction.NumberOfFrames; // % 求余数

            if (_stopNow || (_stopOnFinalFrame && currentFrameIndex == 0))
            {
                _playing = false;
                _stopNow = false;
                _passedTime = 0;
                foreach (string animationKey in _animationDNA.DNABlocks.Keys)
                {
                    RenderAnimationFrame(animationKey,0);
                }

                return;
            }
            
            
        }

        void RenderAnimationFrame(string animationKey, int currentFrameIndex)
        {
            AnimationDNABlock animationDnaBlock = _animationDNA.DNABlocks[animationKey];
            SpriteRenderer rendererCurrent = _spriteRenderers[animationKey];
            if (animationDnaBlock?.Enabled==true)
            {
                // set sprite renderer info
                rendererCurrent.sprite = animationDnaBlock.SpriteList[currentFrameIndex];
                rendererCurrent.sortingOrder = animationDnaBlock.SortingOrder;
                rendererCurrent.sortingLayerName = "Units";
                
                // Don't color clear objects
                if (animationDnaBlock.SpriteColor!=Color.clear)
                {
                    rendererCurrent.material.SetColor("_Color",animationDnaBlock.SpriteColor);
                }
            }
            else { rendererCurrent.sprite = null; }
        }
        
        public void AnimateAction(AnimationDNA animationDna,BaseAction animationAction)
        {
            Debug.Log("animationDna==="+animationDna);
            _animationDNA = animationDna;
            CurrentAnimationAction = animationAction;
            _stopOnFinalFrame = animationAction.StopOnLastFrame;
            _playing = true;
            _currentFrameNumber = 1; // starting on frame 1 makes the animation more snappy
            _totalAnimTimeInSeconds = 2f;
        }

        public void ResetAnimation()
        {
            _stopNow = true;
        }
        
        public void InitializeSpriteRenderers(Dictionary<string, SpriteRenderer> spriteRenderers)
        {
            _spriteRenderers = spriteRenderers;
        }

        public void UpdateAnimationTime(float totalAnimTimeInSeconds)
        {
            _totalAnimTimeInSeconds = totalAnimTimeInSeconds;
        }
    }
}