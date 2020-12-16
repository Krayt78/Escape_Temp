using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGFXController : MonoBehaviour
{
    [SerializeField] GameObject grapplingPrefab;
    [SerializeField] GameObject dartLauncherPrefab;

    GameObject currentGFX;

    private void Start()
    {
        PlayerAbilitiesController abilitiesController = GetComponentInParent<PlayerAbilitiesController>();
        if(abilitiesController)
            abilitiesController.OnAbilityChanged += AbilityWasChanged;

    }

    private void OnDestroy()
    {
        PlayerAbilitiesController abilitiesController = GetComponentInParent<PlayerAbilitiesController>();
        if (abilitiesController)
            abilitiesController.OnAbilityChanged -= AbilityWasChanged;
    }

    private void AbilityWasChanged(Ability newAbility)
    {
        if(newAbility.GetType() == typeof(VrGrapplinController))
        {
            Destroy(currentGFX);
            currentGFX = Instantiate(grapplingPrefab, transform);
            currentGFX.transform.localPosition = Vector3.zero;
            currentGFX.transform.localRotation = Quaternion.identity;
        }
        else if (newAbility.GetType() == typeof(DartAbilityController))
        {
            Destroy(currentGFX);
            currentGFX = Instantiate(dartLauncherPrefab, transform);
            currentGFX.transform.localPosition = Vector3.zero;
            currentGFX.transform.localRotation = Quaternion.identity;
        }else if (newAbility.GetType() == typeof(DecoyController))
        {
            Destroy(currentGFX);
        }
    }
}
