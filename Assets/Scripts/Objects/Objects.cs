using UnityEngine;
using System.Collections;

public enum ObjectType
{
    BARRICADE,
    TURRET,
    ZOMBIE,
}

public class Objects : MonoBehaviour {
    public ObjectType thisObjectType;
}
