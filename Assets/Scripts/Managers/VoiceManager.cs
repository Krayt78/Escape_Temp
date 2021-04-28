using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    private static VoiceManager instance = null;
    public static VoiceManager Instance { get { return instance; } }

    [SerializeField] private Transform playerTransform;

    private List<VoiceEvent> voiceQueue;

    private bool isPlayingVoice;

    private VoiceEvent lastVoiceEvent;

    public enum Priority
    {
        High,
        Medium,
        Low
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
        voiceQueue = new List<VoiceEvent>();
        isPlayingVoice = false;
    }

    public float PlayVoiceImmediate(VoiceEvent voiceEvent, bool deleteQueue = true)
    {
        if (deleteQueue)
            voiceQueue.Clear();

        voiceQueue.Insert(0, voiceEvent);
        return playVoice(voiceEvent);
    }

    public void AddVoiceToQueue(VoiceEvent newVoiceEvent)
    {
        // Si aucun son est joué actuellement on joue le nouveau son.
        if (!isPlayingVoice)
        {
            playVoice(newVoiceEvent);
        }
        // On ignore les sons a basse priorité, il sont joué tout de suite sinon rien
        else if (newVoiceEvent.EventPriority != Priority.Low)
        {
            if (isPriority(newVoiceEvent))
            {
                playVoice(newVoiceEvent);
            }
            else
            {
                voiceQueue.Add(newVoiceEvent);
                sortQueueByPriority();
            }
        }
    }

    private float playVoice(VoiceEvent voice)
    {
        isPlayingVoice = true;
        lastVoiceEvent = voice;
        float lengthSound = FMODPlayerController.PlayVoice(voice.IdFmod, playerTransform.position);
        voiceQueue.Remove(voice);
        Invoke("playNextSound", lengthSound + 1);
        return lengthSound + 1;
    }

    private void playNextSound()
    {
        isPlayingVoice = false;
        if (voiceQueue.Count > 0)
        {
            playVoice(voiceQueue[0]);
        }
    }

    private void sortQueueByPriority()
    {
        voiceQueue = voiceQueue.OrderBy(x => (int) x.EventPriority).ToList();
    }

    // Retourne true si le VoiceEvent passé en paramètre est prioritaire a celui joué actuellement
    private bool isPriority(VoiceEvent voice)
    {
        return voice.EventPriority < lastVoiceEvent.EventPriority;
    }
}
