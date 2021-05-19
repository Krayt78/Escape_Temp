using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private static CheckpointManager instance = null;
    public static CheckpointManager Instance { get { return instance; } }

    private Transform lastCheckpoint;

    [SerializeField] private Transform firstCheckpoint;
    [SerializeField] private Transform playerTransform;

    private bool transformIsReseting;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
        transformIsReseting = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        lastCheckpoint = firstCheckpoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m") && !transformIsReseting)
        {
            transformIsReseting = true;
            Time.timeScale = 0;
            GoToLastCheckpoint();
        }
    }

    public void GoToLastCheckpoint()
    {
        if(lastCheckpoint != null && playerTransform != null)
        {
            playerTransform.position = lastCheckpoint.position;
            playerTransform.rotation = lastCheckpoint.rotation;
            GameController.Instance.ResumeGame();
            // Pour le menu game over
            UIManager.Instance.HideAllUi();
            GameController.Instance.revivePlayer();
            transformIsReseting = false;
        }
    }

    public void setCheckPoint(Transform newCheckPoint)
    {
        lastCheckpoint = newCheckPoint;
    }
}
