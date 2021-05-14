using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGFXController : MonoBehaviour
{
    /*
    [SerializeField] GameObject grapplingPrefab;
    [SerializeField] GameObject dartLauncherPrefab;
    [SerializeField] GameObject decoyLauncherPrefab;*/

    [SerializeField] GameObject grappling;
    [SerializeField] GameObject dartLaunche;
    [SerializeField] GameObject decoyLauncher;

    //GameObject currentGFX;

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
        grappling.SetActive(false);
        dartLaunche.SetActive(false);
        decoyLauncher.SetActive(false);

        if(newAbility.GetType() == typeof(VrGrapplinController))
        {
            //changeAbilityGfx(grapplingPrefab);
            grappling.SetActive(true);
        }
        else if (newAbility.GetType() == typeof(DartAbilityController))
        {
            // changeAbilityGfx(dartLauncherPrefab);
            dartLaunche.SetActive(true);
        } else if (newAbility.GetType() == typeof(DecoyController))
        {
            // changeAbilityGfx(decoyLauncherPrefab);
            decoyLauncher.SetActive(true);
        }
    }
    /*
    private void changeAbilityGfx(GameObject abilityGfx)
    {
        Destroy(currentGFX);
        currentGFX = Instantiate(abilityGfx, abilityGfx.transform.position, abilityGfx.transform.rotation, transform);
        currentGFX.transform.localPosition = Vector3.zero;
        currentGFX.transform.localRotation = Quaternion.identity;
    }*/
}
