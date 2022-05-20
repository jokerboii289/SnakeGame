using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class AISnake : MonoBehaviour
{
    //change Speed Of snake at certain Intervals
    [SerializeField] Transform playerHeadNode;
    float holdOriginalSpeed;
    [SerializeField] float makeSpeedDecraseBy;

    [SerializeField] float timeIntervalToStay;//time interval it can stay with play
    float timer;
    bool speedDecreased;
    [SerializeField] float distanceApart;//if the ai snakes distance with player snake is grreater than distance apart then incearse the ai snake speed;
    bool startRaycasting;


    bool canChangeToPoisionColor;
    [SerializeField] GameObject bomb;
    [SerializeField] Transform showCaseprefabpoint;
    [SerializeField] GameObject[] prefabShowcase;


    // [SerializeField] GameObject TaptoCrushText;
    //[SerializeField] Texture hhead;
    [SerializeField] Texture bbody;
    Texture originalTextureHolder;

    [SerializeField] Material headPoisonMateial;
    [SerializeField] Material bodyPoisonMateial;
    Material originalHead;
    //poison material


   // [SerializeField] Transform point;

    [SerializeField] GameObject[] bloodEffectSplat;    //effect
    [SerializeField] GameObject[] bloodEffect;
    [SerializeField] Animator snakeHead;
    [SerializeField] Vector3 offset;
    bool eating;
    float shalowTimer;
    [SerializeField]bool colorOne;
    [SerializeField]bool colorTwo;
    [SerializeField]bool colorThree;
    [SerializeField]bool colorFour;


    [SerializeField] Material headColor1;
    [SerializeField] Material headColor2;
    [SerializeField] Material headColor3;
    [SerializeField] Material headColor4;
    [SerializeField] Material bodymaterial1;
    [SerializeField] Material bodymaterial2;
    [SerializeField] Material bodymaterial3;
    [SerializeField] Material bodymaterial4;
    [SerializeField] GameObject head;

    //color looping
    float time;

    // Start is called before the first frame update
    void Start()
    {
        startRaycasting = false;
        timer = 0;
        speedDecreased = false;

        //TaptoCrushText.SetActive(false);
        canChangeToPoisionColor = true;

        //colorOne = true;
        //colorTwo = false;
        //colorThree = false;
        //colorFour = false;

        eating = false;

        holdOriginalSpeed = playerHeadNode.GetComponent<SplineFollower>().followSpeed;
    }

    // Update is called once per frame
    void Update()
    {
      //  SplinePlayerMove();
        if (Input.GetMouseButton(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (eating)
        {//activate shader bulge
            shalowTimer += .8f * Time.deltaTime;
            transform.parent.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Panner", shalowTimer);
        }

        //dreacrease the speed of the ai snake for certain amount of time
        if (!speedDecreased)
        {
            timer += 1 * Time.deltaTime;
            if (timer > timeIntervalToStay)
            {
                gameObject.GetComponent<SplineFollower>().followSpeed = holdOriginalSpeed - makeSpeedDecraseBy;
                timer = 0;
                speedDecreased = true;
            }
        }
        //print(speedDecreased);
        if(speedDecreased)
        {
             CheckDistance();
             if(startRaycasting)
            {
                ReturnToTheOriginalSpeed();
            }
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("stopai"))
        {
            print("print");
            gameObject.GetComponent<SplineFollower>().enabled =false;
        }
        if (other.gameObject.CompareTag("obstacle"))
        {
            Instantiate(bomb, transform.position, Quaternion.identity);

            other.gameObject.SetActive(false);
            //Debug.Log("Obstacle hit !");
        }

        if (other.gameObject.CompareTag("green"))
        {
            AudioManager.instance.Play("snakeColorChange");
            transform.parent.gameObject.GetComponent<MeshRenderer>().material = bodymaterial2;
            head.GetComponent<SkinnedMeshRenderer>().material = headColor2;

            colorOne = false;
            colorTwo = true;
            colorThree = false;
            colorFour = false;
        }

        if (other.gameObject.CompareTag("red"))
        {
            AudioManager.instance.Play("snakeColorChange");
            transform.parent.gameObject.GetComponent<MeshRenderer>().material = bodymaterial1;
            head.GetComponent<SkinnedMeshRenderer>().material = headColor1;
            colorTwo = false;
            colorOne = true;
            colorFour = false;
            colorThree = false;
        }

        if (other.gameObject.CompareTag("blue"))
        {
            AudioManager.instance.Play("snakeColorChange");
            transform.parent.gameObject.GetComponent<MeshRenderer>().material = bodymaterial3;
            head.GetComponent<SkinnedMeshRenderer>().material = headColor3;
            colorTwo = false;
            colorOne = false;
            colorThree = true;
            colorFour = false;
        }

        if (other.gameObject.CompareTag("yellow"))
        {
            AudioManager.instance.Play("snakeColorChange");
            transform.parent.gameObject.GetComponent<MeshRenderer>().material = bodymaterial4;
            head.GetComponent<SkinnedMeshRenderer>().material = headColor4;
            colorTwo = false;
            colorOne = false;
            colorThree = false;
            colorFour = true;
        }

        if (other.gameObject.CompareTag("rededibles"))
        {
            FieldOfView.instance.EnableDetect();
            AudioManager.instance.Play("snakeEating");
            SpawnShowCase(0);
            EffectBlood(bloodEffect[0], bloodEffectSplat[0]);
            snakeHead.SetBool("Eat", true);
            eating = true;
            StartCoroutine(Disable());
            StartCoroutine(ResetPanner());
            other.gameObject.SetActive(false);
            if (!colorOne)
            {
                if (canChangeToPoisionColor)
                    StartCoroutine(PoisonActivate());
            }
        }
        if (other.gameObject.CompareTag("greenedibles"))
        {
            FieldOfView.instance.EnableDetect();
            AudioManager.instance.Play("snakeEating");
            SpawnShowCase(1);
            EffectBlood(bloodEffect[1], bloodEffectSplat[1]);
            snakeHead.SetBool("Eat", true);
            eating = true;
            StartCoroutine(Disable());
            StartCoroutine(ResetPanner());
            other.gameObject.SetActive(false);
            if (!colorTwo)
            {
                if (canChangeToPoisionColor)
                    StartCoroutine(PoisonActivate());
            }
          
        }
        if (other.gameObject.CompareTag("blueedibles"))
        {
            FieldOfView.instance.EnableDetect();
            AudioManager.instance.Play("snakeEating");
            SpawnShowCase(2);
            EffectBlood(bloodEffect[2], bloodEffectSplat[2]);
            snakeHead.SetBool("Eat", true);
            eating = true;
            StartCoroutine(Disable());
            StartCoroutine(ResetPanner());
            other.gameObject.SetActive(false);
            if (!colorThree)
            {
                if (canChangeToPoisionColor)
                    StartCoroutine(PoisonActivate());
            }          
        }

        if (other.gameObject.CompareTag("yellowedibles"))
        {
            FieldOfView.instance.EnableDetect();
            AudioManager.instance.Play("snakeEating");
            SpawnShowCase(3);
            EffectBlood(bloodEffect[3], bloodEffectSplat[3]);
            snakeHead.SetBool("Eat", true);
            eating = true;
            StartCoroutine(Disable());
            StartCoroutine(ResetPanner());
            other.gameObject.SetActive(false);
            if (!colorFour)
            {
                if (canChangeToPoisionColor)
                    StartCoroutine(PoisonActivate());
            }
        }
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(.5f);
        snakeHead.SetBool("Eat", false);
    }

    IEnumerator ResetPanner()
    {
        yield return new WaitForSeconds(1f);
        eating = false;
        shalowTimer = -0.3f;
        transform.parent.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Panner", shalowTimer);
    }

    void EffectBlood(GameObject bloodEffect, GameObject bloodSpat)
    {
        //SpawnShowCase();

        var obj = ObjectPooling.instance.GetFromPool(bloodEffect);  //blood effect
        if (obj != null)
        {
            obj.transform.position = transform.position;
        }
        Instantiate(bloodSpat, transform.position + offset, Quaternion.identity); //bloodEffect spat;
    }


    void ColorLooping()
    {
        time += 5 * Time.deltaTime;
        var temp = Mathf.Sin(time);
        head.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_IndirectIntensity", temp);
    }

    IEnumerator PoisonActivate()
    {
        canChangeToPoisionColor = false;
        originalHead = head.GetComponent<SkinnedMeshRenderer>().material;   //roiginal head color
        originalTextureHolder = transform.parent.gameObject.GetComponent<MeshRenderer>().material.GetTexture("_TextureSample0"); //original texture
        transform.parent.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_TextureSample0", bbody);//body change
        head.GetComponent<SkinnedMeshRenderer>().material = headPoisonMateial; // head change

        yield return new WaitForSeconds(.7f);
        transform.parent.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_TextureSample0", originalTextureHolder);
        head.GetComponent<SkinnedMeshRenderer>().material = originalHead;
        canChangeToPoisionColor = true;
    }

    void SpawnShowCase(int index)
    {
        GameObject obj = Instantiate(prefabShowcase[index]);

        obj.transform.position = showCaseprefabpoint.position;
        obj.transform.eulerAngles = new Vector3(90, 0, 0);
        obj.transform.SetParent(showCaseprefabpoint);
    }

    //make snake to slow down At certain intervals
     
    void CheckDistance()
    {
        var distance = (transform.position-playerHeadNode.position).magnitude;
        if(distance>distanceApart)
        {
            gameObject.GetComponent<SplineFollower>().followSpeed = playerHeadNode.GetComponent<SplineFollower>().followSpeed + makeSpeedDecraseBy;
            startRaycasting = true;
        } 
    }

    void ReturnToTheOriginalSpeed()
    {
        //print("okk");
        RaycastHit hit;
        //raycast to check if the AI snake head align with the Player head
      
        if( Physics.Raycast(transform.position, transform.right,out hit,50))
        {
            if(hit.collider.tag=="Player")
            {
                gameObject.GetComponent<SplineFollower>().followSpeed = playerHeadNode.GetComponent<SplineFollower>().followSpeed;
                //gameObject.GetComponent<SplineFollower>().followSpeed = playerHeadNode.GetComponent<SplineFollower>().followSpeed;
                startRaycasting = false;
                speedDecreased = false;
            }
        }
        if (Physics.Raycast(transform.position, -transform.right, out hit, 100))
        {
            if (hit.collider.tag == "Player")
            {
                gameObject.GetComponent<SplineFollower>().followSpeed = playerHeadNode.GetComponent<SplineFollower>().followSpeed;
                startRaycasting = false;
                speedDecreased = false;
            }
        }
    }


    void Assign()
    {
       // if(head.GetComponent<SkinnedMeshRenderer>().material==headColor1[])
       // for(int i=0;i<headColor1)
    }
  
}
