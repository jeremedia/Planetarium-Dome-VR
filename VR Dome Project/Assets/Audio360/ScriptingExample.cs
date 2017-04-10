using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptingExample : MonoBehaviour
{
#if !DISABLE_EXAMPLE
    TBE.SpatDecoderFile spat;

    void Start()
    {
        spat = GetComponent<TBE.SpatDecoderFile>();

        // Listen to events from SpatDecoderFile.
        // Events are currently unsupported on iOS.
#if !UNITY_IOS
        spat.events.newEvent += newEvent;
#endif

        // Specifying no path name results in the file being
        // opened from the Assets/StreamingAssets directory
        if (!spat.open("VoiceDirections.tbe"))
        {
            Debug.LogError("Failed to open file");
            return;
        }
        spat.play();
    }

    void newEvent(TBE.Event e)
    {
        // Loop playback
        if (e == TBE.Event.END_OF_STREAM)
        {
            spat.play();
        }
    }
#endif
}