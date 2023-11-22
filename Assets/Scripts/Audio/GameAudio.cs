
using UnityEngine;
using FMODUnity;
using Utility.Broadcast;
using Audio.Playback;

namespace Audio.GamePlay
{

    /*Not going to do much audio as it is just a last implementation after all the game design and programming is done. Just to bring the game together
      Will be mostly 2D one shots with no params and just pitch modulation*/

    public class GameAudio : MonoBehaviour, IPlayerObserver, IRoundObserver
    {
        [SerializeField] private EventReference gameStartReference;
        [SerializeField] private EventReference gunshotReference;
        [SerializeField] private EventReference newRoundReference;

        void Start()
        {
            BroadCastManager.Instance.AddPlayerObserver(this);
            BroadCastManager.Instance.AddRoundObserver(this);

            AudioPlayback.PlayOneShot(gameStartReference, null);
        }

        void IPlayerObserver.OnNotify(PlayerState state)
        {
            PlayGunshotSFX();
        }

        void IRoundObserver.OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
        {
            switch (state)
            {
                case RoundState.ROUNDINTERIM:
                    PlayNewRoundAudio();
                    break;
            }
        }

        //Play sfxs, second argument is null as that is for 3D attenuationa and I have just implmented basic fire and forget audio
        void PlayGunshotSFX()
        {
            AudioPlayback.PlayOneShot(gunshotReference, null);
        }
        void PlayNewRoundAudio()
        {
            AudioPlayback.PlayOneShot(newRoundReference, null);
        }


    }
}