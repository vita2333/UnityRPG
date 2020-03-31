﻿namespace Animation.Actions
{
    internal class WalkAction : BaseAction
    {
        public WalkAction()
        {
            NumberOfFrames = 9;
        }

        public override string AnimationTag => "wc";
        public override bool StopOnLastFrame => false;

        public override IAnimationImporter GetAnimationImporter()
        {
            var downAnimation = new SingleAnimationImporter($"{AnimationTag}_d", NumberOfFrames, 142, StopOnLastFrame);
            var leftAnimation = new SingleAnimationImporter($"{AnimationTag}_l", NumberOfFrames, 151, StopOnLastFrame);
            var rightAnimation = new SingleAnimationImporter($"{AnimationTag}_r", NumberOfFrames, 160, StopOnLastFrame);
            var upAnimation = new SingleAnimationImporter($"{AnimationTag}_t", NumberOfFrames, 169, StopOnLastFrame);

            return new WASDAnimationImporter(upAnimation, leftAnimation, downAnimation, rightAnimation);
        }
    }
}