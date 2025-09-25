using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Room3ItemInterations : MonoBehaviour
{
    [Header("Room 3 Plate Objects")]
    public GameObject Chest;
    public GameObject Barrel;
    [Header("Tag")]
    public string playerTag = "";

    private Rigidbody Chestrb;
    private Rigidbody Barrelrb;
    private XRGrabInteractable ChestGrab;
    private XRGrabInteractable BarrelGrab;

    void Start()
    {
        var col = GetComponent<Collider>();
        Chestrb = Chest.GetComponent<Rigidbody>();
        Barrelrb = Barrel.GetComponent<Rigidbody>();
        ChestGrab = Chest.GetComponent<XRGrabInteractable>();
        BarrelGrab = Barrel.GetComponent<XRGrabInteractable>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (Chestrb != null) Chestrb.isKinematic = false;
            if (ChestGrab != null) ChestGrab.enabled = true;

            if (Barrelrb != null) Barrelrb.isKinematic = false;
            if (BarrelGrab != null) BarrelGrab.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
