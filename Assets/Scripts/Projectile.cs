using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damageAsTrap;
    [SerializeField] float speed;
    [SerializeField] float travelTime;
    public GameObject knife;
    public GameObject trap;

    private bool isTrap;
    private float time; 

    void Update()
    {
        if (time >= travelTime)
        {
            time = 0;
            isTrap = true;
            knife.SetActive(false);
            trap.SetActive(true);
            GetComponent<Damage>().isTrap = true;
            GetComponent<Damage>().dealtDamage = false;
            GetComponent<Damage>().baseDamage = damageAsTrap;
            SnapToGrid();
        }

        if (!isTrap)
        {
            transform.localScale = new Vector3(0.7972631f, 0.7972631f, 0.7972631f);
            time += Time.deltaTime;
            transform.Translate((Vector3.forward * Time.deltaTime * speed) - (new Vector3(0, Time.deltaTime / 2, 0)));
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3, 3), transform.position.y, Mathf.Clamp(transform.position.z, -2, 2));
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != GetComponent<Damage>().playerRef.gameObject && other.tag == "Player" && isTrap)
        {
            GetComponent<Animator>().SetTrigger("Used");
            return;
        }
        if(other.gameObject == GetComponent<Damage>().playerRef.gameObject && other.tag == "Player" && isTrap)
        {
            GetComponent<Damage>().playerRef.GainSpecial(this.gameObject);
            GetComponent<SoundEffectPlayer>().PlaySound("nova trap pickup");
        }
    }
    public void SetEndPosition(PlayerMovement player) 
    {
        GetComponent<Damage>().playerRef = player;
    }
    private void SnapToGrid()
    {
        GetComponent<SoundEffectPlayer>().PlaySound("nova trap land");
        GetComponent<Animator>().SetTrigger("Landed");
        GetComponent<Damage>().dealtDamage = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3, 3), 0, Mathf.Clamp(transform.position.z, -2, 2));
    }
}
