using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : PrefabsController
{
    private float speed = 3.5f;
    private float timer = 0.0f;
    private float timeRang = 0.1f;

    public Coroutine moveCoroutine;
    private Vector3 startPosition;

    public void ZeroGravity()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }
    public void UseGravity()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
    protected bool IsNear(Vector3 targPos)
    {
        return (transform.position - targPos).sqrMagnitude < .01f;
    }


    protected void MoveBy(Vector3 newPosition, Action action)
    {
        StopMoveCoroutine();
        StartCoroutine(MoveUp(newPosition, action));
    }

    protected void MovePiece(Square square, Action action)
    {
        StopMoveCoroutine();
        StartCoroutine(MoveFor(square, action));
    }
    protected void MaximumEatMove(List<Square> list , Action action)
    {
        StopMoveCoroutine();
        StartCoroutine(MaxEatMove(list, action));
    }
   protected void EatPiece(GameObject piece)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleOut());
    }
    protected void StopMoveCoroutine()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
    }
    protected IEnumerator MoveFor(Square square, Action action)
    {
        //StopMoveCoroutine();
        ready = false;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = square.transform.position;
        targetPosition.y = startPosition.y;
        while (true)
        {
            if (IsNear(targetPosition))
            {
                ready = true;
                if (action != null)
                {
                    action();

                }
                yield break;
            }
            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 0.4974762f,transform.position.z)
                , targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
    protected IEnumerator MaxEatMove(List<Square> sqs, Action action)
    {
        ready = false;
        Vector3 startPosition;
        Vector3 targetPosition;
        int moves = sqs.Count;
        for(int i = 0; i < moves; i++)
        {
            if (i == 0)
            {
                startPosition = transform.position;
                targetPosition = sqs[i].transform.position;
                targetPosition.y = startPosition.y;
            }
            else
            {
                startPosition = sqs[i - 1].transform.position;
                targetPosition = sqs[i].transform.position;
                targetPosition.y = startPosition.y + 0.4974762f;
                
            }
            if (i != moves - 1)
            {
                while (true)
                {
                    if (IsNear(targetPosition))
                    {
                        break;
                    }
                    transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 0.4974762f, transform.position.z), targetPosition, speed * Time.deltaTime);
                    yield return null;
                }
            }
            else
            {
                while (true)
                {
                    if (IsNear(targetPosition))
                    {
                        ready = true;
                        if (action != null)
                        {
                            action();
                        }
                        yield break;
                    }
                    transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 0.4974762f, transform.position.z), targetPosition, speed * Time.deltaTime);
                    yield return null;
                }
            }
        }
        
    }
    protected IEnumerator MoveUp(Vector3 newPosition, Action action)
    {
        ready = false;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + newPosition;

        while (true)
        {
            if (IsNear(targetPosition))
            {
                ready = true;
                if (action != null)
                {
                    action();
                    
                }
                yield break;

            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
    public void Shake()
    {
        startPosition = transform.position;
        startPosition.y = 0.45f;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        StartCoroutine(StartShaking());
    }
    protected IEnumerator StartShaking()
    {
        timer = 0.0f;
        while(timer< timeRang)
        {
            timer += Time.deltaTime;
            Vector3 randomRange = new Vector3(UnityEngine.Random.insideUnitSphere.x, startPosition.y, UnityEngine.Random.insideUnitSphere.z);
            transform.position = startPosition + UnityEngine.Random.insideUnitSphere * 0.05f;
            yield return null;
        }
        transform.position = startPosition;
    }
}
