using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;


/*Not going to do much audio as it is just a last implementation after all the game design and programming is done. Just to bring the game together
  Will be mostly 2D one shots with no params and just pitch modulation*/

public class GameSceneAudio : MonoBehaviour, IPlayerObserver, IRoundObserver
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

    void PlayGunshotSFX()
    {
        AudioPlayback.PlayOneShot(gunshotReference, null);
    }
    void PlayNewRoundAudio()
    {
        AudioPlayback.PlayOneShot(newRoundReference, null);
    }

    
}
