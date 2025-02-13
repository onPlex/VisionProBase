using UnityEngine;

public class TrowelDropdown : MonoBehaviour
{
    [SerializeField]
    GameObject TrowelAnimObj;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DropDownObj"))
        {
            Debug.Log("Test Dropdown");
            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            TrowelAnimObj.SetActive(true);
        }
    }
}
