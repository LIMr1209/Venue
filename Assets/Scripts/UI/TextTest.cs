using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTest : MonoBehaviour
{
    //public TMP_FontAsset fontb;
    void Start()
    {

        transform.GetComponent<TextMeshProUGUI>().font = Resources.Load("PingFangSCA 1") as TMP_FontAsset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
