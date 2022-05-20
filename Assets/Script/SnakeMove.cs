using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SnakeMove : MonoBehaviour
{

    [SerializeField] GameObject fireBall;
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
    Material originalHead,originalBody;
    //poison material


    //[SerializeField] Transform [] points;
    [SerializeField] GameObject dummySnake;
    //points to warp around
   
    [SerializeField] Transform point;
   
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
    
    public SplineFollower splineFollower;
    private float Va;
    public float xPosClamp;
   
    public float speed;

    public static bool playerDie;

   
    float time;


    [SerializeField]bool IsAi;
   
    // Start is called before the first frame update
    void Start()
    {
        //TaptoCrushText.SetActive(false);
        canChangeToPoisionColor = true;
        dummySnake.SetActive(false);

        eating = false;
        playerDie = false;


        //colorOne = true;
        //colorTwo = false;
        //colorThree = false;
        //colorFour = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        SplinePlayerMove();
       
        if (Input.GetMouseButton(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(eating)
        {//activate shader bulge
            shalowTimer += .8f * Time.deltaTime;
            transform.parent.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Panner", shalowTimer);
        }
    }

    void SplinePlayerMove()
    {
        if (Input.GetMouseButton(0))
        {
            Va = Va + (GetInput()) * Time.deltaTime * speed;
            Va = Mathf.Clamp(Va, -xPosClamp, xPosClamp);
            splineFollower.motion.offset = new Vector2(Va, splineFollower.motion.offset.y);
        }
    }

    private float GetInput()
    {
#if UNITY_EDITOR
        if (InputHeldDown())
            return Input.GetAxis("Mouse X");

        return 0;
#elif !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		return Input.GetTouch(0).deltaPosition.x;
#endif
    }

    private bool InputHeldDown()
    {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		return Input.touchCount != 0;
#endif


#if UNITY_EDITOR
        return Input.GetMouseButton(0);
#elif !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		return Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary;
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("lava"))
        {
            var obj = Instantiate(fireBall);
            obj.transform.position = transform.position;
            obj.transform.SetParent(transform);
            AudioManager.instance.Play("snakeonfire");
            StartCoroutine(DelayFail());
        }

        if(other.gameObject.CompareTag("obstacle"))
        {
            Instantiate(bomb, transform.position, Quaternion.identity);
            PuaseMenu.instance.FailInitiate();

            other.gameObject.SetActive(false);
            print("Obstacle collision !!");
        }

        if(other.gameObject.CompareTag("obstacle2"))
        {
            //Instantiate(bomb, transform.position, Quaternion.identity);
            if(colorOne)
            {
                EffectBlood(bloodEffect[0], bloodEffectSplat[0]);
            }
            if (colorTwo)
            {
                EffectBlood(bloodEffect[1], bloodEffectSplat[1]);
            }
            if(colorThree)
            {
                EffectBlood(bloodEffect[2], bloodEffectSplat[2]);
            }
            if (colorFour)
            {
                EffectBlood(bloodEffect[3], bloodEffectSplat[3]);
            }

            AudioManager.instance.Play("obstacle2hit");
            PuaseMenu.instance.FailInitiate();

            //other.gameObject.SetActive(false);
            
            print("Obstacle collision !!");
        }

        if(other.gameObject.CompareTag("green"))
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
            AudioManager.instance.Play("snakeEating");
            SpawnShowCase(0);
            EffectBlood(bloodEffect[0],bloodEffectSplat[0]);
            snakeHead.SetBool("Eat", true);
            eating = true;
            StartCoroutine(Disable());
            StartCoroutine(ResetPanner());
            other.gameObject.SetActive(false);
            if (!colorOne)
            {
                TapCount.instance.ScoreDecreament();
                print("die");
                playerDie = true;

                if (canChangeToPoisionColor)
                    StartCoroutine(PoisonActivate());
            }
            else
                TapCount.instance.ScoreIncreament();
        }
        if (other.gameObject.CompareTag("greenedibles"))
        {
           
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
                TapCount.instance.ScoreDecreament();
                print("die");
                playerDie = true;

                if (canChangeToPoisionColor)
                    StartCoroutine(PoisonActivate());
            }
            else
                TapCount.instance.ScoreIncreament();
        }
        if (other.gameObject.CompareTag("blueedibles"))
        {
            
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
                TapCount.instance.ScoreDecreament();
                print("die");
                playerDie = true;


                if (canChangeToPoisionColor)
                    StartCoroutine(PoisonActivate());
            }
            else
                TapCount.instance.ScoreIncreament();
        }
        if (other.gameObject.CompareTag("yellowedibles"))
        {

           
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
                TapCount.instance.ScoreDecreament();
                print("die");
                playerDie = true;
                
                if(canChangeToPoisionColor)
                    StartCoroutine(PoisonActivate());
            }
            else
                TapCount.instance.ScoreIncreament();
        }

        if(other.gameObject.CompareTag("climbpoint"))
        {
            gameObject.GetComponent<SplineFollower>().enabled = false;
            //climb = true;
            StartCoroutine(DelayDeactivate());
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

    void EffectBlood( GameObject bloodEffect , GameObject bloodSpat)
    {
        //SpawnShowCase();

        var obj = ObjectPooling.instance.GetFromPool(bloodEffect);  //blood effect
        if(obj!=null)
        {
            obj.transform.position = transform.position;
        }
        Instantiate(bloodSpat, transform.position + offset, Quaternion.identity); //bloodEffect spat;
    }


    void ColorLooping()
    {
        time += 5 * Time.deltaTime;
        var temp = Mathf.Sin(time);
        head.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_IndirectIntensity",temp);
    }

    IEnumerator DelayDeactivate()
    {
        TapCount.instance.DeactivateCamera();
        dummySnake.SetActive(true);
        //StartCoroutine(Delay());
        transform.DOMove(point.position, 1);

        //change the texture of snake same as original snake
        var bodyMaterial = transform.parent.gameObject.GetComponent<MeshRenderer>().material;
        var headMaterial= head.GetComponent<SkinnedMeshRenderer>().material;
        TapCount.instance.TypeOfSnake(headMaterial,bodyMaterial);
        yield return new WaitForSeconds(2f);
        TapCount.instance.CanTap();
        //TaptoCrushText.SetActive(true);
        transform.root.gameObject.SetActive(false);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        dummySnake.SetActive(true);
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

    IEnumerator DelayFail()
    {
        yield return new WaitForSeconds(1f);
        PuaseMenu.instance.FailInitiate();
    }

}
