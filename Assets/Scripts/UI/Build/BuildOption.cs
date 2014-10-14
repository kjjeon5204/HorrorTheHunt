using UnityEngine;
using System.Collections;

public class BuildOption : BaseButton {
    public GameObject buildObject;
    public int priceReq;

    public override GameObject get_button_object()
    {
        return buildObject;
    }
}
