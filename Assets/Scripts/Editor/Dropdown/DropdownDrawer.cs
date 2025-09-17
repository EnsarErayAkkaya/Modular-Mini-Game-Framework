using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EEA.Edtor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class StringDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var dropdown = (DropdownAttribute)attribute;
            var type = dropdown.SourceType;

            string[] options = GetStringConstants(type);

            if (property.propertyType == SerializedPropertyType.String)
            {
                int index = Mathf.Max(0, Array.IndexOf(options, property.stringValue));
                index = EditorGUI.Popup(position, label.text, index, options);

                if (options.Length > 0)
                    property.stringValue = options[index];
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [StringDropdown] with string.");
            }
        }

        private string[] GetStringConstants(Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string)) // const
                .Select(f => (string)f.GetRawConstantValue())
                .ToArray();
        }
    }
}