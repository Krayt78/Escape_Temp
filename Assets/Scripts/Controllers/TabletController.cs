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

    [SerializeField] private string tabletPickUpSoundPath;
    [SerializeField] private string tabletVoicePickUpPath;

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
        EventManager.Instance.Raise(new OnTabletGrabEvent() { nom = this.nom });
        FMODPlayerController.PlayOnShotSound(tabletPickUpSoundPath, transform.position);

        if (tabletVoicePickUpPath != null)
        {
            VoiceEvent pickUpProof1 = new VoiceEvent(tabletVoicePickUpPath, VoiceManager.Priority.High);
            VoiceManager.Instance.AddVoiceToQueue(pickUpProof1);
        }
        this.gameObject.SetActive(false);
    }
}
public enum NomTablet
{
    tabletJungle,
    tabletCorail,
    tabletHQ
}
