using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings S;

    public int timePlayer;

    private void Awake()
    {
        S = this;
    }
}
