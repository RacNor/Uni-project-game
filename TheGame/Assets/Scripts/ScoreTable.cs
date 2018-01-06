using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ScoreTable:MonoBehaviour
{
    public void Awake()
    {
        //ScoreController.instance.table = this;
    }
    public void DestroyMyself()
    {
        Destroy(this.gameObject);
    }
}

