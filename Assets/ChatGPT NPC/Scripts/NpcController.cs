using UnityEngine;

public class NpcController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NpcAnimator _npcAnimator;
    private NpcInteractionController interactionController;

    [Header("Parameters")]
    [SerializeField] private int id;
    [SerializeField] private string introduction;
    [TextArea(15, 20)][SerializeField] private string prompt;

    [Space]
    [Range(0.01f, 10f)]
    [SerializeField] private float rotationSpeed = 1.0f;
    [Range(0f, 5f)]
    [SerializeField] private float interactionDistance = 3.0f;

    [Space]
    [SerializeField] private NpcState state;
    [SerializeField] private KeyCode conversationKey;

    public NpcAnimator NpcAnimator { get => _npcAnimator; }

    private void Start()
    {
        interactionController = new NpcInteractionController(this, id, introduction, prompt);
    }

    private void Update()
    {
        if (Input.GetKeyUp(conversationKey) && Vector3.Distance(transform.position, Camera.main.transform.position) < interactionDistance)
        {
            ChangeState(NpcState.Interaction);
            Interact();
        }

        // Rotation to player
        if (state == NpcState.Interaction)
        {
            Transform cameraTransform = Camera.main.transform;
            Vector3 targetPosition = new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z);
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); 
        }
    }

    public void Interact()
    {
        if (state == NpcState.Interaction)
        {
            interactionController.ActivateMenu();
        }
    }

    public void ChangeState(NpcState newState)
    {
        state = newState;
    }
}
public enum NpcState { Idle, Interaction }