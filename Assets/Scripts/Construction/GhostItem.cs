using UnityEngine;

public class GhostItem : MonoBehaviour
{
    [Header("Box collider")]
    public BoxCollider solidCollider;

    [Header("Mesh renderer")]
    public Renderer mRenderer;

    private Material semiTransparentMaterial;
    private Material selectedMaterial;
    private ConstructionManager constructionManager;

    [HideInInspector] public bool isPlaced;
    [HideInInspector] public bool hasSamePosition = false;

    void Awake()
    {
        constructionManager = ConstructionManager.Instance;
    }

    void Start()
    {
        mRenderer = GetComponent<Renderer>();
        semiTransparentMaterial = constructionManager.ghostSemiTransparentMaterial;
        selectedMaterial = constructionManager.ghostSelectedMaterial;

        mRenderer.material = semiTransparentMaterial;
        solidCollider.enabled = false;
    }

    void Update()
    {
        if (constructionManager.inConstrucionMode)
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), constructionManager.player.GetComponent<Collider>());
        }

        if (constructionManager.inConstrucionMode && isPlaced)
        {
            solidCollider.enabled = true;
        }

        if (!constructionManager.inConstrucionMode)
        {
            solidCollider.enabled = false;
        }

        if (constructionManager.selectedGhost == gameObject)
        {
            mRenderer.material = selectedMaterial;
        }
        else
        {
            mRenderer.material = semiTransparentMaterial;
        }
    }
}