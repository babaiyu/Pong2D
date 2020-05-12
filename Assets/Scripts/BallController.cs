using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public int force;
    Rigidbody2D rigid;

    int scoreP1;
    int scoreP2;

    Text scoreUIP1;
    Text scoreUIP2;

    GameObject panelOver;
    Text txtWinner;

    new AudioSource audio;
    public AudioClip hitSound;
    public AudioClip loseSound;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(2, 0).normalized;
        rigid.AddForce(direction * force);
        scoreP1 = 0;
        scoreP2 = 0;
        scoreUIP1 = GameObject.Find("Score1").GetComponent<Text>();
        scoreUIP2 = GameObject.Find("Score2").GetComponent<Text>();
        panelOver = GameObject.Find("PanelOver");
        panelOver.SetActive(false);
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetBall()
    {
        transform.localPosition = new Vector2(0, 0);
        rigid.velocity = new Vector2(0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audio.PlayOneShot(hitSound);
        if (collision.gameObject.name == "BorderRight") PhysicBorder(2, 0);
        if (collision.gameObject.name == "BorderLeft") PhysicBorder(-2, 0);
        if (collision.gameObject.name == "Paddle1" || collision.gameObject.name == "Paddle2") PhysicPaddle(collision);
    }

    void PhysicBorder (int a, int b)
    {
        if (a > 0)
        {
            scoreP1 += 1;
            if (scoreP1 == 5) ChooseWinner("Player 1");
        } else
        {
            scoreP2 += 1;
            if (scoreP2 == 5) ChooseWinner("Player 2");
        }
        ShowScore();
        ResetBall();
        Vector2 direction = new Vector2(a, b).normalized;
        rigid.AddForce(direction * force);
    }

    void PhysicPaddle(Collision2D coll)
    {
        float corner = (transform.position.y - coll.transform.position.y) * 5f;
        Vector2 direction = new Vector2(rigid.velocity.x, corner).normalized;
        rigid.velocity = new Vector2(0, 0);
        rigid.AddForce(direction * force * 2);
    }

    void ShowScore()
    {
        scoreUIP1.text = scoreP1 + "";
        scoreUIP2.text = scoreP2 + "";
    }

    void ChooseWinner(string winner)
    {
        panelOver.SetActive(true);
        txtWinner = GameObject.Find("Winner").GetComponent<Text>();
        txtWinner.text = winner+ " is Winner!" ;
        audio.PlayOneShot(loseSound);
        System.Threading.Thread.Sleep(1000);
        Destroy(gameObject);
        return;
    }
}
