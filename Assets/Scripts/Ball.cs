using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float splitAnimTime;
    public float splitAnimJumpHeight;
    public float maxY;
    public float maxYReductionPerSplit;
    public int baseScore = 50;
    public SessionManager sessionManager;
    public ParticleSystem bubbleBurst;
    private float speedX = 60f;
    private int ropeLayer;
    private Rigidbody2D body;
    private const int MAX_SPLITS = 3;
    private float sizeReductionPerSplit = 0.66f;

    private void Start()
    {
        ropeLayer = LayerMask.NameToLayer("Rope");
        body = GetComponent<Rigidbody2D>();
        body.AddForce(speedX * Vector2.right);
    }
    private void Update()
    {
        if (body.simulated && body.velocity.y > 0 && transform.position.y < maxY)
        {
            float distanceToMaxY = Mathf.Abs(transform.position.y - maxY);
            float desiredTimeToJumpPeak = Mathf.Sqrt(2f * distanceToMaxY / -Physics2D.gravity.y);
            float desiredVelocityY = (distanceToMaxY - Physics2D.gravity.y * desiredTimeToJumpPeak * desiredTimeToJumpPeak) / desiredTimeToJumpPeak;
            if (body.velocity.y != desiredVelocityY)
            {
                body.AddForce(Vector2.up * (desiredVelocityY - body.velocity.y));
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LayerMask colliderLayer = collision.gameObject.layer;
        if (colliderLayer == ropeLayer && body.simulated && gameObject.activeSelf)
        {
            StartCoroutine(Split());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LayerMask colliderLayer = collision.gameObject.layer;
        if (colliderLayer == ropeLayer && body.simulated && gameObject.activeSelf)
        {
            StartCoroutine(Split());
        }
    }

    private IEnumerator Split()
    {
        Instantiate(bubbleBurst, transform.position, transform.rotation);
        sessionManager.levelScore += (int)(transform.localScale.x* baseScore);
        UIMediator.current.SetScore(SessionManager.totalScore + sessionManager.levelScore);
        if (transform.localScale.x > Mathf.Pow(sizeReductionPerSplit, MAX_SPLITS))
        {
            GetComponent<Rigidbody2D>().simulated = false;
            GameObject rightBall = Instantiate(gameObject, transform.parent);
            GameObject leftBall = Instantiate(gameObject, transform.parent);
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.enabled = false;
            rightBall.transform.localScale *= sizeReductionPerSplit;
            leftBall.transform.localScale = rightBall.transform.localScale;
            float spaceBetween = sprite.bounds.max.x * 0.25f * leftBall.transform.localScale.x;
            leftBall.transform.position += spaceBetween * Vector3.right;
            rightBall.transform.position += spaceBetween * Vector3.left;
            float splitBallsMaxY = maxY * maxYReductionPerSplit;
            rightBall.GetComponent<Ball>().maxY = splitBallsMaxY;
            leftBall.GetComponent<Ball>().maxY = splitBallsMaxY;
            Rigidbody2D rightBallBody = rightBall.GetComponent<Rigidbody2D>();
            Rigidbody2D leftBallBody = leftBall.GetComponent<Rigidbody2D>();
            float horizontalDelta = speedX * splitAnimTime * Time.fixedDeltaTime;
            rightBall.transform.DOJump(rightBall.transform.position + horizontalDelta * Vector3.right, splitAnimJumpHeight, 1, splitAnimTime);
            yield return leftBall.transform.DOJump(leftBall.transform.position + horizontalDelta * Vector3.left, splitAnimJumpHeight, 1, splitAnimTime).WaitForCompletion();
            rightBallBody.simulated = leftBallBody.simulated = true;
            leftBallBody.AddForce(speedX * Vector2.left);
            rightBallBody.AddForce(speedX * Vector2.right);
        }
        gameObject.SetActive(false);
        Destroy(gameObject);
        if (sessionManager.IsLevelCompleted())
        {
            sessionManager.NextLevel();
        }
    }



}
