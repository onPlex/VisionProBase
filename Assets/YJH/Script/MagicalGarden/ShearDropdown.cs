using UnityEngine;

public class ShearDropdown : MonoBehaviour
{
   [SerializeField]
    GameObject AnimObj;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropDownObj"))
        {           
            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            AnimObj.SetActive(true);
        }
    }
}
