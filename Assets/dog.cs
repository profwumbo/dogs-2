using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static worlditems;

public class dog : MonoBehaviour
{
    public sun sun;
    public enum DogNeed
    {
        None,
        Water,
        Food,
        Sleep
    }
    public DogNeed currentNeed;
    public float needTimer = 30f;

    public AudioSource barkSource;
    public AudioClip barkClip;

    public NavMeshAgent agent;
    public Transform doghouse;

    private bool alive = true;

    void Start()
    {
        PickNewNeed();
    }

    void Update()
    {
        if (!alive) return;

        needTimer -= Time.deltaTime;
        if (needTimer <= 0)
        {
            Die();
        }
    }

    void PickNewNeed()
    {
        currentNeed = (DogNeed)Random.Range(1, 4);
        needTimer = 30f;
        StartCoroutine(BarkRoutine());
    }

    IEnumerator BarkRoutine()
    {
        int barkCount = (int)currentNeed;

        for (int i = 0; i < barkCount; i++)
        {
            barkSource.PlayOneShot(barkClip);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Interact(PlayerInteract player)
    {
        if (!alive) return;
        if (player.heldItem == ItemType.None) return;

        if (CorrectItem(player.heldItem))
        {
            HandleNeed();
            player.ClearItem();
            PickNewNeed();
        }
        else
        {
            barkSource.PlayOneShot(barkClip); // confused bark
        }
    }

    bool CorrectItem(ItemType item)
    {
        return (currentNeed == DogNeed.Water && item == ItemType.Water)
            || (currentNeed == DogNeed.Food && item == ItemType.Food)
            || (currentNeed == DogNeed.Sleep && item == ItemType.Sleep);
    }

    void HandleNeed()
    {
        if (currentNeed == DogNeed.Sleep)
        {
            agent.SetDestination(doghouse.position);
        }
        // Food & water handled visually/anim-wise
    }

    void Die()
    {
        alive = false;
        sun.OnDogDeath();
        Debug.Log("Dog has died");
    }
  
}
