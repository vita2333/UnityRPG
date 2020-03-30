using System.Threading;
using Animation.AnimationImporters;
using Player;
using UnityEngine;

namespace Scene
{
    public class SceneController : MonoBehaviour
    {
        private AnimationManager _animationManager;
        private bool _animationsLoaded;
        private bool _animationLoading;
        private GameObject _player;
        private PlayerController _playerController;


        private void Start()
        {
            _animationManager=new AnimationManager();
            _animationLoading = false;
            _animationsLoaded = false;

            _player = GameObject.Find("/Player");
            _playerController = _player.GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (!_animationsLoaded)
            {
                _animationsLoaded = true;
                Thread thread=new Thread(_animationManager.LoadAllAnimationsIntoCache);
                thread.Start();
            }
        }

    }
}