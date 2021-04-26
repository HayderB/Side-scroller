using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public Transform StartPosition;
    public Transform EndPosition;
    public GameObject Bullet;

    GameObject CreatedBullet;
    private bool spawnedBullet = false;
    private bool alreadySpawned = false;

    private Animator _anim;

    [SerializeField] private float bulletSpeed = 2;
    [SerializeField] private float radius = 1;
    [SerializeField] private float coolDownTime = 1;

    Vector2 bulletDirection;

    //Sets direction and speed of the bullet
    //Cannon animation does not work
    void Start()
    {
        bulletDirection = (EndPosition.position - StartPosition.position).normalized;
        bulletSpeed *= Time.deltaTime;
        _anim = GetComponent<Animator>();
    }

    //Constantly updating position of the bullet and checks if bullet has reached its max distance
    void Update()
    {
        if (spawnedBullet)
        {
            CreatedBullet.transform.Translate(bulletDirection * bulletSpeed);
            _anim.SetBool("Fire", true);
            if (Vector2.Distance(CreatedBullet.transform.position, EndPosition.position) < radius)
            {
                Destroy(CreatedBullet);
                spawnedBullet = false;
                StartCoroutine(cooldown());
            }
        }
    }

    //On collision, do not spawn another bullet until bullet collides with something or the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlatformerPlayer>() != null && !alreadySpawned)
        {
            spawnedBullet = true;
            alreadySpawned = true;
            CreatedBullet = Instantiate(Bullet, StartPosition.position, Quaternion.identity);
        }
    }

    //Cooldown before next bullet shoots
    private IEnumerator cooldown()
    {
        yield return new WaitForSeconds(coolDownTime);
        spawnedBullet = false;
        alreadySpawned = false;
    }
}
