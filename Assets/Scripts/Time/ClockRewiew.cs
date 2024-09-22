using TMPro;
using UnityEngine;

public class ClockReview : MonoBehaviour
{ 
    [SerializeField] protected Transform hourHand;
    [SerializeField] protected Transform minuteHand;
    [SerializeField] protected Transform secondHand;
    [SerializeField] protected TMP_Text digitalClockText;
 
    public Transform HourHand => hourHand;
    public Transform MinuteHand => minuteHand;
    public Transform SecondHand => secondHand;
    public TMP_Text DigitalClockText => digitalClockText;
 
    protected ITimeSyncService timeSyncService;
    protected string previousTimeText;
}
