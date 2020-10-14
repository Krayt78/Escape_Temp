using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletController : Interactable
{
    int nbPreuve;

    public int NbPreuve { get => nbPreuve; set => nbPreuve = value; }


    // Start is called before the first frame update
    void Start()
    {
        NbPreuve = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use(GameObject user)
    {
    }

    public void grabTablet()
    {
        Destroy(gameObject);
        NbPreuve++;
    }
}
