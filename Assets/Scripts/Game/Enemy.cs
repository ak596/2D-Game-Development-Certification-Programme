﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int enemySpeed = 4;
    [SerializeField]
    private GameObject _laserPfefab;
    private Player _player;
    private Animator _anim;
    private AudioSource _enemyExplosion;
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //starting position
        transform.position = new Vector3(Random.Range(-9.0f, 9.0f), 7, transform.position.z);

        _player = GameObject.Find("Player").GetComponent<Player>();
        
        if(_player == null)
        {
            Debug.LogError("The player is NULL");
        }
        

        _anim = GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.LogError("The animator is NULL");
        }

        _enemyExplosion = GetComponent<AudioSource>();

    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovementg();

        EnemyFireLaser();
    }

    void CalculateMovementg()
    {

        transform.Translate(-Vector3.up * enemySpeed * Time.deltaTime);

        if (transform.position.y <= -5.5f)
        {
            transform.position = new Vector3(Random.Range(-9.0f, 9.0f), 7, transform.position.z);
        }

    }

    //At every 3 to 7 seconds a laser is fired.
    void EnemyFireLaser()
    {
        if (gameObject != null)
        {
            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(3.0f, 7.0f);
                _canFire = Time.time + _fireRate;
                Instantiate(_laserPfefab, transform.position, Quaternion.identity);
            }
        }
      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("We collided");
        if (other.gameObject.tag == "Player")
        {
            //make enemy stop
            enemySpeed = 0;
            //Debug.Log("If is called");
            if (other != null)
            {
                _player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");

            _enemyExplosion.Play();

            //first destroy the collider before the actual destroy 
            Destroy(GetComponent<Collider2D>());

            Destroy(this.gameObject,2.8f);
        }

        else if (other.gameObject.tag == "Laser")
        {
            enemySpeed = 0;
            Destroy(other.gameObject);

            if(_player != null)
            {
                _player.AddScore();
            }

            //calls this trigger when a laser collides with our object and plays the animation
            _anim.SetTrigger("OnEnemyDeath");
            _enemyExplosion.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject,2.8f);
        }

    }

}
