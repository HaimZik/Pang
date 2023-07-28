using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject movementArea;
    public GameObject ropePrefab;
    public float ropeReloadTime = 1f;
    public SessionManager sessionManager;
    public ParticleSystem bloodExplosion;
    private Bounds movementAreaBounds;
    private SpriteRenderer spriteRenderer;
    private int availableRopes;
    private int ballsLayer;
    private bool isAlive;


    private void Start()
    {
        isAlive = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementAreaBounds = movementArea.GetComponent<SpriteMask>().bounds;
        movementAreaBounds.Expand(-Vector3.right * spriteRenderer.bounds.max.x * transform.localScale.x * 0.5f);
        availableRopes = 1;
        ballsLayer = LayerMask.NameToLayer("Balls");
        UIMediator.current.RefreshLivesView();
    }

    public void Move(float axisX)
    {
        Vector3 newPos = transform.position + speed * axisX * Vector3.right;
        if (isAlive && movementAreaBounds.Contains(newPos))
        {
            transform.position = newPos;
        }
    }

    public void LunchRope()
    {
        if (availableRopes == 0 || !isAlive)
        {
            return;
        }
        GameObject rope = Instantiate(ropePrefab, transform.position - spriteRenderer.bounds.max.y * Vector3.down, Quaternion.identity);
        availableRopes--;
        rope.GetComponentInChildren<Rope>().OnHit = OnRopeHit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ballsLayer && isAlive)
        {
            isAlive = false;
            Destroy(collision.gameObject);
            Vector3 bloodPos = (collision.transform.position+transform.position)*0.5f;
            Instantiate(bloodExplosion, bloodPos, transform.rotation, transform.parent);
            StartCoroutine(OnPlayerDeath());
        }
    }

    private IEnumerator OnPlayerDeath()
    {
        yield return new WaitForSeconds(0.6f);
        Time.timeScale = 0.08f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1f;
        spriteRenderer.enabled = false;
        SessionManager.Lives--;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Time.timeScale = 1f;
        if (SessionManager.Lives > 0)
        {
            sessionManager.ResetLevel();
        }
        else
        {
            sessionManager.EndSession();
        }
    }

    public void OnRopeHit()
    {
        availableRopes++;
    }

}
