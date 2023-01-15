using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownBox : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropdown;
    public static string currentName;
    void Start()
    {
        dropdown = GameObject.Find("NameDropdown").GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(delegate { SelectName(dropdown); });     

    }

    public void SelectName(Dropdown dropdown)
    {
        int index = dropdown.value;
        currentName = dropdown.options[index].text;
    }
}
