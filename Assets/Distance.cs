using UnityEngine;
using Firebase.Database;
using System.Collections;
using TMPro;
using System;

public class Distance : DataBaseManager
{
    [SerializeField] private TextMeshProUGUI distance1;               // Display distance value
    [SerializeField] private TextMeshProUGUI distStatus;              // Display on status panel
    private static bool distWorkStat = false;

    private IEnumerator GetDistanceData(Action<int> onCallback)
    {
        var distanceData = dbReference.Child(distInstance).Child("distance").GetValueAsync();
        yield return new WaitUntil(predicate: () => distanceData.IsCompleted);
        if (distanceData != null)
        {
            DataSnapshot snapshot = distanceData.Result;
            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }
    private IEnumerator GetDistWorkData(Action<bool> onCallback)
    {
        var distWorkData = dbReference.Child(distInstance).Child("distWorking").GetValueAsync();
        yield return new WaitUntil(predicate: () => distWorkData.IsCompleted);
        if (distWorkData != null)
        {
            DataSnapshot snapshot = distWorkData.Result;
            onCallback.Invoke(bool.Parse(snapshot.Value.ToString()));
        }
    }
    public void DistSensorDisplay()
    {
        try
        {
            StartCoroutine(GetDistWorkData((bool getdistStat) =>
            {
                distWorkStat = getdistStat;
            }));
        }
        catch { }
        if (buttonState == true && distWorkStat == true)
        {
            StartCoroutine(GetDistanceData((int dist) =>
            {
                try
                {
                    distance1.color = Color.white;
                    distance1.text = "Distance: " + dist.ToString();
                    distStatus.text = "Running";
                    distStatus.color = Color.white;
                    if (dist < 20)
                    {
                        distance1.color = Color.red;
                    }
                    else if (dist < 300)
                    {
                        distance1.color = Color.yellow;
                    }
                }
                catch { }
            }));
        }
        else if (buttonState == true && distWorkStat != true)
        {
            try
            {
                distance1.text = "Input Error";
                distance1.color = Color.magenta;
                distStatus.text = "Error";
                distStatus.color = Color.red;
            }
            catch { }
        }
        else
        {
            try
            {
                distance1.text = "OFF";
                distStatus.text = "OFF";
                distance1.color = Color.white;
                distStatus.color = Color.white;
            }
            catch { }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DistSensorDisplay();
    }
}
