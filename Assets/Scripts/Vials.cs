using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vials : MonoBehaviour
{
    [SerializeField]
    private PlayerDNALevel playerDNALevel;

   [SerializeField]
    private List<MeshRenderer> ListOfVialMaterials; 
    //vail 0 to 2, 0 = omega , 1 = beta , 2 = alpha

    // those are values used to change the appearence of the dna levels in the vials , the pb is since they are small the values are weird 
    private float minimumDNAVial = 0.525f; 
    private float maximumDNAVial = 0.475f;

    private bool isFull = false;

    private void Awake()
    {
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        playerDNALevel.OnDnaLevelChanged += UpdateVialDNAAmount;
    }

    private void UnsubscribeToEvents()
    {
        playerDNALevel.OnDnaLevelChanged -= UpdateVialDNAAmount;
    }

    public void UpdateVialDNAAmount( float amount )
    {

        if(amount > .33f)
        {
            ListOfVialMaterials[0].material.SetFloat("_FillAmount", maximumDNAVial);

            if (amount > .66f)
            {
                ListOfVialMaterials[1].material.SetFloat("_FillAmount", maximumDNAVial);

                ListOfVialMaterials[2].material.SetFloat("_FillAmount", CalculateVialDnaAmount(amount - .66f));
            }
            else
            {
                ListOfVialMaterials[1].material.SetFloat("_FillAmount", CalculateVialDnaAmount(amount - .33f));

                ListOfVialMaterials[2].material.SetFloat("_FillAmount", minimumDNAVial);
            }
        }
        else
        {
            ListOfVialMaterials[0].material.SetFloat("_FillAmount", CalculateVialDnaAmount(amount));
            ListOfVialMaterials[1].material.SetFloat("_FillAmount", minimumDNAVial);
            ListOfVialMaterials[2].material.SetFloat("_FillAmount", minimumDNAVial);
        }

        if (amount >= .99f)
            ChangeVialColour(true);
        else
            ChangeVialColour(false);
    }

    // amount must be between 0 and .34
    private float CalculateVialDnaAmount(float amount)
    {

        float result = 0;

        //get the amount in a %age
        result = amount * 3;

        //since the difference between max and min is 0.050 we / by 20 and we put it negative cause minimal > maximal
        result = result / (-20);

        //we then add the minimum
        result += minimumDNAVial;


        return result;
    }

    private void ChangeVialColour(bool isFull)
    {
        if (isFull)
        {
            foreach (MeshRenderer mat in ListOfVialMaterials)
            {
                mat.material.SetColor("_Colour", Color.green);
                mat.material.SetColor("_TopColor", Color.green);
             }
        }
        else
        {
            foreach (MeshRenderer mat in ListOfVialMaterials)
            {
                mat.material.SetColor("_Colour", Color.red);
                mat.material.SetColor("_TopColor", Color.red);
        }
        }
       
    }
}
