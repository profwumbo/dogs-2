using UnityEngine;
using static worlditems;

public class Player : MonoBehaviour
{
    public Camera playerCamera;
    public Transform handSocket;
    public float moveSpeed = 4f;
    public float mouseSensitivity = 120f;
    public float gravity = -9.81f;
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
        Debug.Log("Update running");
        MouseLook();
        Move();
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
