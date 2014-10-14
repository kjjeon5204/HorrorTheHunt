using UnityEngine;
using System.Collections;

public class BuildOption : BaseButton {
    public GameObject buildObject;
    public int priceReq;
    public BuildLogic myBuildLogic;
    public SpriteRenderer mySprite;
    public Sprite locked;
    public Sprite available;



    public override GameObject get_button_object()
    {
        return buildObject;
    }

    void Update()
    {
        if (myBuildLogic.get_remaining_money() >= priceReq)
        {
            mySprite.sprite = available;
        }
        else
        {
            mySprite.sprite = locked;
        }
    }
}
