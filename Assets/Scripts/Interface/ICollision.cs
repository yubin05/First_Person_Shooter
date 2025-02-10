using System;
using UnityEngine;

public interface ICollision
{
    public void OnCollisionEnter(Collision other);
    public void OnCollisionExit(Collision other);
}

public interface ICollider
{
    public void OnTriggerEnter(Collider other);
    public void OnTriggerExit(Collider other);
}

public interface ICollision2D
{
    public void OnCollisionEnter2D(Collision2D other);
    public void OnCollisionExit2D(Collision2D other);
}

public interface ICollider2D
{
    public void OnTriggerEnter2D(Collider2D other);
    public void OnTriggerExit2D(Collider2D other);
}
