using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushController : MonoBehaviour
{
    [SerializeField] string bushCollisionPath;

    MovementProvider playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerMovement==null)
        {
            playerMovement = other.GetComponentInParent<PlayerEntityController>().GetComponentInChildren<MovementProvider>();
            playerMovement.OnMovement += PlayerMove;

            FMODPlayerController.PlayOnShotSound(bushCollisionPath, other.transform.position);
        }
        else if(!other.gameObject.CompareTag("Player"))
            FMODPlayerController.PlayOnShotSound(bushCollisionPath, other.transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerMovement!=null)
        {
            playerMovement.OnMovement -= PlayerMove;
            playerMovement = null;
        }
    }

    private void PlayerMove(float movement)
    {
        if (movement>.01f)
        {
            FMODPlayerController.PlayOnShotSound(bushCollisionPath, playerMovement.transform.position);
        }
    }
}
