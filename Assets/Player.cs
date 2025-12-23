using UnityEngine;
using static worlditems;

public class Player : MonoBehaviour
{
    public Camera playerCamera;
    public Transform handSocket;
    public float moveSpeed = 4f;
    public float mouseSensitivity = 3f;
    public float gravity = -9.81f;
    public Rigidbody RB;


    public float interactRange = 3f;
    public ItemType itemType;
    public GameObject handPrefab;

    public ItemType heldItem = ItemType.None;
    private GameObject heldVisual;

    public Transform cameraTransform;
    public interface IInteractable
    {
        void Interact(PlayerInteract player);
        void Interact(Player player);
    }

    float xRotation = 0f;
    Vector3 velocity;
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        //If my mouse goes left/right my body moves left/right
        float xRot = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, xRot, 0);

        //If my mouse goes up/down my aim (but not body) go up/down
        float yRot = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        playerCamera.transform.Rotate(yRot, 0, 0);

        //Movement code
        if (moveSpeed > 0)
        {
            //My temp velocity variable
            Vector3 move = Vector3.zero;

            //transform.forward/right are relative to the direction my body is facing
            if (Input.GetKey(KeyCode.W))
                move += transform.forward;
            if (Input.GetKey(KeyCode.S))
                move -= transform.forward;
            if (Input.GetKey(KeyCode.A))
                move -= transform.right;
            if (Input.GetKey(KeyCode.D))
                move += transform.right;
            //I reduce my total movement to 1 and then multiply it by my speed
            move = move.normalized * moveSpeed;

            //Plug my calculated velocity into the rigidbody
            RB.linearVelocity = move;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    

    void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }

    public void HoldItem(ItemType type, GameObject visualPrefab)
    {
        if (heldItem != ItemType.None) return;

        heldItem = type;
        heldVisual = Instantiate(visualPrefab, handSocket);
        heldVisual.transform.localPosition = Vector3.zero;
        heldVisual.transform.localRotation = Quaternion.identity;
    }

    public void ClearItem()
    {
        heldItem = ItemType.None;
        if (heldVisual != null)
            Destroy(heldVisual);
    }
    public void Interact(PlayerInteract player)
    {
        if (player.heldItem != ItemType.None) return;

        player.HoldItem(itemType, handPrefab);
        Destroy(gameObject);
    }
}
