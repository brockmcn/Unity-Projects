using UnityEngine;

public class HUDTriggerTest : MonoBehaviour
{
    void Start()
    {
        HUD.OnInitialized += ((sender, e) =>
        {
            HUD.DisplayTitle("Launch game", "Mission completed");
            HUD.SyncExperience(1000, 100, 1);
            HUD.SetPlayerStatusMax(100, 100);
            HUD.SetPlayerHealth(100);
            HUD.SetPlayerStamina(100);
            HUD.ShowPlayerStatus();
            HUD.DisplayExperience();
        });
        HUD.Load();
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            HUD.DisplayTitle("Hi chief!", "How do you like this new HUD system?");
        }
    }*/
}
