using UnityEngine;

// Credit to https://answers.unity.com/users/661755/sarahnorthway.html
public sealed class GetSetAttribute : PropertyAttribute
{
    public readonly string name;
    public bool dirty;

    public GetSetAttribute(string name)
    {
        this.name = name;
    }
}