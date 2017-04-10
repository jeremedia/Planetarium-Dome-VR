/*
 *  Copyright 2013-Present Two Big Ears Limited
 *  All rights reserved.
 *  http://twobigears.com
 */

using System.Collections;
using System.IO;
using UnityEngine;

namespace TBE
{
    /// <summary>
    /// An object for playing back pre-baked spatial audio files.
    /// </summary>
    public class SpatDecoderFile : MonoBehaviour
    {
        /// File that must be played back
        public string file = string.Empty;
        /// Play when Start() is called
        public bool playOnStart;

        /// Listen to events related to this object.
        /// Events are currently unavailable on iOS. This will be addressed in a future release.
        /// T21208982
#if !UNITY_IOS
        public EventListener events = new EventListener();
#endif

        NativeSpatDecoderFile spat_;
        [SerializeField]
        float volume_ = 1.0f;
        [SerializeField]
        SyncMode syncMode_ = SyncMode.INTERNAL;
        [SerializeField]
        bool focus_ = false;
        [SerializeField]
        bool focusFollowsListener_ = true;
        [SerializeField]
        float offFocusLeveldB_ = -0.0f;
        [SerializeField]
        float focusWidth_ = 90.0f;

        void Awake()
        {
            init();
        }

        void Start()
        {
            if (spat_ != null && file != null && file.Length > 0)
            {
                if (!open(file))
                {
                    Utils.logError("Failed to open " + resolvePath(file, PathType.STREAMING_ASSETS), this);
                    return;
                }

                if (playOnStart)
                {
                    spat_.play();
                }
            }
        }

        /// Internal. Initialise the native object.
        private void init()
        {
            if (spat_ != null)
            {
                return;
            }

            if (AudioEngineManager.Instance == null || AudioEngineManager.Instance.nativeEngine == null)
            {
                return;
            }

            spat_ = AudioEngineManager.Instance.nativeEngine.createSpatDecoderFile();
            if (spat_ == null)
            {
                Utils.logError("Native SpatDecoderFile is invalid", this);
                return;
            }
#if !UNITY_IOS
            spat_.setEventListener(events);
#endif
            spat_.setVolume(volume_, 0 /* default ramp */ );
            spat_.setSyncMode(syncMode_);
            spat_.enableFocus(focus_, focusFollowsListener_);
            spat_.setOffFocusLeveldB(offFocusLeveldB_);
            spat_.setFocusWidthDegrees(focusWidth_);
        }

        /// <summary>
        /// Opens an asset for playback. Currently .wav and .tbe file formats are supported. If no path is specified, 
        /// the asset will be loaded from Assets/StreamingAssets.
        /// While the asset is opened synchronously, it is loaded into the streaming buffer asynchronously. An
        /// event (Event.DECODER_INIT) will be dispatched to the event listener when the streaming buffer is ready for the
        /// asset to play.
        /// </summary>
        /// <param name="fileToplay">Name of the file in StreamAssets or the full path</param>
        /// <param name="map">Channel map/conifiguration of the file</param>
        /// <returns>true if the file was found and successfully opened</returns>
        public bool open(string fileToplay, ChannelMap map = ChannelMap.TBE_8_2)
        {
            if (spat_ != null)
            {
                if (spat_.open(resolvePath(fileToplay, PathType.STREAMING_ASSETS), map) == EngineError.OK)
                {
                    file = fileToplay;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Opens an asset for playback with additional information, such as the size and offset specified through
        /// assetDescriptor. Currently .wav and .tbe file formats are supported. If no path is specified, 
        /// the asset will be loaded from Assets/StreamingAssets.
        /// While the asset is opened synchronously, it is loaded into the streaming buffer asynchronously. An
        /// event (Event.DECODER_INIT) will be dispatched to the event listener when the streaming buffer is ready for the
        /// asset to play.
        /// </summary>
        /// <param name="fileToplay">Name of the file in StreamAssets or the full path</param>
        /// <param name="assetDescriptor">Custom file offset and size. Any or both values can be zero if unknown.</param>
        /// <param name="map">Channel map/conifiguration of the file</param>
        /// <returns>true if the file was found and successfully opened</returns>
        public bool open(string fileToplay, AssetDescriptor assetDescriptor, ChannelMap map = ChannelMap.TBE_8_2)
        {
            if (spat_ != null)
            {
                if (spat_.open(resolvePath(fileToplay, PathType.STREAMING_ASSETS), assetDescriptor, map) == EngineError.OK)
                {
                    file = fileToplay;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Close an opened file.
        /// </summary>
        public void close()
        {
            if (spat_ != null)
            {
                spat_.close();
            }
        }

        /// <summary>
        /// Returns true if a file is open and ready.
        /// </summary>
        /// <returns>Returns true if a file is open and ready.</returns>
        public bool isOpen()
        {
            if (spat_ != null)
            {
                return spat_.isOpen();
            }
            return false;
        }

        /// <summary>
        /// Begin playback of an opened file.
        /// Any subsequent call to this function or any play function
        /// will disregard this event if it hasn't already been triggered.
        /// </summary>
        public void play()
        {
            if (spat_ != null)
            {
                spat_.play();
            }
        }

        /// <summary>
        /// Schedule playback x milliseconds from now.
        /// Any subsequent call to this function or any play function
        /// will disregard this event if it hasn't already been triggered.
        /// </summary>
        /// <param name="millisecondsFromNow">Time from now in milliseconds</param>
        public void playScheduled(float millisecondsFromNow)
        {
            if (spat_ != null)
            {
                spat_.playScheduled(millisecondsFromNow);
            }
        }

        /// <summary>
        /// Begin playback with a fade.
        /// Any subsequent call to this function or any play function
        /// will disregard this event if it hasn't already been triggered.
        /// </summary>
        /// <param name="fadeDurationInMs">Duration of the fade in milliseconds</param>
        public void playWithFade(float fadeDurationInMs)
        {
            if (spat_ != null)
            {
                spat_.playWithFade(fadeDurationInMs);
            }
        }

        /// <summary>
        /// Stop playback.
        /// Any subsequent call to this function or any stop function
        /// will disregard this event if it hasn't already been triggered.
        /// </summary>
        public void stop()
        {
            if (spat_ != null)
            {
                spat_.stop();
            }
        }

        /// <summary>
        /// Schedule to stop playback x milliseconds from now.
        /// Any subsequent call to this function or any stop function
        /// will disregard this event if it hasn't already been triggered.
        /// </summary>
        /// <param name="millisecondsFromNow">Time from now in milliseconds</param>
        public void stopScheduled(float millisecondsFromNow)
        {
            if (spat_ != null)
            {
                spat_.stopScheduled(millisecondsFromNow);
            }
        }

        /// <summary>
        /// Fadeout and stop playback.
        /// Any subsequent call to this function or any stop function
        /// will disregard this event if it hasn't already been triggered.
        /// </summary>
        /// <param name="fadeDurationInMs">Duration of the fade in milliseconds.</param>
        public void stopWithFade(float fadeDurationInMs)
        {
            if (spat_ != null)
            {
                spat_.stopWithFade(fadeDurationInMs);
            }
        }

        /// <summary>
        ///  Pause playback.
        /// Any subsequent call to this function or any pause function
        /// will disregard this event if it hasn't already been triggered.
        /// </summary>
        public void pause()
        {
            if (spat_ != null)
            {
                spat_.pause();
            }
        }

        /// <summary>
        /// Schedule playback to be paused x milliseconds from now.
        /// Any subsequent call to this function or any pause function
        /// will disregard this event if it hasn't already been triggered.
        /// </summary>
        /// <param name="millisecondsFromNow">Time from now in milliseconds</param>
        public void pauseScheduled(float millisecondsFromNow)
        {
            if (spat_ != null)
            {
                spat_.pauseScheduled(millisecondsFromNow);
            }
        }

        /// <summary>
        /// Fadeout and pause playback.
        /// Any subsequent call to this function or any pause function
        /// will disregard this event if it hasn't already been triggered.
        /// </summary>
        /// <param name="fadeDurationInMs">Duration of the fade in milliseconds.</param>
        public void pauseWithFade(float fadeDurationInMs)
        {
            if (spat_ != null)
            {
                spat_.pauseWithFade(fadeDurationInMs);
            }
        }

        /// <summary>
        /// Returns the current play state (Eg. playing, paused, stopped).
        /// </summary>
        /// <returns>Returns the current play state.</returns>
        public PlayState getPlayState()
        {
            if (spat_ != null)
            {
                return spat_.getPlayState();
            }
            return PlayState.INVALID;
        }

        /// <summary>
        /// Seek playback to an absolute point in milliseconds.
        /// </summary>
        /// <param name="ms">Time in milliseconds.</param>
        public void seekToMs(float ms)
        {
            if (spat_ != null)
            {
                spat_.seekToMs(ms);
            }
        }

        /// <summary>
        /// /// Seek playback to an absolute point in samples.
        /// </summary>
        /// <param name="sample">Time in samples</param>
        public void seekToSample(uint sample)
        {
            if (spat_ != null)
            {
                spat_.seekToSample(sample);
            }
        }

        /// <summary>
        /// Returns elapsed playback time in milliseconds.
        /// </summary>
        /// <returns>Returns elapsed playback time in milliseconds.</returns>
        public double getElapsedTimeInMs()
        {
            if (spat_ != null)
            {
                return spat_.getElapsedTimeInMs();
            }
            return 0.0;
        }

        /// <summary>
        /// Returns elapsed playback time in samples.
        /// </summary>
        /// <returns>Returns elapsed playback time in samples.</returns>
        public uint getElapsedTimeInSamples()
        {
            if (spat_ != null)
            {
                return spat_.getElapsedTimeInSamples();
            }
            return 0;
        }

        /// <summary>
        /// Returns the total duration of the asset in milliseconds.
        /// </summary>
        /// <returns>Returns the total duration of the asset in milliseconds.</returns>
        public double getAssetDurationInMs()
        {
            if (spat_ != null)
            {
                return spat_.getAssetDurationInMs();
            }
            return 0.0;
        }

        /// <summary>
        /// Returns the total duration of the asset in samples.
        /// </summary>
        /// <returns>Returns the total duration of the asset in samples.</returns>
        public uint getAssetDurationInSamples()
        {
            if (spat_ != null)
            {
                return spat_.getAssetDurationInSamples();
            }
            return 0;
        }

        void OnDestroy()
        {
            if (spat_ != null && AudioEngineManager.Instance != null && AudioEngineManager.Instance.nativeEngine != null)
            {
                AudioEngineManager.Instance.nativeEngine.destroySpatDecoderFile(spat_);
            }
        }

        /// <summary>
        /// Set the external time source in milliseconds. The audio asset will chase this time
        /// source. Only applicable when the SyncMode is set to SyncMode.EXTERNAL
        /// </summary>
        /// <param name="externalClockInMs">External time in milliseconds, to be updated every frame.</param>
        public void setExternalClockInMs(double externalClockInMs)
        {
            if (spat_ != null)
            {
                spat_.setExternalClockInMs(externalClockInMs);
            }
        }

        /// <summary>
        /// Set how often the engine tries to synchronise to the external clock. Very low values can result in stutter,
        /// very high values can result in synchronisation latency.
        /// This method along with setResyncThresholdMs() can be used to fine-tune synchronisation.
        /// </summary>
        /// <param name="freewheelInMs"></param>
        public void setFreewheelTimeInMs(double freewheelInMs)
        {
            if (spat_ != null)
            {
                spat_.setFreewheelTimeInMs(freewheelInMs);
            }
        }

        /// <summary>
        /// Returns the free wheel time in milliseconds
        /// </summary>
        /// <returns>Returns the free wheel time in milliseconds</returns>
        public double getFreewheelTimeInMs()
        {
            if (spat_ != null)
            {
                return spat_.getFreewheelTimeInMs();
            }
            return 0.0;
        }

        /// <summary>
        /// The time threshold after which the engine will synchronise to the external clock.
        /// This method along with setFreewheelTimeInMs() can be used to fine-tune synchronisation.
        /// </summary>
        /// <param name="resyncThresholdMs">Synchronisation threshold in milliseconds</param>
        public void setResyncThresholdMs(double resyncThresholdMs)
        {
            if (spat_ != null)
            {
                spat_.setResyncThresholdMs(resyncThresholdMs);
            }
        }

        /// <summary>
        /// Returns the current synchronisation threshold.
        /// </summary>
        /// <returns>Returns the current synchronisation threshold.</returns>
        public double getResyncThresholdMs()
        {
            if (spat_ != null)
            {
                return spat_.getResyncThresholdMs();
            }
            return 0.0;
        }

        /// <summary>
        /// Set the volume in linear gain
        /// </summary>
        public float volume
        {
            get
            {
                if (spat_ != null)
                {
                    volume_ = spat_.getVolume();
                }
                return volume_;
            }
            set
            {
                volume_ = value;
                if (spat_ != null)
                {
                    spat_.setVolume(volume_, 0 /* default ramp */ );
                }
            }
        }

        /// <summary>
        /// Set synchronisation mode. Default is SyncMode.INTERNAL. When set to
        /// SyncMode.EXTERNAL, setExternalClockInMs() can be used to get the audio
        /// asset to chase an external time source.
        /// </summary>
        public SyncMode syncMode
        {
            get
            {
                if (spat_ != null)
                {
                    syncMode_ = spat_.getSyncMode();
                }
                return syncMode_;
            }
            set
            {
                syncMode_ = value;
                if (spat_ != null)
                {
                    spat_.setSyncMode(syncMode_);
                }
            }
        }

        /// <summary>
        /// Enable mix focus. This gets a specified area of the mix to be more audible than surrounding areas, by reducing
        /// the volume of the area that isn't in focus.
        /// </summary>
        public bool focus
        {
            get
            {
                return focus_;
            }
            set
            {
                focus_ = value;
                if (spat_ != null)
                {
                    spat_.enableFocus(focus_, focusFollowsListener_);
                }
            }
        }

        /// <summary>
        /// The attenuation level in dB outside of the focus area. Between 0dB and -24dB.
        /// </summary>
        public float offFocusLeveldB
        {
            get
            {
                return offFocusLeveldB_;
            }
            set
            {
                offFocusLeveldB_ = Mathf.Clamp(value, Utils.kFocusMin, Utils.kFocusMax);
                if (spat_ != null)
                {
                    spat_.setOffFocusLeveldB(offFocusLeveldB_);
                }
            }
        }

        /// <summary>
        /// Set the width angle in degrees of the focus area, between 40 and 120 degrees.
        /// </summary>
        public float focusWidth
        {
            get
            {
                return focusWidth_;
            }
            set
            {
                focusWidth_ = Mathf.Clamp(value, Utils.kFocusWidthMin, Utils.kFocusWidthMax);
                if (spat_ != null)
                {
                    spat_.setFocusWidthDegrees(focusWidth_);
                }
            }
        }

        /// <summary>
        /// Helper method to create an instance of SpatDecoderFile on a GameObject
        /// </summary>
        /// <param name="ga">GameObject instance</param>
        /// <param name="file">File to be played back</param>
        /// <param name="map">Channel map/spatial format of the file</param>
        /// <returns></returns>
        static public TBE.SpatDecoderFile createOnObject(GameObject ga, string file, ChannelMap map = TBE.ChannelMap.TBE_8_2)
        {
            var obj = ga.AddComponent<SpatDecoderFile>();
            if (!obj.open(file, map))
            {
                Utils.logError("Failed to open " + file, ga);
                Destroy(obj);
                return null;
            }
            return obj;
        }

        /// Internal. Resolve streaming assets path.
        private string resolvePath(string path, PathType type)
        {
            if (type == PathType.STREAMING_ASSETS)
            {
                string streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, path);
                if (streamingAssetsPath.Contains("apk!") && Application.platform == RuntimePlatform.Android)
                {
                    return "asset:///" + Path.GetFileName(streamingAssetsPath);
                }
                return streamingAssetsPath;
            }
            
            return path;
        }
    }
}