using UnityEngine;

namespace Audio.Playback
{
    public static class AudioPlayback
    {
        //Use to play basic one shot with no param values, can make 3D by passing gameobj as argument, or leave argument as null if 2D
        public static void PlayOneShot(FMODUnity.EventReference fmodEvent, Transform transformToAttachTo)
        {
            FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);

            //Check if position has been given to attach event to that position and make 3D
            if (transformToAttachTo != null)
            {
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, transformToAttachTo);
            }

            instance.start();
            instance.release();

        }

    }
}