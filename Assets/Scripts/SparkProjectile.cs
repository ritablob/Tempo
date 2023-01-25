using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkProjectile : MonoBehaviour
{
    [SerializeField] float resistance;
    [SerializeField] float speed;
    [SerializeField] Damage dmgScript;

    private Vector3 direction;
    private bool isTrap;

    void Update()
    {
        if(speed <= 0)
        {
            GetComponent<Animator>().SetTrigger("Spark");
            isTrap = true;
        }

        if (!isTrap)
        {
            transform.position += direction * Time.deltaTime * speed;
            speed -= resistance * Time.deltaTime;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != dmgScript.playerRef.gameObject && other.tag == "Player" && isTrap)
        {
            GetComponent<Animator>().SetTrigger("Spark");
            return;
        }
        if (other.gameObject == dmgScript.playerRef.gameObject && other.tag == "Player" && isTrap)
        {
            dmgScript.playerRef.GainSpecial(this.gameObject);
        }
    }

    public void SetEndPosition(PlayerMovement player)
    {
        direction = transform.forward; direction.y = 0;
        dmgScript.playerRef = player;
    }
}
