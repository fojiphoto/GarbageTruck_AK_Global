using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/* AXLE CLASS
 * Each instance contains two WheelInfo instances.
 * Distributes torque between front and rear.
 */
[System.Serializable]
public class AxleInfo {
	public WheelInfo leftWheel;
	public WheelInfo rightWheel;
	[Tooltip("Does this axle recieve torque?")]
	public bool motor = true;
	[Tooltip("Does this axle steer?")]
	public bool steering = false;
	[Tooltip("Multiplies stering angle by this. Used for multi wheel steering. Negative for rear wheel steer.")]
	public float steerCoeff = 0; //for multiaxle, negative value reverses steer
	[HideInInspector]
	public float torque;

	// Axle slip
	[HideInInspector]
	public float axleRpm; //Abs()
	[HideInInspector]
	public bool axleSlip;
}

/* WHEEL CLASS
 * Each wheel has one instance, passes final params to colliders and transfoms. 
 * Distributes torque between left and right.
 */
[System.Serializable]
public class WheelInfo {
	[Tooltip("Wheel collider corresponding to the wheel. One of the parents has to have Rigidbody for it to work!")]
	public WheelCollider collider;
	[Tooltip("Visual wheel transform corresponding to the wheel")]
	public Transform visual;

	[HideInInspector]
	public float torque;
	[HideInInspector]
	public float brakeTorque;
	[HideInInspector]
	public bool slip;
}

/* TURBINE CLASS
 * Influences engine power and sound based on rpm
 */
[System.Serializable]
public class Turbine {
	public float rpm;

	[Tooltip("Regulates speed of turbine spinning up. Try 0.2-2")]
	public float spinupSpeedMod = 0.5f;
	[Tooltip("Turbine max rpm.")]
	public float maxRpm = 100000;
	[Tooltip("RPM of the engine that the turbine will start spinning up at.")]
	public float startRpm = 0;
	[Tooltip("Engine power multiplier at full turbine RPM, try 0.05-1")]
	public float powerModifier = 0.3f;
	[Tooltip("Turbine sound to use.")]
	public AudioSource sound;

	public void Main(Engine engine){
		if( engine.currentRpm > startRpm ){ //start spinning up
			//float spinupSpeed = ((( engine.currentRpm-engine.minRpm ))/ maxRpm ) * spinupSpeedMod;

			rpm = Mathf.Lerp( rpm, engine.currentRpm * (maxRpm/engine.maxRpm), Time.deltaTime * spinupSpeedMod);

			if( rpm > maxRpm ){
				rpm = maxRpm;
			} 
		} else {
			rpm = 0;
		}
	}
}

/* ENGINE CLASS
 * Takes care of generating appropriate torque and power for the given rpm. 
 * Does not control RPM - RPM controls it.
 */
[System.Serializable]
public class Engine {
	// RPM
	[Tooltip("Maximum RPM engine is allowed to have.")]
	public float maxRpm = 4200;
	[HideInInspector]
	public float currentRpm;
	[Tooltip("Minimum RPM engine is allowed to have.")]
	public float minRpm = 700;
	//[HideInInspector]
	public float load; //0...1
	[Tooltip("Does this engine have turbo charger?")]
	public bool hasTurbine = false;
    [Tooltip("Engine sound to use.")]
    public AudioSource sound;

    public Turbine turbine;

	[Tooltip("Array of different power values in kW at steps of 1000rpm. Torque curve is generated from this. Change step in getPowerAtRpm function for smaller or bigger steps.")]
	public float[] powerArray = new float[7]{0,30,60,80,95,105,100}; //represents power(kW) at steps of 1k rpm


	public void Main(CarController controller){ //engine can only directly communitcate with transmission
		if(hasTurbine){
			turbine.Main (this);
		}

        currentRpm = Mathf.Abs (controller.transmission.toEngineRpm);

        // rpm limiter
        if (currentRpm < minRpm)
        {
            currentRpm = minRpm;
        }
        else if (currentRpm > maxRpm)
        {
            currentRpm = maxRpm;
        }

        // Update torque based on engine rpm and power
        controller.transmission.fromEngineTorque = getPowerAtRpm(powerArray, currentRpm)*9549 / (currentRpm+1); // torque(Nm) = 9549*Power(kW)/Speed(RPM)
		if(hasTurbine)
			controller.transmission.fromEngineTorque *= ( 1 + (turbine.rpm/turbine.maxRpm) * turbine.powerModifier );

		// Rev control between shifts
		if(controller.transmission.isShifting){
			currentRpm = minRpm;
		}

		// Calculate load based on input and speed feedback
		float speedDiff = 0.2f - (controller.speed - controller.previousSpeed) * (1 - (currentRpm/maxRpm));
		load = Mathf.Clamp(speedDiff*5, 0.0f, 1.0f) * controller.inputs.yAxis; 
		if(load < 0) load = 0;

		if(controller.inputs.direction != 0){
			if(speedDiff > 0){
				//regulates perception of load. Divide speed whith larger number to get longer "clutch"
				currentRpm += load * (2000/( (controller.speed*controller.speed)+2)); 
			}
		}

        if (currentRpm > maxRpm)
        {
            currentRpm = maxRpm;
        }

    }

	// Interpolate between values based on powerArray as y and rpm as x axis
	float getPowerAtRpm(float[] powerArray, float rpm){
		int selector;
		int step = 1000;
		float x_axis;
		
		selector = (int)(Mathf.Floor(rpm / step));
		x_axis = currentRpm % step;
		return powerArray[selector] + ((x_axis / step) * powerArray[selector+1]);
	}
}


/* TRANSMISSION CLASS 
 * Manages rpm for engine, changes gears and passes torque from engine to diffs.
 */
[System.Serializable]
public class Transmission {
	// rpm
	[HideInInspector]
	public float toEngineRpm; // controlls engine

	[Tooltip("At this RPM engine wants to shift up a gear. This number dynamically increases with load.")]
	public int shiftUpRpm = 2700;

	[Tooltip("At this RPM engine wants to shift down a gear.")]
	public int shiftDownRpm = 1400;

	// gears
	[Tooltip("Gear ratios for each gear. First element is reverse and should be < 0, second element is N, element 3 is 1st gear and so on.")]
	public float[] gearRatios = new float[7]{-6, 0, 7, 5, 3.7f, 3, 2.6f};
	[Tooltip("All other gear rations are multiplied by it. Try 1-10. Higher the number slower vehicle but more torque.")]
	public float finalGearRatio = 3.7f; //Simulates both final gear and differential ratio, = final gear ratio * diff. ratio
	[HideInInspector]
	public int gear;

	// shift duration
	[Tooltip("Time(s) it takes driver to shift. No torque is delivered to wheels while shifting. Try 0.3-0.8.")]
	public float shiftDuration = 0.4f;
	[HideInInspector]
	float shiftTimer;
	[HideInInspector]
	public bool isShifting;

	// shift disable after each shift
	[Tooltip("Time(s) from start of shift until next shift is allowed. Can prevent gear hunting. Try 0.3-1.")]
	public float shiftDisableDuration = 0.7f;
	[HideInInspector]
	public float disableTimer;
	[HideInInspector]
	public bool isDisabled;

	// torque
	[HideInInspector]
	public float fromEngineTorque;
	[HideInInspector]
	public float toWheelTorque; // to wheels

	public void Main(CarController controller){

		float rpmSum = 0;
		foreach(AxleInfo axleInfo in controller.axleInfos){
			if(axleInfo.motor){
				rpmSum += axleInfo.axleRpm;;
			}
		}

		toEngineRpm = ((rpmSum/controller.motorAxleCount)) * gearRatios[gear] * finalGearRatio;

		// Get torque to wheels, if allowed
		if( (toEngineRpm > controller.engine.maxRpm && gear == gearRatios.Length) 
		   || isShifting 
		   || (Mathf.Abs(toEngineRpm) > controller.engine.maxRpm && gear == 0)){
			toWheelTorque = 0;
		} else {
			toWheelTorque = fromEngineTorque * finalGearRatio * gearRatios[gear];
		}

		#region ShiftingLogic

		if(isShifting){ // shift delay
			shiftTimer += Time.deltaTime;
			isDisabled = true;
			if(shiftTimer > shiftDuration){
				isShifting = false;
			}
		} else {
			shiftTimer = 0;
		}

		if (isDisabled){ // disable shifting in too close intervals
			disableTimer += Time.deltaTime;
			if(disableTimer > shiftDisableDuration){
				isDisabled = false;
			}
		} else {
			disableTimer = 0;
		}

		// if accelerating in reverse without user input, e.g. downhill
		if( controller.inputs.direction == 0 && controller.velocity < 0){
			gear = 0;
			isShifting = true;
		}

		// in gear
		if(gear >= 2){ 
			// changing gears
			if( controller.inputs.direction >= 0 ){
				float shiftUpSpeed = (( shiftUpRpm+(shiftUpRpm*(controller.engine.load/10))) / finalGearRatio  / gearRatios[gear]) 
				                      * controller.axleInfos[0].leftWheel.collider.radius * 0.10472f;
				float shiftDownSpeed = (( shiftDownRpm+(shiftDownRpm*(controller.engine.load/10))) / finalGearRatio  / gearRatios[gear]) 
				                        * controller.axleInfos[0].leftWheel.collider.radius * 0.10472f;

				if( (controller.speed > shiftUpSpeed && controller.speed > 4)){
					isShifting = ShiftUp();
				}
				else if( controller.speed < shiftDownSpeed)
                {
					isShifting = ShiftDown();
				}
			} 
			else if( controller.velocity < 0.4f ){
				gear = 0;
				isShifting = true;
			}
		}
		// neutral
		else if(gear == 1){ 
			if( controller.inputs.direction > 0 ){
				isShifting = ShiftUp();
			}
			else if( controller.inputs.direction < 0 ){
				isShifting = ShiftDown();
			}
		}
		// reverse
		else if(gear == 0){ 
			if( controller.inputs.direction > 0 ){
				isShifting = ShiftUp ();
			}
		}

		// shift to neutral
		if( gear != 1 && toEngineRpm < 400 && controller.inputs.direction == 0 ){
			gear = 1;
			isShifting = true;
		}

		// shift from neutral to reverse
		if( gear == 1 && controller.inputs.direction < 0 && controller.velocity < 1){
			gear = 0;
			isShifting = true;
		}
		
		#endregion
	}

	public bool ShiftUp(){
		if( !isShifting && !isDisabled && gear < gearRatios.Length - 1){
			gear++;
			return true;
		}
		return false;
	}

	public bool ShiftDown(){
		if(!isShifting && !isDisabled && gear > 2){
			gear--;
			return true;
		}
		return false;
	}
}

/* SOUND CLASS
 * Manages pitch and volume
 */
[System.Serializable]
public class Sound {

	// Pitch
	float oldPitch;
	float targetPitch;

	//Volume 
	float targetVolume;

	public void Main(CarController controller){

		if(controller.engine.sound != null){
			#region pitch
			// remember last pitch value
			oldPitch = controller.engine.sound.pitch;

			// base pitch value
			targetPitch = 1.0f;

			// pitch based on rpm
			targetPitch += ( (controller.engine.currentRpm - controller.engine.minRpm) / controller.engine.maxRpm)*1.5f;

			// random variations
			targetPitch += Random.Range (-0.10f, 0.10f);

			// limit
			if(targetPitch > 10) targetPitch = 10;

			// adjust for SloMo
			if(Time.timeScale < 0.9f){ //if slomo
				targetPitch *= Time.timeScale;
                controller.engine.sound.pitch = Mathf.Lerp(oldPitch, targetPitch, 15 * Time.deltaTime);
			// not slomo
			} else {
				// make shifting and acceleration more realistic
				if(oldPitch < targetPitch){ // if pitch speeding up
                    controller.engine.sound.pitch = Mathf.Lerp(oldPitch, targetPitch, 8 * Time.deltaTime);
				} else { // pitch slowing donwn
                    controller.engine.sound.pitch = Mathf.Lerp(oldPitch, targetPitch, 5 * Time.deltaTime);
				}
			}
			#endregion

			#region volume

			// base volume
			targetVolume = 0.3f;

			// adjust for user input
			targetVolume += Mathf.Abs(controller.inputs.yAxis)*0.1f;

			// adjust for rpm, more = louder
			targetVolume += ( (controller.engine.currentRpm - controller.engine.minRpm) / controller.engine.maxRpm) * 0.2f;

            controller.engine.sound.volume = targetVolume;
			#endregion
		}

		if(controller.engine.hasTurbine){
			controller.engine.turbine.sound.volume = ((controller.engine.turbine.rpm / controller.engine.turbine.maxRpm)*0.1f);
			controller.engine.turbine.sound.pitch = ((controller.engine.turbine.rpm / controller.engine.turbine.maxRpm)*0.5f)+0.1f;
		}
	}
}

/* LIGHT CLASS 
 * Manages headlights and taillights
 */
[System.Serializable]
public class Lighting {
	// taillights
	[Tooltip("Point source of light at the position of left rear tail light.[optional]")]
	public Light rearLeft;
	[Tooltip("Point source of light at the position of right rear tail light.[optional]")]
	public Light rearRight;

	public void Main(CarController controller){
		// tailights
		if( rearLeft != null && rearRight != null){
			if(controller.transmission.gear > 0 && !controller.brake){
				rearLeft.intensity = 4;
				rearRight.intensity = 4;
			} else {
				rearLeft.intensity = 8;
				rearRight.intensity = 8;
			}
		}
	}
}

/* PARTICLE CLASS
 * Manages exhaust particles
 */
[System.Serializable]
public class Smoke {
	[Tooltip("Exhaust particle source. [optional]")]
	public ParticleSystem exhaust;
	
	public void Main(CarController controller){
		if(exhaust != null){
			exhaust.startSpeed = (controller.engine.currentRpm*4 / controller.engine.maxRpm)+1;

			float color = 0.6f - (controller.engine.currentRpm / (controller.engine.maxRpm*3)); //grey to black
			exhaust.startColor = new Color( color, color, color, 1 - color);

            var em = exhaust.emission;
            var rate = new ParticleSystem.MinMaxCurve();
            rate.constantMax = (controller.engine.currentRpm / controller.engine.maxRpm) * 25;
            em.rate = rate;
		}
	}
}

/* INPUT CLASS
 * For managing user controlls
 */
[System.Serializable]
public class Inputs {
	[HideInInspector]
	public float xAxis;
	[HideInInspector]
	public float yAxis;
	[HideInInspector]
	public float direction;


	public void Main(CarController controller){
		if (Application.platform == RuntimePlatform.Android){
			xAxis = Input.acceleration.x;
			yAxis = Input.acceleration.y;
		} else {
			/** MOUSE CONTROLL 
			xAxis = ((Input.mousePosition.x) - (Screen.width/2))/ (Screen.width/2);
			yAxis = ((Input.mousePosition.y) - (Screen.height/2))/ (Screen.height/2);*/
			xAxis = Input.GetAxis("Horizontal");
			yAxis = Input.GetAxis("Vertical");
		}

		xAxis = Mathf.Clamp(xAxis, -1.0f, 1.0f);
		yAxis = Mathf.Clamp(yAxis, -1.0f, 1.0f);

		// input direction
		if(yAxis > 0){
			direction = 1;
		}
		else if(yAxis == 0){
			direction = 0;
		}
		else {
			direction = -1;
		}

		/* MAPPING INPUTS **********************/
		
		if(Input.GetKey (KeyCode.Q))
			controller.awd = !controller.awd;
		
		if(Input.GetKey (KeyCode.E))
			controller.abs = !controller.abs;
		
		if(Input.GetKey (KeyCode.Space)){
			controller.brake = true;
			controller.brakeTorque = controller.maxBrakeTorque; 
		}
		else {
			controller.brake = false;
			controller.brakeTorque = 0;
		}

        /* Slow motion / can be buggy
		#region SloMo
		if (Input.GetButtonDown("Fire1")) {
			if (Time.timeScale > 0.95F)
				Time.timeScale = 0.3F;
			else
				Time.timeScale = 1.0F;
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}
		#endregion
		*/

	}
}

/* MAIN CONTROLLER CLASS
 * Manages car behavior, controls, wheels and diffs
 */
public class CarController : MonoBehaviour {
	// Level Management
	[HideInInspector]
	public bool vehicleIsActive;

	// Objects
	[Tooltip("List of axles and wheels a vehicle has. Up to 10 axles, 2 wheels each.")]
	public List<AxleInfo> axleInfos; 
	public Engine engine;
	public Transmission transmission;
	[HideInInspector]
	public Sound sound;
	public Lighting lights;
	public Smoke smoke;
	[HideInInspector]
	public Inputs inputs;

	// Debugs 
	[HideInInspector]
	public float debTorque; //sum of all wheel torques, can be used as control variable

	// Speed
	[HideInInspector]
	public float velocity; 
	/* velocity is in m/s, multiply by 3.6 for kmh
	 * don't modify directly beacause all physics in this script is calculated in m/s 
	 */
	[HideInInspector]
	public float speed; // abs(velocity)
	[HideInInspector]
	public float previousSpeed; // load sensing
	[HideInInspector]
	public int direction; // vehicle direction = -1, 0, 1

	// Steer
	[Tooltip("Reduces maxSteeringAngle at higher speeds for more stable vehicle. Try 3-20.")]
	public float speedSensitiveSteering = 7; //larger = more influence on steering at speed
	[Tooltip("Maximum angle in degrees a wheel can turn.")]
	public float maxSteeringAngle = 42; // maximum wheel steer angle
	[Tooltip("Miniumum maxSteeringAngle in degrees a wheel can turn. Comes into effect in combination with speed sensitive steering.")]
	public float minSteeringAngle = 20; // minimum wheel steer angle

	// Differential
	[Tooltip("Enable AWD. Torque will be distributed between wheels depending on velocity.")]
	public bool awd = true;
	[HideInInspector]
	public bool brake;
	[HideInInspector]
	public float maxAvailableTorque;
	[HideInInspector]
	public bool wheelSlip; //if any of the weels have slipped, dont upshift
	[HideInInspector]
	public int motorAxleCount;

	// Brakes
	[Tooltip("Maximum system braking torque to the wheels when braking is applied. Divided between wheels.")]
	public float maxBrakeTorque = 30000;
	[HideInInspector]
	public float brakeTorque;
	[Tooltip("Prevents wheels from locking when braking.")]
	public bool abs = true;

	// GUI (delete if you delete OnGUI()
	GUIStyle whiteText = new GUIStyle();
	GUIStyle blackText = new GUIStyle();
	GUIStyle greyText = new GUIStyle();
	static Texture2D _staticRectTexture;
	static GUIStyle _staticRectStyle;


	void Start(){

		//GUI
		whiteText.normal.textColor = Color.white;
		blackText.normal.textColor = Color.black;
		greyText.normal.textColor = Color.grey;

		//End GUI
		motorAxleCount = countPoweredAxles();
    }

	public void FixedUpdate(){
		
		// Update different classes
		transmission.Main(this);
		engine.Main(this);
		sound.Main(this);
		lights.Main(this);
		smoke.Main(this);
		inputs.Main(this);

		motorAxleCount = countPoweredAxles(); //remove this line if you won't change number of powered axles during gameplay

		previousSpeed = speed; //for load calculation
		velocity = transform.InverseTransformDirection(this.GetComponent<Rigidbody>().velocity).z; // -...+
		speed = Mathf.Abs(velocity); // 0...+

		direction 		= velocity 	>= 0 ? 1 : -1;

		if(transmission.gear != 0){ // reverse gear neds reverse inputs
			maxAvailableTorque = transmission.toWheelTorque * inputs.yAxis;
		} else {
			maxAvailableTorque = transmission.toWheelTorque * -inputs.yAxis;
		}


		// change max steering based on current speed
		float steering = (maxSteeringAngle/(((speed/100)*speedSensitiveSteering)+1)) * inputs.xAxis;

		// get axle speed for final speed calculation
		foreach(AxleInfo axleInfo in axleInfos){
			axleInfo.axleRpm = (Mathf.Abs(axleInfo.leftWheel.collider.rpm) + Mathf.Abs(axleInfo.rightWheel.collider.rpm))/2;
		}

		/* AWD SYSTEM */
		if(awd){
			#region Torque distribution
			/* TORQUE DISTRIBUTION BETWEEN AXLES 
			 * maxAvailableTorque is spread onto axles depending on rotation speed
			 */

			//get rpm and motor value for each axle
			float[] rpms = new float[axleInfos.Count];
			bool[] motorEnabled = new bool[axleInfos.Count]; //enable torque distribution if axle.motor = true
			for(int i = 0; i < axleInfos.Count; i++){
				rpms[i] = axleInfos[i].axleRpm;
				motorEnabled[i] = axleInfos[i].motor;
			}

			//put rpm values into torque splitter, result is torque array
			float[] torqueArr;
			torqueArr = TorqueSplit(motorEnabled, rpms, maxAvailableTorque);

			for(int i = 0; i < axleInfos.Count; i++){
				if(motorEnabled[i]) axleInfos[i].torque = torqueArr[i];
			}


			/* TORQUE DISTRIBUTION BETWEEN WHEELS */
			debTorque = 0;
			foreach (AxleInfo axleInfo in axleInfos) {

				float[] wheelTorqueArr;
				wheelTorqueArr = TorqueSplit(	new bool[]{true, true}, 
												new float[]{axleInfo.rightWheel.collider.rpm, axleInfo.leftWheel.collider.rpm}, 
												axleInfo.torque);

				if(axleInfo.rightWheel.collider.isGrounded)
					axleInfo.rightWheel.torque = wheelTorqueArr[0];
				else 
					axleInfo.rightWheel.torque = 0;

				if(axleInfo.leftWheel.collider.isGrounded)
					axleInfo.leftWheel.torque = wheelTorqueArr[1];
				else 
					axleInfo.leftWheel.torque = 0;

				debTorque += axleInfo.torque;
			}
			#endregion
		}

		/* NOT AWD ***/
		else {
			foreach(AxleInfo axleInfo in axleInfos){
				if (axleInfo.motor) {
					axleInfo.torque = maxAvailableTorque / motorAxleCount;
					axleInfo.leftWheel.torque = axleInfo.torque / 2;
					axleInfo.rightWheel.torque = axleInfo.torque / 2;
				}
			}
		}

		// WHEEL SLIPPING
		// set wheel slip if any exists to disable shifting
		wheelSlip = false;
		foreach(AxleInfo axleInfo in axleInfos){
			if(DetectWheelSlip(axleInfo.leftWheel, speed))
				axleInfo.leftWheel.torque = 0; // acts out as mechanical loss when no power applied, spins down the wheel
			if(DetectWheelSlip(axleInfo.rightWheel, speed))
				axleInfo.rightWheel.torque = 0;
		}

		foreach(AxleInfo axleInfo in axleInfos){
			wheelSlip = axleInfo.leftWheel.slip ? true : wheelSlip;
			wheelSlip = axleInfo.rightWheel.slip ? true : wheelSlip;
			if(wheelSlip) break;
		}

		#region Collider update
		// UPDATE COLLIDERS WITH CALCULATED VALUES
		foreach(AxleInfo axleInfo in axleInfos){

			// apply steering to colliders
			if (axleInfo.steering) {
				axleInfo.leftWheel.collider.steerAngle = steering * axleInfo.steerCoeff;
				axleInfo.rightWheel.collider.steerAngle = steering * axleInfo.steerCoeff;
			}

			// if axle has motor enabled apply torque values to wheels
			if (axleInfo.motor) {
				axleInfo.leftWheel.collider.motorTorque = axleInfo.leftWheel.torque;
				axleInfo.rightWheel.collider.motorTorque = axleInfo.rightWheel.torque;
			} else {
				axleInfo.torque = 0;
				axleInfo.leftWheel.collider.motorTorque = 0;
				axleInfo.rightWheel.collider.motorTorque = 0;
			}

			#region Braking
			/* BRAKING SECTION */
			// if brake enabled calculate and apply brake torque to wheels;
			if((Mathf.Abs(inputs.yAxis) > 0.2f && inputs.direction != direction && speed > 0.2f)){
				brake = true;
				brakeTorque = maxBrakeTorque;
			}

			if (brake) {

				#region ABS
				// abs active
				if( (abs && speed > 0.5f)){ 
					if( Mathf.Abs(axleInfo.leftWheel.collider.rpm) < (speed * 2) ){
						axleInfo.leftWheel.brakeTorque = 0;
					} else {
						axleInfo.leftWheel.brakeTorque = brakeTorque / 4;
					}
				
					if( Mathf.Abs(axleInfo.rightWheel.collider.rpm) < (speed * 5) ){
						axleInfo.rightWheel.brakeTorque = 0;
					} else {
						axleInfo.rightWheel.brakeTorque = brakeTorque / 4;
					}
				}
				#endregion

				// no abs
				else { 
					axleInfo.leftWheel.brakeTorque = brakeTorque / 4;
					axleInfo.rightWheel.brakeTorque = brakeTorque / 4;
				}

				axleInfo.leftWheel.collider.brakeTorque = axleInfo.leftWheel.brakeTorque;
				axleInfo.rightWheel.collider.brakeTorque = axleInfo.rightWheel.brakeTorque;
			} 
			else { 
				// release brakes when no brake applyed
				axleInfo.leftWheel.collider.brakeTorque = 0;
				axleInfo.rightWheel.collider.brakeTorque = 0;
			}
			#endregion

			//Update wheel visual rotation and position
		}
		#endregion

		//Update rotation and position of wheels 
		UpdateWheelVisuals(axleInfos);
	}
		
	public void UpdateWheelVisuals(List<AxleInfo> axleInfos){
		foreach(AxleInfo axleInfo in axleInfos){
			UpdateWheel(axleInfo.leftWheel);
			UpdateWheel(axleInfo.rightWheel);
		}
	}
	
	public void UpdateWheel(WheelInfo wheel){
		Vector3 position;
		Quaternion rotation;
		wheel.collider.GetWorldPose(out position, out rotation);
		wheel.visual.transform.position = position;
		wheel.visual.rotation = rotation;
    }
	
	public float[] TorqueSplit(bool[] enabled, float[] rpms, float availableTorque){
		float[] torqueArr = new float[rpms.Length];
		bool lowRpmAlert = false;
		int enabledCount = 0;

		for(int i = 0; i < rpms.Length; i++){
			rpms[i] = Mathf.Abs(rpms[i]);
			if(enabled[i]){
				lowRpmAlert = rpms[i] < 1 ? true : lowRpmAlert;
				enabledCount++;
			}
		}

		// if one axle dont calculate the rest
		if(enabledCount == 1){
			for(int i = 0; i < torqueArr.Length; i++){
				if(enabled[i]) torqueArr[i] = availableTorque;
			}
		// if multiple axes are enabled
		} else {
			// if low rpm dont calculate torque, spread it evenly
			if(lowRpmAlert){
				for(int i = 0; i < torqueArr.Length; i++){
					if(enabled[i]) torqueArr[i] = availableTorque / enabledCount;
				}
			} 
			else { 
				// if normal rpm spread torque by rpm
				float rpmSum = 0;
				for(int i = 0; i < rpms.Length; i++){
					if(enabled[i]) rpmSum += rpms[i];
				}
				float[] torqueDemand = new float[rpms.Length];
				float torqueDemandSum = 0;
				for(int i = 0; i < rpms.Length; i++){
					if(enabled[i]){
						torqueDemand[i] = ( 1-(rpms[i]/rpmSum) ); 
						torqueDemandSum += torqueDemand[i];
					}
				}
				for(int i = 0; i < rpms.Length; i++){
					if(enabled[i]){
						torqueArr[i] = ( torqueDemand[i] / torqueDemandSum ) * availableTorque;
					}
				}
			}
		}
		
		return torqueArr;
	}
	
	public bool DetectWheelSlip(WheelInfo wheel, float speed){
        /* SLIP DETECTION BASED ON COLLIDER, this can be used as an alternative
        WheelHit hit = new WheelHit();
        if (wheel.collider.GetGroundHit(out hit))
        {
            if (hit.forwardSlip > wheel.collider.forwardFriction.extremumSlip*1.1f)
            {
                wheel.slip = true;
                return true;
            }
        }*/


        /* SLIP CALCULATION BASED ON WHEEL SPEED
		speed (m/s) = wheel.r * RPM * 0.10472 */
		if( Mathf.Abs(wheel.collider.radius * wheel.collider.rpm * 0.10472f) > (speed*1.5f)+3 ){
			// wheel is faster than vehicle, slip occured
			wheel.slip = true;
			return true;
		}

        wheel.slip = false;
		return false; 
	}


    public int countPoweredAxles()
    {
        int motorAxleCount = 0;
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.collider.ConfigureVehicleSubsteps(5, 20, 20); //fix for wheel physics glitch, rpm goes crazy
            axleInfo.rightWheel.collider.ConfigureVehicleSubsteps(5, 20, 20);
            if (axleInfo.motor)
            {
                motorAxleCount++;
            }
        }
        return motorAxleCount;
    }

    public void Active(bool state)
    {
        vehicleIsActive = state;
    }

}
