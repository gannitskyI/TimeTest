using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class WebTimeSyncService : MonoBehaviour, ITimeSyncService
{
    private float timeDifference;
    private Action<DateTime> onTimeUpdated;

    [Serializable]
    private class TimeData
    {
        public long time;
    }

    public void StartSync(Action<DateTime> onTimeUpdated)
    {
        this.onTimeUpdated = onTimeUpdated;
        StartCoroutine(GetServerTime());
    }

    private IEnumerator GetServerTime()
    {
        //UnityWebRequest request = UnityWebRequest.Get("https://yandex.com/time/sync.json");
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/getTime");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error getting server time: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            TimeData timeData = JsonUtility.FromJson<TimeData>(json);

            if (timeData != null)
            {
                long unixTime = timeData.time;
                DateTime serverTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime;
                timeDifference = (float)(serverTime - DateTime.UtcNow).TotalSeconds;

                InvokeRepeating("UpdateServerTime", 1f, 1f);
            }
            else
            {
                Debug.LogError("Failed to parse JSON.");
            }
        }
    }

    private void UpdateServerTime()
    {
        DateTime currentTime = DateTime.UtcNow.AddSeconds(timeDifference);
        onTimeUpdated?.Invoke(currentTime);
    }
}
