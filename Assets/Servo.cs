using UnityEngine;
using Firebase.Database;
using System.Collections;
using TMPro;
using System;

public class Servo : DataBaseManager
{
    [SerializeField] private TextMeshProUGUI servoAngle1 = null;               // Display duty value
    [SerializeField] private TextMeshProUGUI servoStatus;               // Display on status panel
    private static bool servoWorkStat = false;
    private static int servoAngle = 0;

    // Sending Angle data to Firebase
    public void SetServoData(float angle)
    {
        servoAngle = (int)angle;
        servoAngle1.text = angle.ToString();
        try
        {
            dbReference.Child(servoInstance).Child("angle").SetRawJsonValueAsync(servoAngle.ToString());
        }
        catch (Exception ex)
        {
            print(ex.ToString());
        }
    }

    // Getting the required data from Firebase
    private IEnumerator GetServoWorkData(Action<bool> onCallback)       // Gets working status
    {
        var servoWorkData = dbReference.Child(servoInstance).Child("servoWorking").GetValueAsync();
        yield return new WaitUntil(predicate: () => servoWorkData.IsCompleted);
        if (servoWorkData != null)
        {
            DataSnapshot snapshot = servoWorkData.Result;
            onCallback.Invoke(bool.Parse(snapshot.Value.ToString()));
        }
    }

    // Manages Servo display variables
    public void ServoDisplay()
    {
        try
        {
            StartCoroutine(GetServoWorkData((bool getServoStat) =>
            {
                servoWorkStat = getServoStat;
            }));
        }
        catch { }
        if (buttonState == true && servoWorkStat == true)
        {
            try
            {
                servoStatus.text = "Running";
                servoStatus.color = Color.white;
            }
            catch { }
        }
        else if (buttonState == true && servoWorkStat == true)
        {
            try
            {
                servoStatus.text = "OFF";
                servoStatus.color = Color.red;
            }
            catch { }
        }
        else
        {
            try
            {
                servoStatus.text = "OFF";
                servoStatus.color = Color.white;
            }
            catch { }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ServoDisplay();
    }
}
