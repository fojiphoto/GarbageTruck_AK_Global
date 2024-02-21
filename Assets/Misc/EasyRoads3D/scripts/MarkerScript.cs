using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EasyRoads3D;
public class MarkerScript : MonoBehaviour {
public float tension = 0.5f;
public float ri;
public float OOQOQQOO;
public float li;
public float ODODQQOO;
public float rs;
public float ODOQQOOO;
public float ls;
public float DODOQQOO;
public float rt;
public float qt;
public float ODDQODOO;
public float lt;
public float ODDOQOQQ;
public bool OQODDCOODQ;
public bool ODQDOQOO;
public float OOQQQOCQCD;
public float ODOOQQOO;
public Transform[] OOCODCOCQQs;
public float[] trperc;


public Vector3 oldPos = Vector3.zero;
public bool autoUpdate;
public bool changed;
public Transform surface;
public bool OCDQCOOOOC;
Vector3 position;
private bool updated;
private int frameCount;
private float currentstamp;
private float newstamp;
private bool mousedown;
private Vector3 lookAtPoint;
public bool bridgeObject;
public  bool distHeights;
public RoadObjectScript objectScript;
public List<string> OQODQQDO = new List<string>();
public List<bool> ODOQQQDO = new List<bool>();
public List<bool> OQQODQQOO = new List<bool>();
public List<float> ODDOQQOO = new List<float>();


public string[] ODDOOQDO;
public bool[] ODDGDOOO;
public bool[] ODDQOOO;
public float[] ODDQOODO;
public float[] ODOQODOO;
public float[] ODDOQDO;
public int markerNum;
public string distance = "0";
public string OOOOQDCCQQ = "0";
public string OCOQOQCDCD = "0";
public bool newSegment = false;
public float floorDepth = 2f;
public float oldFloorDepth = 2f;
public float waterLevel = 0.5f;
public bool lockWaterLevel = true;
public bool tunnelFlag = false;
public bool sharpCorner = false;

public bool snapMarker = false;
public int markerInt = 0;
void Start () {

}
void OnDrawGizmos()
{
if(objectScript != null){
if(!objectScript.OQCCDOOOCC){  


Vector3 v;
if(snapMarker){
if(OQCQQDCCCO.terrain != null){
v = transform.position;
v.y = OQCQQDCCCO.terrain.SampleHeight(v)+ OQCQQDCCCO.terrain.transform.position.y;
transform.position = v;
}
}

Vector3 change = transform.position - oldPos;
if(OQODDCOODQ && oldPos != Vector3.zero && change != Vector3.zero){
int i = 0;
foreach(Transform tr in OOCODCOCQQs){
tr.position += change * trperc[i];

if(snapMarker){
if(OQCQQDCCCO.terrain != null){
v = tr.position;
v.y = OQCQQDCCCO.terrain.SampleHeight(v);
tr.position = v;
}
}

i++;
}
}
if(oldPos != Vector3.zero && change != Vector3.zero){
changed = true;
if(objectScript.OQCCDOOOCC){
objectScript.OOOOODODCQ.specialRoadMaterial = true;
}
}
oldPos = transform.position;
}else if(objectScript.ODODDDOO){ 
transform.position = oldPos;
}
}
}
void SetObjectScript(){

objectScript = transform.parent.parent.GetComponent<RoadObjectScript>();
if(objectScript.OOOOODODCQ == null){

List<ODODDQQO> arr = OCDQDCQOCQ.OQOODCQQCO(false);
objectScript.ODODCDOOQC(arr, OCDQDCQOCQ.OQQQOOOOOC(arr), OCDQDCQOCQ.OQODCCCCCD(arr));


}
}
void GetMarkerCount(){
int c = 0;
foreach(Transform tr in transform.parent){
if(tr == transform){
markerInt = c;
break;
}
c++;
}
}
public void LeftIndent(float change, float perc){
ri += change * perc;
if(ri < objectScript.indent) ri = objectScript.indent;
OOQOQQOO = ri;
}
public void RightIndent(float change, float perc){
li += change * perc;
if(li < objectScript.indent) li = objectScript.indent;
ODODQQOO = li;
}
public void LeftSurrounding(float change, float perc){
rs += change * perc;
if(rs < objectScript.indent) rs = objectScript.indent;
ODOQQOOO = rs;
}
public void RightSurrounding(float change, float perc){
ls += change * perc;
if(ls < objectScript.indent) ls = objectScript.indent;
DODOQQOO = ls;
}
public void LeftTilting(float change, float perc){
rt += change * perc;
if(rt < 0) rt = 0;
ODDQODOO = rt;
}
public void RightTilting(float change, float perc){
lt += change * perc;
if(lt < 0) lt = 0;
ODDOQOQQ = lt;
}
public void FloorDepth(float change, float perc){
floorDepth += change * perc;
if(floorDepth > 0) floorDepth = 0;
oldFloorDepth = floorDepth;
}
public bool InSelected(){

for(int i = 0; i < objectScript.OOCODCOCQQs.Length; i++){
if(objectScript.OOCODCOCQQs[i] == this.gameObject)return true;
}
return false;
}
}
