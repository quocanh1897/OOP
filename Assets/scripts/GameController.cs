﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {
   [SerializeField]
    private InputField input;
    private string inputStringFromPlayer;
    //khoi tao mot mang cac doi tuong jet
    private bool flatstart;
    private Text show_totalpoint;
    private Vector3 pos, oldPos;
    private bool isendgames;
    private bool iswingames;
    private bool isstartgames;
    private float time_start_game;
    private GameObject Countdown;
    public GameObject panelstart;
    public GameObject panelend;
    public GameObject panelwin;
    public GameObject[] jet;
    public GameObject player;
    public GameObject countdown;
    public GameObject randomjet;
    void Start()
    {
        input.ActivateInputField();//dau nhap input duoc active ngay sau start game
        Time.timeScale = 0;
        oldPos = new Vector3(0, 0, 0);
        pos = oldPos;
        input = Object.FindObjectOfType<InputField>();
        isendgames = false;
        iswingames = false;
        isstartgames = true;
        player.GetComponent<tanka>().point = 0f;
    }
    void Update()
    {
        input.ActivateInputField();//sau moi lan destroy enemy thi active inputField
        jet = GameObject.FindGameObjectsWithTag("jet");//lien tuc cap nhat danh sach cac jet
        foreach (GameObject jetInList in jet)//xet tung jet trong list da cap nhat
        {
            string t = jetInList.GetComponentInChildren<textHolder>().keyOfJet.text;//lay keyOfJet tao ra tu textHolder cua moi jet
            if(input.text==t)//truong hop input dung voi keyOfJet
            {
                input.text = null;
                if (pos == oldPos)//de cho doi tuong Explose k bi di chuyen theo jet
                    pos = jetInList.GetComponent<Transform>().position;
                jetInList.transform.Translate(pos);
                jetInList.GetComponent<Rigidbody2D>().isKinematic = false;
                jetExplose(jetInList, pos);//tao doi tuong Explose tu vi tri pos
                pos = oldPos;
                jetInList.GetComponent<jetFall>().getPointforplayer();
                jetInList.GetComponent<jetFall>().getManaforplayer();
                jetInList.GetComponent<jetFall>().getshot();
            }
        }
        if(Countdown!=null&&isstartgames)
        {
            if(Time.time>time_start_game+4f)
            {
                Destroy(Countdown, 0.01f);isstartgames = false;randomjet.SetActive(true);
            }
        }
    }
    public void GetInput(string typeString)//player nhap text tu day
    {
        inputStringFromPlayer = typeString;
        input.text = null;
    }
    void jetExplose(GameObject jetInput, Vector3 positionOfExplosion)//khoi tao doi tuong no jet
    {
        GameObject explose = jetInput.GetComponent<jetFall>().explose;
        Vector3 transPos = positionOfExplosion;
        GameObject explosion = Instantiate(explose, transPos, Quaternion.identity);
        Text textKeyJet = jetInput.GetComponentInChildren<textHolder>().keyOfJet;
        Destroy(jetInput, 0.2f);
        Destroy(textKeyJet, 0f);
        Destroy(explosion, 1f);
    }
    public void EndGames()
    {
        Time.timeScale = 0;
        isendgames = true;
        panelend.SetActive(true);
        Text[] arr_Text = Text.FindObjectsOfType<Text>();
        foreach (Text txt in arr_Text)
        {
                if (txt.tag == "totalpoint")
                {
                    txt.text = "Your Point: " + player.GetComponent<tanka>().getnowpoint();
                    break;
                }
        }
    }
    public void WinGame()
    {
        Time.timeScale = 0;
        iswingames = true;
        panelwin.SetActive(true);
        Text[] arr_Text = Text.FindObjectsOfType<Text>();
        foreach (Text txt in arr_Text)
           {
                if (txt.tag == "Textshowwin")
                {
                    txt.text = "Your Point: " + player.GetComponent<tanka>().getnowpoint();
                    break;
                }
           }
    }
    public void StartGame()
    {
        panelstart.SetActive(false);
        player.GetComponent<tanka>().point = 0f;
        Time.timeScale = 1;
        count_down();
    }
    public void RestartGame()
    {
       panelend.SetActive(false);
       SceneManager.LoadScene(1);
       player.GetComponent<tanka>().point = 0f;
    }
    public void Startnewgame()
    {
        panelwin.SetActive(false);
        SceneManager.LoadScene(1);
        player.GetComponent<tanka>().point = 0f;
    }
    void count_down()
    {
         Countdown = Instantiate(countdown, new Vector3(0, 2.2f, 0), Quaternion.identity);
         time_start_game = Time.time;
    }
}