using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Decembrist.Utils.Callback;
using VoidTask = System.Threading.Tasks.Task;

namespace Decembrist.Utils.Task
{
    /// <summary>
    /// Class for callback animation control
    /// </summary>
    public class AnimationPromise
    {
        private const string AnimationFinishedSignal = "animation_finished";

        private readonly AnimationPlayer _animObject;
        
        private VoidTask _animationTask;
        private AbstractCallback _callback;

        public AnimationPromise(AnimationPlayer animObject)
        {
            _animObject = animObject;
        }

        /// <summary>
        /// Queue next animation with name <param name="animationName"></param>
        /// </summary>
        /// <param name="animationName">Animation name</param>
        /// <returns>This object</returns>
        /// <exception cref="Exception">If this promise started already</exception>
        public AnimationPromise Show(string animationName)
        {
            if (_callback != null) throw new Exception("Animation started already");

            _animationTask = Promises.Of((resolve, reject) =>
            {
                _callback = SubscribeOnAnimationFinished(animationName, resolve);
                _animObject.Queue(animationName);
            });

            return this;
        }

        /// <summary>
        /// Get task for animation finish awaiting
        /// </summary>
        /// <returns>Animation task</returns>
        /// <exception cref="Exception">If animation was not started</exception>
        public async VoidTask AwaitUntilDone()
        {
            if (_animationTask == null) throw new Exception("Animation wasn't started");
            
            await _animationTask;
        }

        /// <summary>
        /// Scale animation speed by ratio
        /// </summary>
        /// <param name="ratio">Scale ratio</param>
        /// <returns>This object</returns>
        public AnimationPromise SetSpeedRatio(float ratio)
        {
            _animObject.PlaybackSpeed = ratio;
            return this;
        }

        /// <summary>
        /// Skip current animation
        /// </summary>
        /// <returns>This object</returns>
        public AnimationPromise Skip()
        {
            _animObject.Advance(9999999999);
            return this;
        }

        private AbstractCallback SubscribeOnAnimationFinished(string name, Action<object> resolve)
        {
            return _animObject.Subscribe(
                AnimationFinishedSignal,
                (string animationName) =>
                {
                    if (animationName == name)
                    {
                        _animObject.Unsubscribe(AnimationFinishedSignal, _callback);
                        resolve(null);
                    }
                }
            );
        }
    }
}