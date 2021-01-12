using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb2d; //RigidBody нашего персонажа
    private SpriteRenderer spriterend; //SpriteRenderer нашего персонажа
    private bool IsGrounded = true; // Проверка, стоит ли персонаж на земле
    public float runSpeed = 5f; //Скорость бега персонажа
    public float jumpSpeed = 25f; //Высота прыжка персонажа
    private Animator anim; //Аниматор нашего персонажа
    private bool isMoving = false; //Движется ли наш персонаж в данное время


    private bool lookRight = true; //Смотрит ли персонаж сейчас вправо?
    public Transform bulletPos; //Позиция нашей пули
    public GameObject bullet; //Сама наша пуля как игровой объект
    public static bool canShoot = true; // Можно ли сейчас ему стрелять?
    public static int health = 5; //Здоровье нашего персонажа
    public float fireCooldown = 0.5f; //Перерыв между выстрелами в секундах

    [SerializeField]
    Transform GroundCheck; //Центральный объект проверки касания земли
    [SerializeField]
    Transform GroundCheck_L; //Левый объект проверки касания земли
    [SerializeField]
    Transform GroundCheck_R; //Правый объект проверки касания земли

    void Start()
    {
        
        rb2d = GetComponent<Rigidbody2D>(); //Получаем компонент Rigidbody2D нашего персонажа и присваиваем в переменную
        spriterend = GetComponent<SpriteRenderer>(); //Получаем компонент SpriteRendered нашего персонажа и присваиваем в переменную
        anim = GetComponent<Animator>(); //Получаем компонент Animator нашего персонажа и присваиваем в переменную
    }

    // Update is called once per frame
    void Update()
    {
        //Условие смерти персонажа если его здоровье меньше или равно 0
        if (health <= 0)
        {
            StartCoroutine(Death()); //Запуск коротины смерти
        } 
        float move = Input.GetAxis("Horizontal"); //Получение значения горизонтальной оси ввода и запись в переменную
        anim.SetBool("isMoving", isMoving); //Связь между переменными в аниматоре и в скрипте
        anim.SetBool("isGrounded", IsGrounded);
        anim.SetInteger("Health", health);
        
        //GroundCheck проверка через условие
        if (Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, GroundCheck_L.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, GroundCheck_R.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }


        if (Input.GetKey(KeyCode.D) || Input.GetKey("right")) //Движение персонажа вправо
        {
            rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y);
            //spriterend.flipX = false;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey("left")) //Движение персонажа влево
        {
            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);
            //spriterend.flipX = true;
            isMoving = true;
          
        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("up")) && IsGrounded == true) //Прыжок персонажа
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        //Остановка анимации бега
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp("right") || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp("left")) 
        {
            isMoving = false;
        }

        //Поворот персонажа на 180 градусов при движении вправо
        if (move > 0 && !lookRight)
        {
            Flip();
        }
        else if (move < 0 && lookRight) //Поворот персонажа на 180 градусов при движении влево
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.F) && canShoot==true) //Стрельба персонажа
        {
            Instantiate(bullet, bulletPos.position, bulletPos.rotation);
            canShoot = false;
            StartCoroutine(FireCoolDown());
        }

    }

    void Flip() //Метод поворота персонажа
    {
        lookRight = !lookRight;
        transform.Rotate(0, 180f, 0);
    }


    IEnumerator FireCoolDown() //Установка времени задержки между выстрелами
    {
        yield return new WaitForSeconds(fireCooldown);
        canShoot = true;
    }

    //Проверка касания персонажем врага
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            health--;
        }
    }

    //Коротина смерти
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(0.8f);
        health = 5;
        SceneManager.LoadScene(0);
    }
}
