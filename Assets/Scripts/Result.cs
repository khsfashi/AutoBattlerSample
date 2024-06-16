using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject demonVictory;
    public GameObject demonDefeat;

    public void Victory()
    {
        demonVictory.SetActive(true);
    }

    public void Defeat()
    {
        demonDefeat.SetActive(true);
    }

    public void CloseAll()
    {
        demonVictory.SetActive(false);
        demonDefeat.SetActive(false);
    }
}
