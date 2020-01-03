using UnityEngine;

[CreateAssetMenu(menuName="Variables/Float", fileName="New Float")]
public class FloatVariable : ScriptableObject
{
    public float value;

    public override string ToString()
    {
        return ((int) value).ToString();
    }
}
