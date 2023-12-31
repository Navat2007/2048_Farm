using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private Image _image;
    
    public TileState state { get; private set; }
    public TileCell cell { get; private set; }

    public int number { get; private set; }
    public bool locked { get; set; }

    private Image _background;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        // _background = GetComponent<Image>();
        // _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetState(TileState state, int number)
    {
        this.state = state;
        this.number = number;

        // _background.color = state.backgroundColor;
        // _text.color = state.textColor;
        // _text.text = number.ToString();
        _image.sprite = state.sprite;
        
        _image.gameObject.SetActive(false);

        if (state.sprite != null)
        {
            _image.gameObject.SetActive(true);
        }
    }

    public void Spawn(TileCell cell)
    {
        if (this.cell != null) {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    public void MoveTo(TileCell cell)
    {
        if (this.cell != null) {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        StartCoroutine(Animate(cell.transform.position, false));
    }

    public void Merge(TileCell cell)
    {
        if (this.cell != null) {
            this.cell.tile = null;
        }

        this.cell = null;
        cell.tile.locked = true;

        StartCoroutine(Animate(cell.transform.position, true));
    }

    private IEnumerator Animate(Vector3 to, bool merging)
    {
        float elapsed = 0f;
        float duration = 0.2f;

        Vector3 from = transform.position;

        while (elapsed < 1)
        {
            elapsed += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0, 1, elapsed));
            
            // transform.position = Vector3.Lerp(from, to, elapsed / duration);
            // elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging) {
            Destroy(gameObject);
        }
    }
}
