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
            transform.position += direction * Time.deltaTime * speed - new Vector3(0, fallSpeed * Time.deltaTime, 0);
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
            GetComponent<Animator>().SetTrigger("Used");
        }
    }

    public void SetEndPosition(PlayerMovement player) 
    { 
        direction = transform.forward; direction.y = 0;
        GetComponent<Damage>().playerRef = player;
    }
    private void SnapToGrid()
    {
        GetComponent<Animator>().SetTrigger("Landed");
        GetComponent<Damage>().dealtDamage = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
        transform.position = new Vector3(Mathf.Round(transform.position.x / 2f) * 2, 0, Mathf.Round(transform.position.z / 2) * 2);
        transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z + 0.5f);
    }
}
