using System;
using Animation;

namespace Animation.Actions
{
    public abstract class BaseAction
    {
        public abstract string AnimationTag { get; }
        public abstract bool StopOnLastFrame { get; }
        public string Direction;
        private int _numberOfFrames;

        public int NumberOfFrames
        {
            get => _numberOfFrames;
            protected set => _numberOfFrames = Math.Max(value, 1);
        }

        public abstract IAnimationImporter GetAnimationImporter();
    }
}