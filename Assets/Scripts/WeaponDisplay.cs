using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] Sprite[] weaponImages;

    public void WeaponDisplayChanger()
    {
       int weaponSelected = FindObjectOfType<PlayerController>().GetWeaponSelected();
       GetComponent<Image>().sprite = weaponImages[weaponSelected];

    }

}
