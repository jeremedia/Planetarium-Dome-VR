using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class ProjectManager : MonoBehaviour {

    // Video Sync Solving Forum https://forum.unity3d.com/threads/projecting-a-360-degree-vr-video-on-a-sphere.376522/
    // Threading Issue Solution https://forum.unity3d.com/threads/20usd-easy-threading-multi-threading-made-easy.262860/page-4

    public MediaPlayerCtrl leftVideo;
    public MediaPlayerCtrl rightVideo;
    public Slider ProgressOfVideoValue;
    public TBE.SpatDecoderFile AmbisonicSound;

    [SerializeField]
    private MediaPlayer SubtitleMedia;

    //   public AudioSource StereoAudioSource;

    public GameObject MessagePanel;
    public static bool isVideoCompleted = false;
    public static bool isExperienceStarted = false;

    void Awake()
    {
        leftVideo.m_bAutoPlay = false;
        rightVideo.m_bAutoPlay = false;
    }

    // Use this for initialization
    void Start ()
    {
        if (System.IO.Directory.Exists(Application.persistentDataPath + "/Data") == false)
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Data");

            leftVideo.Load("file://" + Application.persistentDataPath + "/Data/" + "leftlinear.mp4");
            rightVideo.Load("file://" + Application.persistentDataPath + "/Data/" + "rightlinear.mp4");

            MessagePanel.SetActive(false);
            isVideoCompleted = false;
            isExperienceStarted = false;
            SubtitleMedia.Stop();
        if (AmbisonicSound.isOpen() == false)
        {
            Debug.Log("File AmbisonicSound Openening.");

            if (!AmbisonicSound.open("TwoBigEars 3.tbe"))
            {
                Debug.Log("AmbisonicSound Failed to open file.");
                return;
            }
            else
            {
                Debug.Log("File AmbisonicSound is Opened.");
            }
        }

        AmbisonicSound.syncMode = TBE.SyncMode.EXTERNAL;

        // Task.Run(OnVideoSyncPlay);
        StartCoroutine(LoopVideo());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isVideoCompleted)
            {
                StartCoroutine(LoopVideo());
                MessagePanel.SetActive(false);
                OVRManager.display.RecenterPose();
            }
        }

        if (isExperienceStarted)
        {
            float leftvideoTime = leftVideo.GetSeekPosition();
            float rightVideoTime = rightVideo.GetSeekPosition();
            AmbisonicSound.setExternalClockInMs(leftvideoTime);

   //         Debug.Log("leftvideoTime : " + (leftvideoTime));
   //         Debug.Log("rightVideoTime  : " + (rightVideoTime));
   //         Debug.Log("AmbisonicSoundTime : " + AmbisonicSound.getElapsedTimeInMs());
        }
    }
    
    private IEnumerator LoopVideo()
    {
        isVideoCompleted = false;
        ProgressOfVideoValue.value = 0.0f;
              
        yield return new WaitForSeconds(2.0f);

        isExperienceStarted = true;

        leftVideo.SeekTo(0);
        rightVideo.SeekTo(0);

        leftVideo.Play();
        rightVideo.Play();

        SubtitleMedia.Control.Seek(0.0f);
        SubtitleMedia.Play();

        AmbisonicSound.play();
        AmbisonicSound.seekToMs(0);

        yield return new WaitForSeconds(0.3f);

    //    StereoAudioSource.Play(0);

        while (isVideoCompleted != true)
        {
            if (ProgressOfVideoValue.value >= 0.997f)
            {
                isVideoCompleted = true;                       
            }
            yield return null; // wait until next frame
        }

        MessagePanel.SetActive(true);

        leftVideo.Stop();
        rightVideo.Stop();

        SubtitleMedia.Control.Seek(0.0f);
        SubtitleMedia.Stop();

        AmbisonicSound.stop();

        //       StereoAudioSource.Stop();
        ProgressOfVideoValue.value = 0.0f;
        isExperienceStarted = false;
        //      leftVideo.SeekTo(0);
        //      rightVideo.SeekTo(0);

        yield break;
    }
}