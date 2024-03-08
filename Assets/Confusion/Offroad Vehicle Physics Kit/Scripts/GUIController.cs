using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIController : MonoBehaviour {
    private Transform vehicle;
    private Transform oldVehicle;

    public GameObject rpmNeedle;
    public Text unitUI;
    public Text speedUI;
    public Text gearUI;

    public float maxRpm = 10000.0f;
    public float maxRpmAngle = 315.0f;
    public float zeroRpmAngle = 225.0f;

    public bool isMetric = true;

    private int speed;
    private int gear;
    private string gearText;
    private float rpm;
    private float targetRpm;

    public GameObject transaxleButtonPrefab;
    public GameObject canvas;
    public GameObject[] transaxleButtons;

    private string[] gears = {
        "R","N","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20"
    };

    void Start()
    {
        vehicle = Camera.main.GetComponent<CameraDefault>().TargetLookAt;
        InitButtons();
    }

    void Update()
    {
        if(oldVehicle != vehicle)
        {
            InitButtons();
        }
        oldVehicle = vehicle;

        vehicle = Camera.main.GetComponent<CameraDefault>().TargetLookAt;
        SetMeter();

        SetSpeed(vehicle.GetComponent<CarController>().speed*3); //for some reason Unity3D's idea of speed seems a bit too slow (?)
        SetGear(vehicle.GetComponent<CarController>().transmission.gear);

    }

    public void InitButtons()
    {
        foreach(GameObject transaxleButton in transaxleButtons)
        {
            Destroy(transaxleButton);
        }

        transaxleButtons = new GameObject[vehicle.GetComponent<CarController>().axleInfos.Count];
        for (int i = 0; i < vehicle.GetComponent<CarController>().axleInfos.Count; i++)
        {
            int counter = i;
            GameObject instantiated = Instantiate(transaxleButtonPrefab) as GameObject;
            instantiated.transform.SetParent(canvas.transform);
            transaxleButtons[i] = instantiated;
            transaxleButtons[i].transform.position = new Vector2(100, Screen.height - i * 50 - 200);
            transaxleButtons[i].GetComponent<Button>().onClick.AddListener(() => { ToggleAxle(counter); });

            if (vehicle.GetComponent<CarController>().axleInfos[counter].motor)
            {
                transaxleButtons[counter].GetComponentInChildren<Text>().text = "Axle " + (counter + 1) + " [powered]";
            }
            else
            {
                transaxleButtons[counter].GetComponentInChildren<Text>().text = "Axle " + (counter + 1) + " [passive]";
            }
        }
    }

    public void ToggleAxle(int nr)
    {

        int poweredAxleCount = 0;
        foreach(AxleInfo axle in vehicle.GetComponent<CarController>().axleInfos)
        {
            if (axle.motor)
                poweredAxleCount += 1;
        }

        if (vehicle.GetComponent<CarController>().axleInfos[nr].motor && poweredAxleCount >= 2) {
            vehicle.GetComponent<CarController>().axleInfos[nr].motor = false;
            transaxleButtons[nr].GetComponentInChildren<Text>().text = "Axle " + (nr + 1) + " [passive]";
        }
        else {
            vehicle.GetComponent<CarController>().axleInfos[nr].motor = true;
            transaxleButtons[nr].GetComponentInChildren<Text>().text = "Axle " + (nr + 1) + " [powered]";
        }

    }


    public void ToggleAbs(bool toggle)
    {
        if(vehicle.GetComponent<CarController>().abs == false)
            vehicle.GetComponent<CarController>().abs = true;
        else
            vehicle.GetComponent<CarController>().abs = false;
    }


    public void ToggleAwd(bool toggle)
    {
        if (vehicle.GetComponent<CarController>().awd == false)
            vehicle.GetComponent<CarController>().awd = true;
        else
            vehicle.GetComponent<CarController>().awd = false;
    }

    private void SetMeter()
    {

        if (isMetric == true)
        {
            unitUI.text = "km/h";
        }
        else
        {
            unitUI.text = "mph";
        }

        gearUI.text = gearText;
        speedUI.text = speed.ToString();


        float offset = 360 - maxRpmAngle;
        float zeroRpmOffsetAngle = zeroRpmAngle + offset;

        targetRpm = vehicle.GetComponent<CarController>().engine.currentRpm;
        rpm = Mathf.Lerp(rpm, targetRpm, Time.deltaTime * 6);

        float offsetAngle = zeroRpmOffsetAngle - (zeroRpmOffsetAngle * (rpm / maxRpm));

        float angle = offsetAngle - offset;

        float rotateAngle = 0.0f;
        if (angle <= 0)
        {
            rotateAngle = 360.0f + angle;
        }
        else
        {
            rotateAngle = angle;
        }

        rpmNeedle.transform.eulerAngles = new Vector3(0, 0, rotateAngle);
    }

    public void SetSpeed(float currentSpeed)
    {
        if (isMetric == false)
        {
            currentSpeed *= 0.621371f;
        }
        speed = Mathf.RoundToInt(currentSpeed);
    }

    public void SetGear(int currentGear)
    {
        gear = currentGear;
        gearText = gears[gear];
    }

    public void levelReset()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    
    public void motorToggle()
    {
        for(int i = 0; i < vehicle.GetComponent<CarController>().axleInfos.Count; i++)
        {

        }
    }
    
}
