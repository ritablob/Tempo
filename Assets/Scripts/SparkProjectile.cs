using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkProjectile : MonoBehaviour
{
    [SerializeField] float resistance;
    [SerializeField] float speed;
    [SerializeField] float fallSpeed;
    [SerializeField] float travelTime;
    [SerializeField] Damage dmgScript;

    private Vector3 direction;
    private bool isTrap;

    void Update()
    {
        if(speed < 0)
        {
            GetComponent<Animator>().SetTrigger("Spark");
            SoundPlayer.PlaySound(1, "riven_trap_stop");
            SoundPlayer.PlaySound(1, "riven_trap_activate");
            isTrap = true;
            speed = 0;
            return;
        }

        if (!isTrap)
        {
            transform.position += direction * Time.deltaTime * speed;
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y - (Time.deltaTime * fallSpeed), 0.25f, 9), transform.position.z);

            if (travelTime > 0)
            {
                travelTime -= Time.deltaTime;
                return;
            }

            speed -= resistance * Time.deltaTime;
            fallSpeed = Mathf.Clamp(fallSpeed - (resistance * (Time.deltaTime / 2)), 0, 999);
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
            GetComponent<SoundEffectPlayer>().PlaySound("riven trap pickup");
            dmgScript.playerRef.GainSpecial(this.gameObject);
        }
    }

    public void SetEndPosition(PlayerMovement player)
    {
        direction = transform.forward; direction.y = 0;
        dmgScript.playerRef = player;
    }
}
