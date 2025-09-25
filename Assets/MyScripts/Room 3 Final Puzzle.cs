using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Room3FinalPuzzle : MonoBehaviour
{
    [Header("Crown Objects")]
    [SerializeField] public GameObject Crown;
    [SerializeField] public GameObject ThroneCrown;
    [SerializeField] public string puzzleTag = "";
    [SerializeField] public MeshRenderer ThroneCrownMesh;
    [Header("Animators")]
    [SerializeField] public Animator FloorAnimator1;
    [SerializeField] public Animator FloorAnimator2;
    [SerializeField] public Animator ThroneAnimator;
    [Header("Tags")]
    [SerializeField] public string playTag = "";
    [SerializeField] public string throneTag = "";
    [Header("Setting up the end")]
    [SerializeField] public GameObject floor;
    [SerializeField] public GameObject stairs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ThroneCrownMesh = ThroneCrown.GetComponent<MeshRenderer>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(puzzleTag))
        {
            Destroy(Crown);
            ThroneCrownMesh.enabled = true;
            if (!FloorAnimator1 && FloorAnimator2) return;
            FloorAnimator1.SetTrigger(playTag);
            FloorAnimator2.SetTrigger(playTag);

            if (!ThroneAnimator) return;
            ThroneAnimator.SetTrigger(throneTag);
            Destroy(floor);
            stairs.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
