using LethalLib;
using UnityEngine;

namespace ExampleScrapPlugin.custom
{
    public class MonkeyWithSound : GrabbableObject
    {
        // these are public so we can set them in the unity editor
        public AudioSource monkeyAudio; 
        public AudioClip monkeyClip;
        
        public override void Start()
        {
            ExamplePlugin.Log.LogDebug("MonkeyWithSound started");
            base.Start();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            ExamplePlugin.Log.LogDebug("MonkeyWithSound activated");
            base.ItemActivate(used, buttonDown);
            SwitchPlaying();
        }

        private void SwitchPlaying()
        {
            ExamplePlugin.Log.LogDebug("Switching playing");
            ExamplePlugin.Log.LogDebug($"AudioSource state: {(monkeyAudio.isPlaying ? "Playing" : "Stopped")}");
            if (monkeyAudio.isPlaying)
                monkeyAudio.Stop();
            else
                monkeyAudio.Play();
        }
    }
}