using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Audio;

//The Game Manager keeps track of which scenes to load, handles loading scenes, fading between scenes and also video playing/pausing

namespace Interactive360
{

    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;

        Scene scene;
        VideoPlayer video;
        Animator anim;
        Image fadeImage;

        AsyncOperation operation;


        [Header("Scene Management")]
        public string[] scenesToLoad;
        public string activeScene;

        [Space]
        [Header("UI Settings")]

        public bool useFade;
        public GameObject fadeOverlay;
        public GameObject ControlUI;
        public GameObject LoadingUI;

        // custom code
        public string currentScene = "";
        public GameObject[] destroyObjects;
        static public AudioMixer mixer;
        private float defaultVolume;



        //make sure that we only have a single instance of the game manager
        void Awake()
        {
            print("Awake");
            mixer = Resources.Load("MenuMixer") as AudioMixer; 
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        
        //set the initial active scene
        void Start()
        {
            scene = SceneManager.GetActiveScene();
            activeScene = scene.buildIndex + " - " + scene.name;
            print("Start");
            currentScene = SceneManager.GetActiveScene().name;
        }

        private void Update()
        {
            float volume;
            mixer.GetFloat("MasterVolume", out volume);
            print(volume);
           
            if(currentScene != SceneManager.GetActiveScene().name)
            {
                print("AudioFadeIn");
                //StartCoroutine(AudioFadeIn(0.2f));

                foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                {
                    if (obj.name == "Hotspots")
                        ControlUI = obj;
                    if (obj.name == "Loading")
                        LoadingUI = obj;
                    if (obj.name == "AutoChange")
                        scenesToLoad[0] = obj.GetComponent<AutoChange>().refScene;
                }

                currentScene = SceneManager.GetActiveScene().name;
            }
        }

        //Select scene is called from either the menu manager or hotspot manager, and is used to load the desired scene
        public void SelectScene(string sceneToLoad, bool use)
        {
            mixer.GetFloat("MasterVolume", out defaultVolume);
            

            StartCoroutine(AudioFadeOut(0.7f));
            

            if(!use)
            {
                SceneManager.LoadScene(sceneToLoad);
                return;
            }

            //if we want to use the fading between scenes, start the coroutine here
            if (useFade)
            {
                StartCoroutine(FadeOutAndIn(sceneToLoad));

            }
            //if we dont want to use fading, just load the next scene
            else
            {
                SceneManager.LoadScene(sceneToLoad);
                foreach (GameObject obj in destroyObjects)
                {
                    print(obj.name);
                    Destroy(obj);
                }
            }
            //set the active scene to the next scene
            activeScene = sceneToLoad;
        }

        IEnumerator AudioFadeOut(float seconds)
        {
            float volume;
            
            mixer.GetFloat("MasterVolume", out volume);
            float segment = Mathf.Abs((defaultVolume - (-80f)) / (seconds * 10));
            print("fadeout" + segment);
            for (;volume > -80f;)
            {
                yield return new WaitForSeconds(0.1f);

                volume -= segment;
                mixer.SetFloat("MasterVolume", volume);

                if(volume < -80f)
                {
                    volume = -80f;
                    mixer.SetFloat("MasterVolume", volume);
                }
            }


            print("fadein" + segment);
            for (; volume < defaultVolume;)
            {
                yield return new WaitForSeconds(0.1f);

                volume += segment;
                mixer.SetFloat("MasterVolume", volume);

                if (volume > defaultVolume)
                {
                    volume = defaultVolume;
                    mixer.SetFloat("MasterVolume", volume);
                }
            }
        }

        //IEnumerator AudioFadeIn(float seconds)
        //{
        //    float volume;
            
        //    mixer.GetFloat("MasterVolume", out volume);
        //    float segment = Mathf.Abs((defaultVolume - (-80f)) / (seconds * 10));
            

            
        //}

        IEnumerator FadeOutAndIn(string sceneToLoad)
        {
            //get references to animatior and image component 
            anim = fadeOverlay.GetComponent<Animator>();
            fadeImage = fadeOverlay.GetComponent<Image>();

            //turn control UI off and loading UI on
            if (ControlUI != null)
                ControlUI.SetActive(true);
            ControlUI.SetActive(false);
            if(LoadingUI != null)
                LoadingUI.SetActive(true);

            //set FadeOut to true on the animator so our image will fade out
            anim.SetBool("FadeOut", true);

            //wait until the fade image is entirely black (alpha=1) then load next scene
            yield return new WaitUntil(() => fadeImage.color.a == 1);
            SceneManager.LoadScene(sceneToLoad);
            Scene scene = SceneManager.GetSceneByName(sceneToLoad);
            Debug.Log("loading scene:" + scene.name);
            yield return new WaitUntil(() => scene.isLoaded);

            // grab video and wait until it is loaded and prepared before starting the fade out
            video = FindObjectOfType<VideoPlayer>();
            yield return new WaitUntil(() => video.isPrepared);

            //set FadeOUt to false on the animator so our image will fade back in 
            anim.SetBool("FadeOut", false);
            
            //wait until the fade image is completely transparent (alpha = 0) and then turn loading UI off and control UI back on
            yield return new WaitUntil(() => fadeImage.color.a == 0);
            LoadingUI.SetActive(false);
            
            //if we have not destroyed the control UI, set it to active
            if (ControlUI) 
            ControlUI.SetActive(true);

            print("destroy");
            foreach(GameObject obj in destroyObjects)
            {
                print(obj.name);
                Destroy(obj);
            }



        }

        //Find the video in the scene and pause it
        public void PauseVideo()
        {
            if (!video)
            {
                video = FindObjectOfType<VideoPlayer>();
            }
            video.Pause();
        }

        //Find the video in the scene and play it
        public void PlayVideo()
        {
            if (!video)
            {
                video = FindObjectOfType<VideoPlayer>();
            }
            video.Play();
        }
    }
}

