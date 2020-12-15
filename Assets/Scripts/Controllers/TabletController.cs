using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletController : Interactable
{
    [SerializeField]
    static public List<NomTablet> nomPreuvesRamasse;
    [SerializeField]
    GameObject highlightVfx;

    public NomTablet nom = new NomTablet();

    [SerializeField] string tabletPickUpSoundPath;

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

    public void onGrab()
    {
        highlightVfx.SetActive(false);
    }

    public void grabTablet()
    {
        nomPreuvesRamasse.Add(this.nom);
        this.gameObject.SetActive(false);
        FMODPlayerController.PlayOnShotSound(tabletPickUpSoundPath, transform.position);
        EventManager.Instance.Raise(new OnTabletGrabEvent() { nom = this.nom });
    }
}
public enum NomTablet
{
    tabletJungle,
    tabletCorail,
    tabletHQ
}
