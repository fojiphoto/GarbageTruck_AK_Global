using UnityEngine;
public class UserNevigationArrowHandler : MonoBehaviour
{
    public Transform[] Waypoints;
    public float distance = 1f;
    LineRenderer LR;
    public void OnEnable()
    {
        if (GetComponent<LineRenderer>())
        {
            LR = GetComponent<LineRenderer>();
            LR.textureMode = LineTextureMode.Tile;
            LR.alignment = LineAlignment.TransformZ;
            LR.positionCount = Waypoints.Length;
            LR.SetPosition(0, transform.position);
            LR.material.mainTextureScale = new Vector3(distance * Waypoints.Length, 1);
            for (int i = 0; i < Waypoints.Length; i++)
                LR.SetPosition(i, new Vector3(Waypoints[i].transform.position.x, Waypoints[i].transform.position.y, Waypoints[i].transform.position.z));
        }
        else { Debug.LogError("No Line Renderer Attached"); }
    }
}
