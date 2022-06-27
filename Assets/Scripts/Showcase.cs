using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showcase : MonoBehaviour
{
    public Transform Player;
    public Transform TiRoot;
    public GameObject Ti;


    void Start()
    {
        Player = GameObject.Find("PlayerArmature(Clone)").transform;
        Ti = Instantiate(Resources.Load("Ti") as GameObject);
        TiRoot = GameObject.Find("Canvas").transform.Find("TiRoot");
        Ti.transform.SetParent(TiRoot);
        Ti.transform.localPosition = Vector3.zero;
        Ti.SetActive(false);
    }


    void Update()
    {
        if (Vector3.Distance(transform.position, Player.position) < 2.5)
        {
            Ti.SetActive(true);
        }
        else
        {
            Ti.SetActive(false);
        }
    }
}
