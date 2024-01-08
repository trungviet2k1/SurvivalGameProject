using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance {  get; set; }
    public bool onTarget;
    public GameObject selectedObject;
    public GameObject interaction_info_UI;
    public Image centerDotImage;
    public Image handIcon;
    Text interaction_Text;

    void Start()
    {
        onTarget = false;
        interaction_Text = interaction_info_UI.GetComponent<Text>();
    }

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

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interacable = selectionTransform.GetComponent<InteractableObject>();

            if (interacable && interacable.playerInRange)
            {
                onTarget = true;
                selectedObject = interacable.gameObject;
                interaction_Text.text = interacable.GetItemName();
                interaction_info_UI.SetActive(true);


                if (interacable.CompareTag("PickAble"))
                {
                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                }
                else
                {
                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                }
            }
            else
            {
                onTarget = false;
                interaction_info_UI.SetActive(false);
                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            onTarget = false;
            interaction_info_UI.SetActive(false);
            centerDotImage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
        }
    }
}
