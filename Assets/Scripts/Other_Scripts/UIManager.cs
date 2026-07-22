using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject hitUI;
    private void Awake()
    {
        Instance = this;
    }
    public void InstantiateHitUI()
    {
        Instantiate(hitUI, transform);
    }
}
