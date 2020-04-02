using System;

namespace Animation.Actions
{
    public abstract class BaseAction
    {
        /**
         * Short name for the animation
         * eg: sh: shoot
         *     sc: spell cast
         */
        public abstract string AnimationTag { get; }
        public abstract bool StopOnLastFrame { get; }

        public string Direction;

        /**
         * Number of frames in the animation, minimum is 1
         */
        private int _numberOfFrames;

        public int NumberOfFrames
        {
            get => _numberOfFrames;
            protected set => _numberOfFrames = Math.Max(value, 1);
        }

        
        public abstract IAnimationImporter GetAnimationImporter();
    }
}