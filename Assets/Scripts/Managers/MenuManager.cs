using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; set; }
    public GameObject menuCanvas;
    public GameObject uiCanvas;

    public GameObject saveMenu;
    public GameObject settingMenu;
    public GameObject menu;

    public bool isMenuOpen;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isMenuOpen)
        {
            uiCanvas.SetActive(false);
            menuCanvas.SetActive(true);

            isMenuOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isMenuOpen)
        {
            saveMenu.SetActive(false);
            settingMenu.SetActive(false);
            menu.SetActive(true);

            uiCanvas.SetActive(true);
            menuCanvas.SetActive(false);

            isMenuOpen = false;
            if (CraftingSystem.Instance.isOpen == false && InventorySystem.Instance.isOpen == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; 
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }
}