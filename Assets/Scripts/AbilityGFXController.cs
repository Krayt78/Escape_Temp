using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGFXController : MonoBehaviour
{
    [SerializeField] GameObject grapplingPrefab;
    [SerializeField] GameObject dartLauncherPrefab;
    [SerializeField] GameObject decoyLauncherPrefab;

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
            changeAbilityGfx(grapplingPrefab);
        }
        else if (newAbility.GetType() == typeof(DartAbilityController))
        {
            changeAbilityGfx(dartLauncherPrefab);
        }else if (newAbility.GetType() == typeof(DecoyController))
        {
            changeAbilityGfx(decoyLauncherPrefab);
        }
    }

    private void changeAbilityGfx(GameObject abilityGfx)
    {
        Destroy(currentGFX);
        currentGFX = Instantiate(abilityGfx, transform);
        currentGFX.transform.localPosition = Vector3.zero;
        currentGFX.transform.localRotation = Quaternion.identity;
    }
}
