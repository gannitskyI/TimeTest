using System;

public interface ITimeSyncService
{
    void StartSync(Action<DateTime> onTimeUpdated);
}
