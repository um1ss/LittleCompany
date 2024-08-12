using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Bucket : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 20;
    [SerializeField] private SpriteRenderer _waterSprite;
    [SerializeField] private DecalProjector _projector;
    [SerializeField] private Material _bucketSpotMat;

    private BoxCollider _boxCollider;
    private int _health;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        _health = _maxHealth;
        _bucketSpotMat.color = Color.black;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BucketWater"))
        {
            _boxCollider.enabled = false;
            _waterSprite.color = new Color(0,0, 0, 0);
            Destroy(other.gameObject);
        }
    }
    public Color TakeDirt(int dirt, Color dirtColor)
    {
        if(dirtColor == Color.black) { return Color.black; }

        Color sumColor = _waterSprite.color;
        float healFactor = (_maxHealth - _health) / (float)_maxHealth;
        sumColor.r = Mathf.LerpUnclamped(sumColor.r, dirtColor.r, healFactor);
        sumColor.g = Mathf.LerpUnclamped(sumColor.g, dirtColor.g, healFactor);
        sumColor.b = Mathf.LerpUnclamped(sumColor.b, dirtColor.b, healFactor);

        _waterSprite.color = sumColor;
        _health -= dirt;

        _bucketSpotMat.color = sumColor;
        _projector.fadeFactor = healFactor;
        if (_health < 0)
        {
            _health = 0;
            return _waterSprite.color;
        }
        return Color.black;
    }
}
