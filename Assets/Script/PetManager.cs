using UnityEngine;
using System.Collections.Generic;

public class PetManager : MonoBehaviour
{
    public static PetManager Instance;
    public GameObject petPrefab;
    public float respawnDelay = 5f;

    private List<GameObject> pets = new List<GameObject>();
    private Transform player;
    private float timer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        timer = respawnDelay;
    }

    void Update()
    {
        if (player == null)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            GameObject newPet = Instantiate(petPrefab, player.position + Vector3.right, Quaternion.identity);
            newPet.GetComponent<Pet>().player = player;
            pets.Add(newPet);

            timer = respawnDelay;
        }
    }

    public void ResetPet()
    {
        foreach (var pet in pets)
        {
            if (pet != null)
                Destroy(pet);
        }

        pets.Clear();
        timer = respawnDelay;

        Debug.Log("Player hit - resetting all pets and timer.");
    }

    public void RemovePet(GameObject pet)
    {
        if (pets.Contains(pet))
            pets.Remove(pet);
    }
}
