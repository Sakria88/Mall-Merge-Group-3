using UnityEngine;

public class OutOfEnergyPanel : MonoBehaviour
{
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
