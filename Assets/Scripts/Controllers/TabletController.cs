using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletController : Interactable
{
    [SerializeField]
    static public List<NomTablet> nomPreuvesRamasse;

    public NomTablet nom = new NomTablet();

    // Start is called before the first frame update
    void Start()
    {
        nomPreuvesRamasse = new List<NomTablet>();
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
        nomPreuvesRamasse.Add(this.nom);
        this.gameObject.SetActive(false);
    }
}
public enum NomTablet
{
    tabletJungle,
    tabletCorail,
    tabletHQ
}
