using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    private Animator anim;
    private GameObject magic_Circle;
    public ParticleSystem warningStrike;   // The indicator (parent particle)
    public ParticleSystem lightning;
    public GameObject player;
    private float lightningSpawnRadius = 2f;
    private  int strikes = 2;
    private AudioScript audioManager;
    private MonsterScript monster;
    private GameObject  attackerObj;
    private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioScript>();
    }
    public void Attack(GameObject attacker){
        attackerObj = attacker;
        Debug.Log("The player trying to attack is " + attacker);
        if(attacker.tag == "Wizard"){
            magic_Circle = FindChildWithTag(attacker.transform, "Magic Circle");
            magic_Circle.SetActive(true);
            anim = attacker.GetComponent<Animator>();
            anim.SetTrigger("Attack1");
            Debug.Log("WizardDebug: attack animation and set and magic circle set starting attac.");
            // StartCoroutine(DeactivateMagicCircleAfterAnimation(attacker));
            StartCoroutine(PlayLightningSequence());

            
        }
        else if(attacker.tag == "Skeleton"){
            anim = attacker.GetComponent<Animator>();
            anim.SetTrigger("Attack1");
            monster = attackerObj.GetComponent<MonsterScript>();
            monster.AttackCooldown();

        }
    }
    private GameObject FindChildWithTag(Transform parentTransform, string tag)
    {
        foreach (Transform child in parentTransform)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject; 
            }
        }
        return null; 
    }
    // private IEnumerator DeactivateMagicCircleAfterAnimation(GameObject attacker)
    // {
    //     yield return new WaitForSeconds(1);
    //     magic_Circle.SetActive(false);
    // }

//      private IEnumerator PlayLightningSequence()
// {
//     for (int i = 0; i < strikes; i++) // Loop for the number of strikes
//     {
//         // Get a random position within the radius around the player
//         Vector3 randomPosition = GetRandomPosition(player.transform.position, lightningSpawnRadius);

//         // Move the particle systems to the new position
//         warningStrike.transform.position = randomPosition;
//         lightning.transform.position = randomPosition;

//         // Play the warning particle system
//         warningStrike.Play();
//         yield return new WaitForSeconds(2f); // Duration of the warning

//         // Play the lightning strike
//         audioManager.playSFX(audioManager.lightninSound);
//         lightning.Play();
//         yield return new WaitForSeconds(lightning.main.duration); // Wait for the lightning effect duration

//         // Stop the warning particle system
//         warningStrike.Stop();
//     }
// }
private IEnumerator PlayLightningSequence(){
    Debug.Log("WizardDebug: Lighting sequence was called.");
    List<Vector3> strikePositions = new List<Vector3>(); // Store the generated positions

    for (int i = 0; i < strikes; i++) // Loop for the number of strikes
    {
        // Get a random position within the radius around the player
        Vector3 randomPosition = GetRandomPosition(player.transform.position, lightningSpawnRadius);
        strikePositions.Add(randomPosition); // Save this position for later checks

        // Move the particle systems to the new position
        warningStrike.transform.position = randomPosition;
        lightning.transform.position = randomPosition;

        // Play the warning particle system
        Debug.Log("WizardDebug: circle warnning spawing");
        warningStrike.Play();
        yield return new WaitForSeconds(2f); // Duration of the warning
        // Play the lightning strike
        audioManager.playSFX(audioManager.lightninSound);
        lightning.Play();
        // yield return new WaitForSeconds(lightning.main.duration); 
        // Stop the warning particle system
        if (IsPlayerInStrikeZone(player.transform.position, randomPosition, 3f)) // Margin of 1 unit
        {
            CollisionDetectionScript playerCollision = player.GetComponent<CollisionDetectionScript>();
            playerCollision.LightningHit();
        }
        yield return new WaitForSeconds(lightning.main.duration);
        
    }
    magic_Circle.SetActive(false);
    monster = attackerObj.GetComponent<MonsterScript>();
    monster.AttackCooldown();
    
}

private bool IsPlayerInStrikeZone(Vector3 playerPosition, Vector3 strikePosition, float radius)
{
    return Vector3.Distance(playerPosition, strikePosition) <= radius;
}
private Vector3 GetRandomPosition(Vector3 center, float radius)
{
    Vector2 randomCircle = Random.insideUnitCircle * radius; // Random point in a 2D circle
    Vector3 randomPosition = new Vector3(center.x + randomCircle.x, center.y, center.z + randomCircle.y); // Convert to 3D
    return randomPosition;
}

}
