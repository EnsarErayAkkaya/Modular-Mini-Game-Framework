using System;
using UnityEngine;

public class DropdownAttribute : PropertyAttribute
{
    public Type SourceType { get; private set; }

    /// <summary>
    /// Pass a type that contains const or static readonly strings.
    /// </summary>
    public DropdownAttribute(Type sourceType)
    {
        SourceType = sourceType;
    }
}
