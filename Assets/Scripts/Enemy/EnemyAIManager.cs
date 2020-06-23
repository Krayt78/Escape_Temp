using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    private static EnemyAIManager instance;
    public static EnemyAIManager Instance { get { return instance; } }

    [Range(0f, 1f)]
    private float globalAlertLevel = 0f;
    public float GlobalAlertLevel { get { return globalAlertLevel; } private set { globalAlertLevel = value; } }

    private float currentTimer = 0f;

    private List<Guard> EnemiesOnAlert = new List<Guard>();
    private List<Guard> EnemiesOnSight = new List<Guard>();

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        this.GlobalAlertLevel = 0f;
    }

    private void Start(){
        StartCoroutine("CheckGlobalAlert", 0.15f);
    }

    IEnumerator CheckGlobalAlert(float delay){
        while(true){
            yield return new WaitForSeconds(delay);
            AlertLevel();
        }
    }

    public void SetGlobalAlertLevel(float value)
    {
        this.GlobalAlertLevel = Mathf.Clamp(value, 0, 1);
    }

    public void AddEnemyOnSight(Guard enemy){
        Debug.Log("ADD ENEMY ON SIGHT");
        if(!EnemiesOnSight.Contains(enemy)) EnemiesOnSight.Add(enemy);
        Debug.Log("EnemyOnSightCount : "+EnemiesOnSight.Count);
    }

    public void AddEnemyOnAlert(Guard enemy){
        if(!EnemiesOnAlert.Contains(enemy)) EnemiesOnAlert.Add(enemy);
    }

    public void RemoveEnemyOnSight(Guard enemy){
        Debug.Log("REMOVE ENEMY ON SIGHT");
        if(EnemiesOnSight.Contains(enemy)) EnemiesOnSight.Remove(enemy);
    }

    public void RemoveEnemyOnAlert(Guard enemy){
        if(EnemiesOnAlert.Contains(enemy)) EnemiesOnAlert.Remove(enemy);
        if(EnemiesOnSight.Count == 0 && EnemiesOnAlert.Count == 0) SetGlobalAlertLevel(GlobalAlertLevel -0.5f);
    }

    public bool HasEnemyAlerted(){
        return EnemiesOnAlert.Count > 0;
    }

    public bool HasEnemySighted(){
        return EnemiesOnSight.Count > 0;
    }

    public bool HasCurrentEnemyAlerted(Guard enemy){
        return EnemiesOnAlert.Exists(element => element.Equals(enemy));
    }

    public void ClearEnemiesOnSight(){
        this.EnemiesOnSight.Clear();
    }

    private void AlertLevel(){
        if(HasEnemySighted())
        {
            Debug.Log("EnemiesSighted : "+EnemiesOnSight.Count);
            SetGlobalAlertLevel(GlobalAlertLevel + 0.02f * EnemiesOnSight.Count);
        }
        else
        {
            currentTimer += 0.1f;
            if(currentTimer >= 5f){
                SetGlobalAlertLevel(GlobalAlertLevel - 0.01f);
            }
        }
        
        if(!HasEnemyAlerted() && !HasEnemySighted())
        {
            SetGlobalAlertLevel(0);
        }
        Debug.Log("GLOBAL ALERT : "+GlobalAlertLevel);
    }
}
