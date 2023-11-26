using UnityEngine;
using Firebase.Database;
using System.Collections;
using TMPro;
using System;

public class DataBaseManager : MonoBehaviour
{
    public DatabaseReference dbReference;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI conStatus;
    [SerializeField] private TextMeshProUGUI con2Status;

    protected static bool buttonState = false;
    private static bool conWorkStatus = false;
    private static bool con2WorkStatus = false;

    protected static string sysInstance = "system";
    protected static string distInstance = "distanceSensor";
    protected static string dcMotorInstance = "dcMotor";
    protected static string servoInstance = "servo";

    public void ButtonPressed()
    {
        buttonState = !buttonState;
        if (buttonState)
        {
            //dbReference.Child(distInstance).Child("distWorking").SetValueAsync(false);  //To check the status everytime
            buttonText.text = "Tap to OFF";
            dbReference.Child(sysInstance).Child("buttonState").SetValueAsync(true);
        }
        else
        {
            buttonText.text = "Tap to ON";
            dbReference.Child(sysInstance).Child("buttonState").SetValueAsync(false);
        }
    }
    /*public void Display()
    {
        try
        {
            StartCoroutine(GetConWorkData((bool getConStatus) =>
            {
                conWorkStatus = getConStatus;
            }));
        }
        catch { }
        if (buttonState == true && conWorkStatus == true)
        {
            try
            {
                buttonText.text = "Tap to OFF";
                buttonText.color = Color.green;
            }
            catch { }
        }
        else if (buttonState == true && conWorkStatus != true)
        {
            try
            {
                buttonText.text = "MicroController OFF!";
                buttonText.color = Color.red;
            }
            catch { }
        }
        else
        {
            try
            {
                buttonText.text = "Tap to ON";
                buttonText.color = Color.black;
            }
            catch { }
        }
    }*/
    public void Display()
    {
        try
        {
            StartCoroutine(GetConWorkData((bool getConStatus) =>
            {
                conWorkStatus = getConStatus;
            }));
            StartCoroutine(GetCon2WorkData((bool getCon2Status) =>
            {
                con2WorkStatus = getCon2Status;
            }));
        }
        catch { }
        if (buttonState == true && (conWorkStatus == true || con2WorkStatus == true))
        {
            try
            {
                buttonText.text = "Tap to OFF";
                buttonText.color = Color.green;
            }
            catch { }
        }
        else if (buttonState == true && conWorkStatus != true && con2WorkStatus != true)
        {
            try
            {
                buttonText.text = "Please Wait!";
                buttonText.color = Color.blue;
            }
            catch { }
        }
        else
        {
            try
            {
                buttonText.text = "Tap to ON";
                buttonText.color = Color.black;
            }
            catch { }
        }
        try
        {
            if (conWorkStatus == true)
            {
                conStatus.text = "ON";
            }
            else
            {
                conStatus.text = "OFF";
            }
        }
        catch { }
        try
        {
            if (con2WorkStatus == true)
            {
                con2Status.text = "ON";
            }
            else
            {
                con2Status.text = "OFF";
            }
        }
        catch { }
    }
    private IEnumerator GetConWorkData(Action<bool> onCallback)
    {
        var conWorkData = dbReference.Child(sysInstance).Child("controller").GetValueAsync();
        yield return new WaitUntil(predicate: () => conWorkData.IsCompleted);
        if (conWorkData != null)
        {
            DataSnapshot snapshot = conWorkData.Result;
            onCallback.Invoke(bool.Parse(snapshot.Value.ToString()));
        }
    }
    private IEnumerator GetCon2WorkData(Action<bool> onCallback)
    {
        var conWorkData = dbReference.Child(sysInstance).Child("controller2").GetValueAsync();
        yield return new WaitUntil(predicate: () => conWorkData.IsCompleted);
        if (conWorkData != null)
        {
            DataSnapshot snapshot = conWorkData.Result;
            onCallback.Invoke(bool.Parse(snapshot.Value.ToString()));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        dbReference.Child(sysInstance).Child("controller").SetValueAsync(false);
        dbReference.Child(sysInstance).Child("controller2").SetValueAsync(false);
        dbReference.Child(distInstance).Child("distWorking").SetValueAsync(false);
        dbReference.Child(dcMotorInstance).Child("dcMotorWorking").SetValueAsync(false);
        dbReference.Child(servoInstance).Child("servoWorking").SetValueAsync(false);
    }

    // Update is called once per frame
    void Update()
    {
        Display();
    }

    // OnDestroy is called just before stopping the script.
    void OnDestroy()
    {
        dbReference.Child(sysInstance).Child("buttonState").SetValueAsync(false);
    }
}
