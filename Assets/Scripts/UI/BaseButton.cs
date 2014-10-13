using UnityEngine;
using System.Collections;

public class BaseButton : MonoBehaviour {
    public enum KeyType
    {
        BUILDOPTION,
        SKIP
    }
    public KeyType curKeyType;

    public Sprite buttonHover;
    public GameObject buttonHoverOverlay;
    public Sprite buttonClicked;
    public Sprite buttonNoEffect;

    SpriteRenderer spriteRenderer;


	// Use this for initialization
	public virtual void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}

    public virtual GameObject get_button_object()
    {
        return null;
    }

    public void activate_hover_overlay()
    {
        if (buttonHoverOverlay != null && buttonHoverOverlay.activeInHierarchy == false)
            buttonHoverOverlay.SetActive(true);
    }

    public void deactivate_hover_overlay()
    {
        if (buttonHoverOverlay != null)
            buttonHoverOverlay.SetActive(false);
    }

    public void hover_effect()
    {
        if (buttonHover != null)
            spriteRenderer.sprite = buttonHover;
    }

    public void selected_effect()
    {
        if (buttonClicked != null)
            spriteRenderer.sprite = buttonClicked;
    }

    public void no_effect()
    {
        if (buttonNoEffect != null)
        {
            spriteRenderer.sprite = buttonNoEffect;
        }
        if (buttonHoverOverlay != null)
        {
            buttonHoverOverlay.SetActive(false);
        }
    }
}
