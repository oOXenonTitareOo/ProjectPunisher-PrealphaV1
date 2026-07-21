using UnityEngine;

public class WeaponShellEjection : MonoBehaviour
{
    private Gun gun;
    public Transform shellSpawnPoint;
    private Transform shellParent;
    public float shellEjectionForce = 0.5f;
    public float shellSpin = 0.001f;
    public GameObject Shell;
    private Vector3 randomSpin;
    private Vector3 baseEjectionDirection;
    private Vector3 finalEjectionDirection;
    private Rigidbody shellRb;
    public string targetGroupName = "GROUP-PLAYER_SHELLS";
    void Start()
    {
        gun = GetComponent<Gun>();
        FindShellParent();
    }
    public void ChangeShellGroup(string newGroupName)
    {
        targetGroupName = newGroupName;
        FindShellParent();
    }
    private void FindShellParent()
    {
        GameObject parentObj = GameObject.Find(targetGroupName);
        if (parentObj != null)
        {
            shellParent = parentObj.transform;
        }
    }
    public void EjectShell()
    {
        GameObject shell = Instantiate(Shell, shellSpawnPoint.position, shellSpawnPoint.rotation, shellParent);
        shellRb = shell.GetComponent<Rigidbody>();

        if (gun != null && gun.playerRb != null)
        {
            shellRb.linearVelocity = gun.playerRb.linearVelocity;
        }

        baseEjectionDirection = shellSpawnPoint.right * 4f;
        
        randomSpin = new Vector3(
            0f,
            Random.Range(0.001f, 0.2f),
            Random.Range(0.001f, 0.2f)
        );

        finalEjectionDirection = baseEjectionDirection + randomSpin;

        float randomForce = shellEjectionForce * Random.Range(0.1f, 0.5f);
        shellRb.AddForce(finalEjectionDirection.normalized * randomForce, ForceMode.Impulse);

        shellRb.AddTorque(new Vector3(
            Random.Range(-shellSpin, shellSpin),
            Random.Range(-shellSpin, shellSpin),
            Random.Range(-shellSpin, shellSpin)
        ), ForceMode.Impulse);

        Destroy(shell, 10f);
    }

}
