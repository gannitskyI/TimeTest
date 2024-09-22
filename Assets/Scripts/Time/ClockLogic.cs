using UnityEngine;
using DG.Tweening;
using System;

public class ClockLogic : ClockReview
{
    private void Start()
    {
        timeSyncService = GetComponent<ITimeSyncService>();
        timeSyncService.StartSync(UpdateClock);
    }

    private void UpdateClock(DateTime currentTime)
    {
        UpdateAnalogClock(currentTime);
        UpdateDigitalClock(currentTime);
    }

    private void UpdateAnalogClock(DateTime currentTime)
    {
        int hours = currentTime.Hour;
        int minutes = currentTime.Minute;
        int seconds = currentTime.Second;

        float hourRotation = -hours * 30 - minutes * 0.5f;
        float minuteRotation = -minutes * 6;
        float secondRotation = -seconds * 6;

        hourHand.DORotate(new Vector3(0, 0, hourRotation), 0.5f).SetEase(Ease.OutCubic);
        minuteHand.DORotate(new Vector3(0, 0, minuteRotation), 0.5f).SetEase(Ease.OutCubic);
        secondHand.DORotate(new Vector3(0, 0, secondRotation), 0.5f).SetEase(Ease.OutCubic);
    }

    private void UpdateDigitalClock(DateTime currentTime)
    {
        string newTimeText = currentTime.ToString("HH:mm:ss");

        if (previousTimeText != newTimeText)
        {
            digitalClockText.text = newTimeText;
            previousTimeText = newTimeText;
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) return;

        timeSyncService ??= GetComponent<ITimeSyncService>();
        if (timeSyncService == null)
        {
            Debug.LogError("TimeSyncService not found on the object when restoring focus.");
            return;
        }

        timeSyncService.StartSync(UpdateClock);
    }
}
