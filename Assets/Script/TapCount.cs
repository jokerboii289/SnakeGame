using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dreamteck.Splines;

public class TapCount : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] GameObject destroyparticleEffect;
    [SerializeField] GameObject Bosspos;
    [SerializeField] GameObject particleEffect;
    [SerializeField] Vector3 offset;

    [SerializeField] GameObject boss;
    bool canTap;
    [SerializeField] GameObject dummyHead;
    [SerializeField] GameObject dummyBody;

    public static TapCount instance;
    [SerializeField] GameObject vCamera;
    [SerializeField] GameObject dummySnake, spline;
    int count;

    float scoreCount;
    [SerializeField] TextMeshProUGUI scoreText;

  
   

    // Start is called before the first frame update
    void Start()
    {
        particle.SetActive(false);
        instance = this;
        vCamera.SetActive(true);
        count = 0;
        scoreCount = 0;
        canTap = false;
    }

    // Update is called once per frame
    void Update()
    {    //Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && canTap
        if (Input.GetMouseButtonDown(0) && canTap)
            {
                if (count <= 4)
                {
                    EffectBlood();
                    AudioManager.instance.Play("tapEffect");
                }
                if (count == 3)
                {
                    AudioManager.instance.Play("BossDeath");
                    particle.SetActive(true);
                    particle.GetComponent<ParticleSystem>().Play();

                    //call win panel
                    PuaseMenu.instance.WinInitiate();
                }
                boss.transform.localScale += new Vector3(.1f, .1f, .1f);
                count++;
               // print(count);
            }
            if (count > 3)
            {
                //dummySnake.SetActive(false);
                //spline.SetActive(false);
                StartCoroutine(Delay());
            }      
    }

    public void DeactivateCamera()
    {
        vCamera.SetActive(false);
    }

    public void TypeOfSnake(Material head,Material body)
    {
        dummyHead.GetComponent<SkinnedMeshRenderer>().material = head;
        dummyBody.GetComponent<MeshRenderer>().material = body;
    }

    public void ScoreIncreament()
    {
        scoreCount++;

        if (scoreCount >= 0)
            scoreText.text = scoreCount.ToString();
    }

    public void ScoreDecreament()  // score decrease
    { 
        if(scoreCount>0)
            scoreCount--;
        if(scoreCount>=0)
            scoreText.text = scoreCount.ToString();
    }

    public void CanTap()
    {
        canTap = true;
    }

    void EffectBlood()
    {
        //var obj = ObjectPooling.instance.GetFromPool(particleEffect);  //blood effect
        //if (obj != null)
        //{
        //    obj.transform.position = transform.position+ offset;
        //}
        Instantiate(particleEffect,Bosspos.transform.position + offset, Quaternion.identity); 
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0f);
        dummySnake.SetActive(false);
        spline.SetActive(false);
    }

}
