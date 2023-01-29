using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    [SerializeField] float speed;

    private Vector3 direction;
    private bool isTrap;

    void Update()
    {
        if (transform.position.y < 0)
        {
            isTrap = true;
            SnapToGrid();
        }

        if (!isTrap)
        {
            transform.localScale = new Vector3(0.7972631f, 0.7972631f, 0.7972631f);
            transform.position += direction * Time.deltaTime * speed - new Vector3(0, fallSpeed * Time.deltaTime, 0);
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
            GetComponent<SoundEffectPlayer>().PlaySound("nova trap pickup");
            GetComponent<Damage>().playerRef.GainSpecial(this.gameObject);
        }
    }

    public void SetEndPosition(PlayerMovement player) 
    { 
        direction = transform.forward; direction.y = 0;
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
