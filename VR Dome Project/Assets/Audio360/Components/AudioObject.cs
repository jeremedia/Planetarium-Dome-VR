/*
 *  Copyright 2013-Present Two Big Ears Limited
 *  All rights reserved.
 *  http://twobigears.com
 */

using System.Collections;
using UnityEngine;

namespace TBE
{
    /// <summary>
    /// Experimental.
    /// </summary>
    public class AudioObject : MonoBehaviour
    {
        public string file;
        public bool playOnStart = false;
        public EventListener events = new EventListener();

        NativeAudioObject nativeObj_;
        Renderer prevRenderer_;

        void Awake()
        {
            init();
        }

        void Start()
        {
            if (nativeObj_ != null && file != null && file.Length > 0)
            {
                if (!open(file))
                {
                    Utils.logError("Failed to open " + file, this);
                    return;
                }

                if (playOnStart)
                {
                    nativeObj_.play();
                }
            }
        }

        public void init()
        {
            if (nativeObj_ != null)
            {
                return;
            }

            nativeObj_ = AudioEngineManager.Instance.nativeEngine.createAudioObject();
            if (nativeObj_ == null)
            {
                Utils.logError("Native audio object is invalid", this);
                return;
            }

            nativeObj_.setEventListener(events);
        }

        void Update()
        {
            if (nativeObj_ != null)
            {
                nativeObj_.setPosition(Utils.toTBVector(transform.position));
            }
        }

        public bool open(string file)
        {
            if (nativeObj_ != null)
            {
                return (nativeObj_.open(file) == EngineError.OK);
            }
            return false;
        }

        public void close()
        {
            if (nativeObj_ != null)
            {
                nativeObj_.close();
            }
        }

        public bool isOpen()
        {
            if (nativeObj_ != null)
            {
                return nativeObj_.isOpen();
            }
            return false;
        }

        public void play()
        {
            if (nativeObj_ != null)
            {
                nativeObj_.play();
            }
        }

        public void stop()
        {
            if (nativeObj_ != null)
            {
                nativeObj_.stop();
            }
        }

        public void pause()
        {
            if (nativeObj_ != null)
            {
                nativeObj_.pause();
            }
        }

        public void seekToMs(float ms)
        {
            if (nativeObj_ != null)
            {
                nativeObj_.seekToMs(ms);
            }
        }

        public double getElapsedTimeInMs()
        {
            if (nativeObj_ != null)
            {
                return nativeObj_.getElapsedTimeInMs();
            }
            return 0.0;
        }

        public double getDurationInMs()
        {
            if (nativeObj_ != null)
            {
                return nativeObj_.getAssetDurationInMs();
            }
            return 0.0;
        }

        void OnDestroy()
        {
            if (nativeObj_ != null && AudioEngineManager.Instance != null && AudioEngineManager.Instance.nativeEngine != null)
            {
                AudioEngineManager.Instance.nativeEngine.destroyAudioObject(nativeObj_);
            }
        }

        static public TBE.AudioObject createAndPlayOnObject(GameObject ga, string file)
        {
            var obj = ga.AddComponent<AudioObject>();
            if (!obj.open(file))
            {
                Utils.logError("Failed to open " + file, null);
                Destroy(obj);
                return null;
            }
            obj.play();
            return obj;
        }

        static public TBE.AudioObject createOnObject(GameObject ga, string file)
        {
            var obj = ga.AddComponent<AudioObject>();
            if (!obj.open(file))
            {
                Utils.logError("Failed to open " + file, null);
                Destroy(obj);
                return null;
            }
            return obj;
        }
    }
}