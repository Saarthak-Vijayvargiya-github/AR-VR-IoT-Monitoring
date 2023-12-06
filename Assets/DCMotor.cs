using UnityEngine;
using Firebase.Database;
using System.Collections;
using TMPro;
using System;

public class DCMotor : DataBaseManager
{
    
    [SerializeField] private TextMeshProUGUI dutyCycle1;               // Display duty value
    [SerializeField] private TextMeshProUGUI dcMotorStatus;            // Display on status panel
    private static bool dcmWorkStat = false;
    private static string GraphUrl = "https://thingspeak.com/channels/2266759/charts/1?bgcolor=%23000000&color=%23fff222&dynamic=true&max=100&min=0&results=10&title=Motor+Database&type=line";

    // Getting the required data from Firebase
    private IEnumerator GetDCMotorData(Action<int> onCallback)
    {
        var dcMotorData = dbReference.Child(dcMotorInstance).Child("dutyCycle").GetValueAsync();
        yield return new WaitUntil(predicate: () => dcMotorData.IsCompleted);
        if (dcMotorData != null)
        {
            DataSnapshot snapshot = dcMotorData.Result;
            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }
    private IEnumerator GetDCMWorkData(Action<bool> onCallback)
    {
        var dcmWorkData = dbReference.Child(dcMotorInstance).Child("dcMotorWorking").GetValueAsync();
        yield return new WaitUntil(predicate: () => dcmWorkData.IsCompleted);
        if (dcmWorkData != null)
        {
            DataSnapshot snapshot = dcmWorkData.Result;
            onCallback.Invoke(bool.Parse(snapshot.Value.ToString()));
        }
    }

    // Opens ThingSpeak website in any web browser 
    public void OpenWebsite()
    {
        Application.OpenURL(GraphUrl);
    }

    // Manages Potentiometer/ DCMotor display variables
    public void DCMotorDisplay()
    {
        try
        {
            StartCoroutine(GetDCMWorkData((bool getdcmStat) =>
            {
                dcmWorkStat = getdcmStat;
            }));
        }
        catch { }
        if (buttonState == true && dcmWorkStat == true)
        {
            StartCoroutine(GetDCMotorData((int duty) =>
            {
                try
                {
                    dutyCycle1.text = duty.ToString() + " %";
                    dcMotorStatus.text = "Running";
                    dcMotorStatus.color = Color.white;
                    if (duty < 80)
                    {
                        dutyCycle1.color = Color.green;
                    }
                    else
                    {
                        dutyCycle1.color = Color.yellow;
                    }
                }
                catch { }
            }));
        }
        else if (buttonState == true && dcmWorkStat != true)
        {
            try
            {
                dutyCycle1.text = "0 %";
                dutyCycle1.color = Color.red;
                dcMotorStatus.text = "OFF";
                dcMotorStatus.color = Color.red;
            }
            catch { }
        }
        else
        {
            try
            {
                dutyCycle1.text = "OFF";
                dcMotorStatus.text = "OFF";
                dutyCycle1.color = Color.white;
                dcMotorStatus.color = Color.white;
            }
            catch { }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DCMotorDisplay();
    }
}
