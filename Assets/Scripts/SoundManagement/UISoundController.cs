using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundController : MonoBehaviour
{
    [SerializeField] string HoverEvent, SelectEvent;

    public void PlayerHover()
    {
        FMODPlayerController.PlayOnShotSound(HoverEvent, transform.position);
    }

    public void PlayerSelect()
    {
        FMODPlayerController.PlayOnShotSound(SelectEvent, transform.position);
    }
}
