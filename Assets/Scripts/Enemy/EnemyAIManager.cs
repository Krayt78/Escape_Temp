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
    public int onAttack = 0;

    private List<Guard> EnemiesOnAlert = new List<Guard>();
    private List<Guard> EnemiesOnSight = new List<Guard>();

    private void OnGUI()
    {
            string printString = "Global Level Alert : " + this.globalAlertLevel;
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 20;
            GUI.Label(new Rect(450, 50, 300, 500), printString, myStyle);
    }

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

    private void Start()
    {
        StartCoroutine("CheckGlobalAlert", 0.2f);
    }

    IEnumerator CheckGlobalAlert(float delay)
    {
        while(true){
            yield return new WaitForSeconds(delay);
            AlertLevel();
        }
    }

    public void SetGlobalAlertLevel(float value)
    {
        this.GlobalAlertLevel = Mathf.Clamp(value, 0f, 1f);
    }

    public void AddEnemyOnSight(Guard enemy)
    {
        if(EnemiesOnSight.Count == 0) currentTimer = 0;
        if(!EnemiesOnSight.Exists(element => element.Equals(enemy)))
        {
            EnemiesOnSight.Add(enemy);
            SetGlobalAlertLevel(GlobalAlertLevel + 0.1f);
        }
    }

    public void AddEnemyOnAlert(Guard enemy)
    {
        if(!EnemiesOnAlert.Exists(element => element.Equals(enemy)))
        {
            EnemiesOnAlert.Add(enemy);
        } 
    }

    public void RemoveEnemyOnSight(Guard enemy)
    {
        if(EnemiesOnSight.Exists(element => element.Equals(enemy)))
        {
            EnemiesOnSight.Remove(enemy);
        }
    }

    public void RemoveEnemyOnAlert(Guard enemy)
    {
        if(EnemiesOnAlert.Exists(element => element.Equals(enemy)))
        {
            EnemiesOnAlert.Remove(enemy);
        } 
        if(EnemiesOnSight.Count == 0 && EnemiesOnAlert.Count == 0) SetGlobalAlertLevel(GlobalAlertLevel-0.5f);
    }

    public bool HasEnemyAlerted()
    {
        return EnemiesOnAlert.Count > 0;
    }

    public bool HasEnemySighted()
    {
        return EnemiesOnSight.Count > 0;
    }

    public bool HasCurrentEnemyAlerted(Guard enemy)
    {
        return EnemiesOnAlert.Exists(element => element.Equals(enemy));
    }

    public void ClearEnemiesOnSight()
    {
        this.EnemiesOnSight.Clear();
    }

    public bool HasOnlyOneEnemyOnSight()
    {
        return this.EnemiesOnSight.Count == 1;
    }

    private void AlertLevel()
    {
      //  Debug.Log("ALERTLEVEL HASENEMY ONSIGHT : "+HasEnemySighted());
        if(HasEnemySighted())
        {
            SetGlobalAlertLevel(GlobalAlertLevel + (0.01f * EnemiesOnSight.Count / 3));
        }
        else
        {
            currentTimer += 0.2f;
            if(currentTimer > 8f){
                SetGlobalAlertLevel(GlobalAlertLevel - 0.01f);
            }
        }
        
      //  Debug.Log("GLOBAL ALERT : "+GlobalAlertLevel);
    }
}
