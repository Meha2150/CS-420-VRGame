using UnityEngine;

public class Room3PlateBController : MonoBehaviour
{
    [SerializeField] private string triggerTag = "Puzzle5B";
    

    // Track how many qualifying objects are on the plate (handles multiple colliders)
    public int platesDown = 0;

    private void Awake()
    {
        var col = GetComponent<Collider>();
        if (!col.isTrigger) Debug.LogWarning($"{name}: Collider is not set as Trigger.");
    }

    private bool MatchesTagB(Collider other)
    {
        return other.CompareTag(triggerTag) ||
               (other.attachedRigidbody && other.attachedRigidbody.CompareTag(triggerTag)) ||
               other.transform.root.CompareTag(triggerTag);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (!MatchesTagB(other) ) return;

        platesDown++;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!MatchesTagB(other) ) return;


        platesDown--;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
