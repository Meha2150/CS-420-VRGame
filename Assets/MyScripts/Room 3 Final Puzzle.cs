using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Room3FinalPuzzle : MonoBehaviour
{
    [Header("Crown Objects")]
    public GameObject Crown;
    public GameObject ThroneCrown;
    public string puzzleTag = "";
    public MeshRenderer ThroneCrownMesh;
    [Header("Animators")]
    public Animator FloorAnimator1;
    public Animator FloorAnimator2;
    public Animator ThroneAnimator;
    [Header("Tags")]
    public string playTag = "";
    public string throneTag = "";
    [Header("Setting up the end")]
    public GameObject floor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ThroneCrownMesh = ThroneCrown.GetComponent<MeshRenderer>();
        FloorAnimator1 = GetComponent<Animator>();
        FloorAnimator2 = GetComponent<Animator>();
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
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
