using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace sugi.cc
{
    public class ScreenCapture : MonoBehaviour
    {
        static ScreenCapture Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = FindObjectOfType<ScreenCapture>();
                if (_Instance == null)
                    _Instance = new GameObject("capture").AddComponent<ScreenCapture>();
                return _Instance;
            }
        }
        static ScreenCapture _Instance;
        public int fps = 10;
        public int superSize;

        bool recording;
        int framecount;

        // Use this for initialization
        void Start()
        {
            System.IO.Directory.CreateDirectory("Capture");
            Time.captureFramerate = fps;
            framecount = -1;
        }

        // Update is called once per frame
        void Update()
        {
            if (!recording) return;

            if (framecount > 0)
            {
                var name = "Capture/frame" + Time.frameCount.ToString("00000") + ".png";
                Application.CaptureScreenshot(name, superSize);
            }
            framecount++;
        }
#if UNITY_EDITOR
        [MenuItem("ScreenCapture/Start")]
        public static void StartRecording()
        {
            Instance.recording = true;
        }
        [MenuItem("ScreenCapture/Stop")]
        public static void StopRecording()
        {
            Instance.recording = false;
        }
#endif
    }
}