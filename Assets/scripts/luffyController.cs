using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class luffyController : MonoBehaviour
{
    private float vidaLuffy, VidaActualLuffy=1;
    public Image barraSaludLuffyimg;
    public Image barraEnergiaLuffyimg;
    public Image barraVidaFrezerimg;
    private float vidaFrezer, VidaActualFrezer=1;
    private float EnergiaLuffy,EnergiaActualLuffy=0.2f;
    public float velocidad = 5;
    private const int ANIMATION_QUIETO = 0;
    private const int ANIMATION_CORRER = 1;
    private const int ANIMATION_PUÑO = 3;
    private const int ANIMATION_REDHAWK= 2;
    private const int ANIMATION_CAE = 4;
    private const int ANIMATION_SALTAR=5;
    private const int ANIMATION_PUÑO_3MARCHA=6;
    private const int ANIMATION_TRANSFORMACION=10;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;
    private bool EstaSaltando = false;
    public float fuerzaSalto = 10;
    public BoxCollider2D plataform;
    public GameObject nubeInvok;
    public static luffyController instance;
    public bool EstaAtacando=false;
    public bool estaTransformandose=false;
    public bool estaCayendo=false;
    //datos para manejar los ataques con las coliciones
    public bool animacionCaer=false;
    private void Awake(){
        if(instance==null){
            instance=this;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !EstaSaltando)
            {
                CambiarAnimacion(ANIMATION_SALTAR);
                Saltar();
                EstaSaltando = true;
            }
            else if (Input.GetKey(KeyCode.D))//Si presiono la tecla rigtharrow voy a ir hacia la derecha
            {
                estaCayendo=false;
                rb.velocity = new Vector2(velocidad, rb.velocity.y);//velocidad de mi objeto
                CambiarAnimacion(ANIMATION_CORRER);//Accion correr 
                spriteRenderer.flipX = false;//Que mi objeto mire hacia la derecha
                
                if (Input.GetKey(KeyCode.W) && !EstaSaltando)
                {
                    CambiarAnimacion(ANIMATION_SALTAR);
                    Saltar();
                    EstaSaltando = true;
                }
            }
            
            else if (Input.GetKey(KeyCode.A))
            {
                estaCayendo=false;
                rb.velocity = new Vector2(-velocidad, rb.velocity.y);
                CambiarAnimacion(ANIMATION_CORRER);
                spriteRenderer.flipX = true;
                /*if(Input.GetKey(KeyCode.C))
                {
                    
                    CambiarAnimacion(ANIMATION_SLIDE);
                }
            */if (Input.GetKey(KeyCode.W) && !EstaSaltando)
                {
                    CambiarAnimacion(ANIMATION_SALTAR);
                    Saltar();
                    EstaSaltando = true;
                  
                }
            }else if (Input.GetKey(KeyCode.F))
            {
               CambiarAnimacion(ANIMATION_PUÑO);
               Invoke("isAttack",1.5f);
            }else if (Input.GetKey(KeyCode.C))
            {
                EstaAtacando=true;
               CambiarAnimacion(ANIMATION_REDHAWK);
               empujeadelante();
               Invoke("isAttack",1.5f);

            }else if (Input.GetKey(KeyCode.G))
            {
               CambiarAnimacion(ANIMATION_PUÑO_3MARCHA);
               empujeadelante();
                Invoke("isAttack",1.5f);
            } else if(EstaSaltando==false&&EstaAtacando==false&&estaTransformandose==false&&estaCayendo==false)            
            {
                CambiarAnimacion(ANIMATION_QUIETO);//Metodo donde mi objeto se va a quedar quieto
                rb.velocity = new Vector2(0, rb.velocity.y);//Dar velocidad a nuestro objeto
            }
             if(VidaActualLuffy<=0){
                 CambiarAnimacion(ANIMATION_CAE);
                Invoke("luffyMuere",1.5f);
            }
             if(VidaActualFrezer<=0){
                  
                Invoke("FrezerMuere",1.5f);
            }
           
            if(Input.GetKey(KeyCode.Q)){
                estaTransformandose=true;
                CambiarAnimacion(ANIMATION_TRANSFORMACION);
                 
                Invoke("transformacion",1f/2);
                Destroy(gameObject, 1f);
            //time count up//
           
                
                 
            }
            if(Input.GetKeyUp(KeyCode.Q)){
                 
                
                
            }
           // Debug.Log(VidaActualLuffy);
           // Debug.Log(EnergiaActualLuffy);
            
    }
    private int Gear4=1;
    public void transformacion (){

        if(Gear4==1){
            var position = new Vector3(transform.position.x , transform.position.y , transform.position.z-1);
            Instantiate(nubeInvok, position, nubeInvok.transform.rotation);
            Gear4=0;
        }

    }
    public void FrezerMuere(){
        Destroy(this.gameObject);
    }
    public void luffyMuere(){
        Destroy(this.gameObject);
    }
    public void isAttack(){
        EstaAtacando=false;  
    }
    private void CambiarAnimacion(int animacion)
    {
        animator.SetInteger("Estado", animacion);
    }
      private void Saltar()
    {
        CambiarAnimacion(ANIMATION_SALTAR);
        rb.velocity = Vector2.up * fuerzaSalto;//Vector 2.up es para que salte hacia arriba
    }
    void empuje(){
        if (spriteRenderer.flipX)
                    {
 
                        rb.velocity = new Vector2(5, rb.velocity.y);
                    }
                    else
                    {
                       
                        rb.velocity = new Vector2(-5, rb.velocity.y);
                    }
    }
    void empujeadelante(){
        if (spriteRenderer.flipX)
                    {
 
                        rb.velocity = new Vector2(-5, rb.velocity.y);
                    }
                    else
                    {
                       
                        rb.velocity = new Vector2(5, rb.velocity.y);
                    }
    }
     void OnCollisionEnter2D(Collision2D other)
    {
        EstaSaltando = false;
        if (other.gameObject.tag == "supernova"&&EstaAtacando==true)
        {
            
            //DañoRecibeFrezer(0.3f);
            Destroy(other.gameObject);
            //Destroy(this.gameObject);
            //PersonajeController.IncrementerPuntajeEn10();
        }else if (other.gameObject.tag == "supernova"&&EstaAtacando==false)
        {
            estaCayendo=true;
            DañoRecibeLuffy(0.3f);
            CambiarAnimacion(ANIMATION_CAE);
            Invoke("empuje",1f/5);
            Destroy(other.gameObject);
            //Destroy(this.gameObject);
            //PersonajeController.IncrementerPuntajeEn10();
        }else if (other.gameObject.tag == "RayoMortal"&&EstaAtacando==true)
        {
             
            Destroy(other.gameObject);
            //Destroy(this.gameObject);
            //PersonajeController.IncrementerPuntajeEn10();
        }else if (other.gameObject.tag == "RayoMortal"&&EstaAtacando==false)
        {
             
            estaCayendo=true;
            DañoRecibeLuffy(0.2f);
            CambiarAnimacion(ANIMATION_CAE);
            Invoke("empuje",1f/5);
            Destroy(other.gameObject);
            
          
        }else if (other.gameObject.tag == "RayoCuerpoxcuerpo"&&EstaAtacando==true)
        {
             
            Destroy(other.gameObject);
            //Destroy(this.gameObject);
            //PersonajeController.IncrementerPuntajeEn10();
        }else if (other.gameObject.tag == "RayoCuerpoxcuerpo"||other.gameObject.tag == "puas"&&EstaAtacando==false)
        {
             
            estaCayendo=true;
            DañoRecibeLuffy(0.1f);
            CambiarAnimacion(ANIMATION_CAE);
            Invoke("empuje",1f/5);
            
          
        }else if(other.gameObject.tag == "Frezer"&&EstaAtacando==true)
        {
            DañoRecibeFrezer(0.1f);
        }
      
    }
    public void DañoRecibeLuffy(float daño)
    {
        //vidaFrezer=GetComponent<FrezerController>
        vidaLuffy=0.2f;
        VidaActualLuffy = VidaActualLuffy-daño;
        barraSaludLuffyimg.fillAmount = VidaActualLuffy ;
    }
    public void DañoRecibeFrezer(float daño)
    {
        //vidaFrezer=GetComponent<FrezerController>
        vidaFrezer=0.2f;
        VidaActualFrezer = VidaActualFrezer-daño;
        barraVidaFrezerimg.fillAmount = VidaActualFrezer ;
    }
    public void AumentaEnergiaLuffy()
    {
        //vidaFrezer=GetComponent<FrezerController>
        EnergiaLuffy=0.2f;
        EnergiaActualLuffy= EnergiaActualLuffy+EnergiaLuffy;
        barraEnergiaLuffyimg.fillAmount = EnergiaActualLuffy ;
    }
}
