using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPanel : UIbase
{
    

    void Start()
    {
        FindChild(this.gameObject);
        for (int i = 0; i < aaa.Count; i++)
        {
            aaa[i].gameObject.AddComponent<TextTest>();
        }
    }

    void Update()
    {
        
    }

}
