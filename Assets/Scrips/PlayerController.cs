using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _renderer;
    private Animator _animator;
  
    
    public Text Vidas;
    public Text Monedas;
    
    public int vida = 3;
    public int monedas = 0;
    public int tempVida = 0;
    
    public GameObject kunaiPrefabs;
    public GameObject bulletPrefabs2;
    
    public float velocity = 20;
    public float JumpForce = 10;
        
    private static readonly int right = 1;
    private static readonly int left = -1;
        
         
    private static readonly int Animation_idle = 0;
    private static readonly int Animation_run = 1;
    private static readonly int Animation_jump = 2;
    private static readonly int Animation_escalera = 3;
    private static readonly int Animation_ataque = 4;
    private static readonly int Animation_paracaidas = 5;
    private static readonly int Animation_dead = 7;
    private static readonly int fin = 7;

    private int temporal = 0;
    private float time = 0f;
    private Puntajes Scors;

    void Start()
    { 
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        Scors = FindObjectOfType<Puntajes>();
    }

    // Update is called once per frame
    void Update()
    {
        if (temporal == 1)
        {
            ChangeAnimation(Animation_paracaidas);
        }
        else
        {
            
            Vidas.text=""+vida;
            Monedas.text =""+ monedas;
           
            
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            ChangeAnimation(Animation_idle);
            //ChangeAnimation(Animation_paracaidas);
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Desplazarse(right);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Desplazarse(left);
            }
            if(Input.GetKeyUp(KeyCode.Space))
            {
                ChangeAnimation(Animation_jump);
                _rigidbody2D.AddForce(Vector2.up*JumpForce,ForceMode2D.Impulse);
            }
            if(Input.GetKeyUp(KeyCode.C))
            {
                ChangeAnimation(Animation_ataque);
                Disparar2();
            
            }
            if (Scors.miVida == 0)
            {
                SceneManager.LoadScene("SampleScene");
            }

            timekey();
        }

       
    }
    void timekey()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            time = Time.time;

        }
         
        if(Input.GetKeyUp(KeyCode.X))
        {
            time = Time.time - time;
            Debug.Log("Pressed for : " + time + " Seconds");
            if (time >= 2f)
            {
                ChangeAnimation(Animation_ataque);
                Disparar();
            }

        }
    }
    private void Desplazarse(int position)
    {
        _rigidbody2D.velocity = new Vector2(velocity * position, _rigidbody2D.velocity.y);
        _renderer.flipX = position == left;
        ChangeAnimation(Animation_run);
    }
    private void Disparar()
    {
        
        //crear elementos en tiempo de ejecuccion
        var x = this.transform.position.x;
        var y = this.transform.position.y;
       

        var bullgo=Instantiate(kunaiPrefabs,new Vector2(x,y),Quaternion.identity) as GameObject;
        var controller = bullgo.GetComponent<CucchilloController>();
        
        controller.SetController(this);
        
        if (_renderer.flipX)
        {
            
            controller.velocity = controller.velocity * -1;
        }
    }
    private void Disparar2()
    {
        //crear elementos en tiempo de ejecuccion
        var x = this.transform.position.x;
        var y = this.transform.position.y;

        var bullgo=Instantiate(bulletPrefabs2,new Vector2(x,y),Quaternion.identity) as GameObject;
        var controller = bullgo.GetComponent<CucchilloController>();
        
        controller.SetController(this);
        
        if (_renderer.flipX)
        {
            
            controller.velocity = controller.velocity * -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var tag = other.gameObject.tag;
        if (tag == "Pinchos")
        {
            
            vida -= 3;
            if (vida <= 0)
            {
                SceneManager.LoadScene("SampleScene");
                Scors.MenosVida(1);
            }
            
            
        }
        if (tag == "Pinchos2")
        {
            SceneManager.LoadScene("SegundEcena");            
        }
        if (tag == "Nivel2")
        {
            SceneManager.LoadScene("SegundEcena");
        }

     
        if (tag == "cieloTop")
        {
           // Debug.Log("soy tope");
            temporal = 1;
            
            Debug.Log(temporal);
        }
        if (tag == "moneda1")
        {

            monedas += 1;
            Destroy(other.gameObject);
            
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        var tag = collision.gameObject.tag;
        if (tag == "Escalable" && Input.GetKey(KeyCode.UpArrow))
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 10);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemigo"))
        {
            Debug.Log("soy enemy");
            Scors.MenosVida(1);
            vida -= 1;
            if (vida <= 0)
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
        // if (other.gameObject.CompareTag("Moneda"))
        // {
        //     Scors.SumMonedas(1);
        // }
    }


    private void ChangeAnimation(int animation)
    {
        _animator.SetInteger("Estado",animation);
    }
}
