using UnityEngine;
using System.Collections;

public class FallingObject : MonoBehaviour
{
    private bool isFalling = false;
    private float stopFallDelay = 1f;
    private Coroutine stopFallingCoroutine;
    private Vector3 initialPosition;

    // Store the parent before falling
    private Transform originalParent;
        
    private void Start()
    {
       initialPosition = transform.localPosition; // Store the initial local position before falling

        gameObject.SetActive(false);
        originalParent = transform.parent; // Store the parent before falling
        float randomDelay = Random.Range(0.5f, 6f);
        Invoke("StartFalling", randomDelay);
    }

    private void StartFalling()
    {
        gameObject.SetActive(true);
        isFalling = true;

        //float randomXForce = Random.Range(2f, 3f);
        // Rigidbody rb = GetComponent<Rigidbody>();
        // rb.AddForce(Vector3.right * randomXForce, ForceMode.Impulse);

        // Start the coroutine to stop falling after 1 second
        stopFallingCoroutine = StartCoroutine(StopFallingAfterDelay());
    }

    private void Update()
    {
        if (isFalling)
        {
            // Move the object down towards Y position 0
            float newY = Mathf.MoveTowards(transform.position.y, 0f, Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private IEnumerator StopFallingAfterDelay()
    {
        yield return new WaitForSeconds(stopFallDelay);
        isFalling = false; // Stop falling after the delay
        transform.parent = null;

        // Wait for 4 seconds and then re-add the object to its original parent
        yield return new WaitForSeconds(8f);
        transform.parent = originalParent;
        transform.localPosition = initialPosition; // Optionally, you can reset the local position to (0,0,0) to ensure it's correctly placed back in the parent's local space.

        stopFallingCoroutine = null; // Reset the coroutine reference
    }
}
