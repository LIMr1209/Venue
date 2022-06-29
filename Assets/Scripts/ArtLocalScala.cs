using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtLocalScala : MonoBehaviour
{
    Transform art;
    Transform artKuang;

    void Start()
    {
       for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("frames"))
            {
                artKuang = transform.GetChild(i);
            }
            if (transform.GetChild(i).name.Contains("paintings"))
            {
                art = transform.GetChild(i);
            }
        }
        OnSetLocalScala(10, 9);

    }

    private void OnSetLocalScala(float length, float width)
    {
        Debug.Log(art.localScale);
        Debug.Log(artKuang.localScale);

    }
}
