using UnityEngine;
using static VoiceManager;

public class VoiceEvent : MonoBehaviour
{
    [SerializeField] private string idFmod;
    [SerializeField] private Priority eventPriority;

    private bool alreadyPlayed;

    public VoiceEvent(string idFmod, Priority eventPriority)
    {
        this.idFmod = idFmod;
        this.eventPriority = eventPriority;
    }

    public string IdFmod { get => idFmod; set => idFmod = value; }
    public Priority EventPriority { get => eventPriority; set => eventPriority = value; }

    // Start is called before the first frame update
    void Start()
    {
        alreadyPlayed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !alreadyPlayed)
        {
            alreadyPlayed = true;
            PlayVoice();
            Destroy(this.gameObject);
        }
    }

    public void PlayVoice()
    {
        VoiceManager.Instance.AddVoiceToQueue(this);
    }
}
