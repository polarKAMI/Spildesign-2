using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUIManager;
    private OptionsPanelManager optionsPanelManager;
    private PlayerJump playerJump;
    private PlayerMovement playerMovement;
    private Shoot Shoot;
    public PauseMenu pauseMenu;

    private bool isInventoryOpen = false; // Flag to track inventory state

    private KeyCode moveLeftKey;
    private KeyCode moveRightKey;

    bool leftKeyPressed = false;
    bool rightKeyPressed = false;
    bool rightKeyDormant = false;
    bool leftKeyDormant = false;

    void Start()
    {

        // Ensure inventoryUIManager is assigned before using it
        if (inventoryUIManager == null)
        {
            // Try to find the GameObject with the InventoryUIManager script
            inventoryUIManager = GameObject.Find("Inventory"); // Change "Inventory" to match the name of your GameObject
            if (inventoryUIManager == null)
            {
                Debug.LogError("InventoryUIManager GameObject not found in the scene.");
            }
        }
        optionsPanelManager = inventoryUIManager.GetComponent<OptionsPanelManager>();
        playerJump = GetComponent<PlayerJump>();
        playerMovement = GetComponent<PlayerMovement>();
        Shoot = GetComponent<Shoot>();
        pauseMenu = GameObject.FindObjectOfType<PauseMenu>();

        if (pauseMenu != null)
        {
            Debug.Log("pauseMenu successfully found in the scene.");
        }
        else
        {
            Debug.LogWarning("pauseMenu not found in the scene.");
        }
        moveLeftKey = GlobalInputMapping.activeInputMappings["MoveLeft"];
        moveRightKey = GlobalInputMapping.activeInputMappings["MoveRight"];
    }


    void Update()
    {
        if (!isInventoryOpen)
        {
            float horizontalInput = 0f;

            // Check if the left key is pressed
            if (Input.GetKeyDown(moveLeftKey))
            {
                leftKeyPressed = true;

                if (rightKeyPressed)
                {
                    rightKeyPressed = false;
                    rightKeyDormant = true;
                }
            }

            // Check if the right key is pressed
            if (Input.GetKeyDown(moveRightKey))
            {
                rightKeyPressed = true;

                if (leftKeyPressed)
                {
                    leftKeyPressed = false;
                    leftKeyDormant = true;
                }
            }

            // Check if the left key is released
            if (Input.GetKeyUp(moveLeftKey))
            {
                leftKeyPressed = false;

                if (rightKeyDormant)
                {
                    if (Input.GetKey(moveRightKey))
                    {
                        rightKeyDormant = false;
                        rightKeyPressed = true;
                    }

                }
            }

            // Check if the right key is released
            if (Input.GetKeyUp(moveRightKey))
            {
                rightKeyPressed = false;

                if (leftKeyDormant)
                {
                    if (Input.GetKey(moveLeftKey))
                    {
                        leftKeyDormant = false;
                        leftKeyPressed = true;
                    }
                }
            }

            // Determine horizontal input based on the key states
            if (leftKeyPressed && !rightKeyPressed)
            {
                horizontalInput = -1f; // Move left
            }
            else if (rightKeyPressed && !leftKeyPressed)
            {
                horizontalInput = 1f; // Move right
            }
            else
            {
                // No input or conflicting input
                horizontalInput = 0f;
            }

            // Apply horizontal input
            playerMovement.Move(horizontalInput);
        }
        else
        {
            // Inventory is open, stop player movement
            playerMovement.Move(0f);
        }

        if (GlobalInputMapping.activeInputMappings == GlobalInputMapping.inGameInputMapping)
            {
                if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Jump"]))
                {
                    playerJump.StartChargingJump();
                }

                else if (Input.GetKeyUp(GlobalInputMapping.activeInputMappings["Jump"]))
                {
                    playerJump.ReleaseJump();
                }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Shoot"]))
                {
                    Shoot.Shooting();
                }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Interact"]))
                {
                Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();

                if (playerCollider != null)
                {
                    Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(playerCollider.bounds.center, playerCollider.bounds.size, 0f);

                    foreach (Collider2D collider in overlappingColliders)
                    {
                        IInteractable interactable = collider.gameObject.GetComponent<IInteractable>();
                        if (interactable != null)
                        {
                            interactable.Interact();
                        }
                    }
                }
            }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Menu"]))
                {
                if (pauseMenu != null)
                {
                    pauseMenu.Menu();
                }
                else
                {
                    Debug.LogWarning("PauseMenu reference is null.");
                }
            }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Inventory"]))
                {
                  ToggleInventory();
                }
            }


            else if (GlobalInputMapping.activeInputMappings == GlobalInputMapping.inventoryInputMapping)
            {
                if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Inventory"]))
                {
                    ToggleInventory();
                }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["ScrollLeft"]))
                {
                    ScrollInventoryLeft();
                }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["ScrollRight"]))
                {
                    ScrollInventoryRight();
                }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Select"]))
                {
                    optionsPanelManager.ToggleOptionsPanel(!optionsPanelManager.isOptionsPanelActive);
                }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Menu"]))
                {
                    pauseMenu.Menu();
                }
            }
            else if (GlobalInputMapping.activeInputMappings == GlobalInputMapping.optionsInputMapping)
            {
                if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["ScrollUp"]))
                {
                    optionsPanelManager.ChangeSelectedIndex(-1);
                }
                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["ScrollDown"]))
                {
                    optionsPanelManager.ChangeSelectedIndex(1);
                }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Select"]))
                {
                    optionsPanelManager.SelectHighlightedItem();
                }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Deselect"]))
                {
                    optionsPanelManager.ToggleOptionsPanel(false);
                }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Menu"]))
                {
                     pauseMenu.Menu();
                }
                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Inventory"]))
                {
                    ToggleInventory();
                }
            }
            else if (GlobalInputMapping.activeInputMappings == GlobalInputMapping.menuInputMapping)
            {
                if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Menu"]))
                {
                 pauseMenu.Menu();
                }

                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["ScrollUp"]))
                {
                    pauseMenu.ChangeSelectedIndex(-1);
                }
                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["ScrollDown"]))
                {
                    pauseMenu.ChangeSelectedIndex(1);
                }
                else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Select"]))
                {
                    pauseMenu.MenuOptionSelect();
                }

        }

        }

        void ToggleInventory()
        {
            if (inventoryUIManager != null)
            {
                // Get the InventoryUIManager component
                InventoryUIManager uiManager = inventoryUIManager.GetComponent<InventoryUIManager>();

            if (isInventoryOpen)
            {
                uiManager.CloseInventory();
                GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.inGameInputMapping);
                isInventoryOpen = false;
                // Reset movement input
                playerMovement.Move(0f);
                leftKeyPressed = false;
                rightKeyPressed = false;
                leftKeyDormant = false;
                rightKeyDormant = false;
            }
            else
            {
                isInventoryOpen = true;
                playerMovement.Move(0f);
                uiManager.OpenInventory();
                GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.inventoryInputMapping);
            }
        }
            else
            {
                Debug.LogError("InventoryUIManager is not initialized.");
            }
        }

        void ScrollInventoryLeft()
        {
            if (inventoryUIManager != null)
            {
                // Get the InventoryUIManager component
                InventoryUIManager uiManager = inventoryUIManager.GetComponent<InventoryUIManager>();

                // Call the ScrollInventoryLeft function
                uiManager.ScrollInventoryLeft();
            }
        }

        void ScrollInventoryRight()
        {
            if (inventoryUIManager != null)
            {
                // Get the InventoryUIManager component
                InventoryUIManager uiManager = inventoryUIManager.GetComponent<InventoryUIManager>();

                // Call the ScrollInventoryRight function
                uiManager.ScrollInventoryRight();
            }
        }
    }

