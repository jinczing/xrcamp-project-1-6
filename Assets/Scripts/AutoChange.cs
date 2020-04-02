using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Interactive360;

public class AutoChange : MonoBehaviour
{
    public string sceneToLoad;
    public string refScene;
    public GameObject[] destroyObjects;
    

    // Start is called before the first frame update
    void Awake()
    {
        GameManager.instance.SelectScene(sceneToLoad, false);
        foreach(GameObject obj in destroyObjects)
        {
            Destroy(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
