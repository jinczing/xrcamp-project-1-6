using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Ending : MonoBehaviour
{
    public float margin;
    public float showtime;

    public string[] credits;
    public float[] seconds;

    public Text credit;


    public float videoMargin;

    public VideoPlayer player;

    public AudioSource source;


    private void Awake()
    {
        StartCoroutine(PlayCredit());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayCredit()
    {
        player.Pause();
        yield return new WaitForSeconds(videoMargin);
        player.Play();
        source.PlayOneShot(source.clip);
        yield return new WaitForSeconds(margin);

        for(int i=0; i<credits.Length; ++i)
        {
            credit.text = credits[i];
            while(credit.color.a < 1)
            {
                credit.color = new Color(credit.color.r, credit.color.g, credit.color.b, credit.color.a + 0.1f);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(showtime);
            if (i == credits.Length - 1)
                break;
            while(credit.color.a > 0)
            {
                credit.color = new Color(credit.color.r, credit.color.g, credit.color.b, credit.color.a - 0.1f);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(seconds[i]);
        }
    }
}
