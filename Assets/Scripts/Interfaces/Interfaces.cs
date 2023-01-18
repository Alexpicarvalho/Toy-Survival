using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(int amount);
    Transform GetTransform();
}

public interface IKillable
{
    void Kill();
}

public interface IInteractable
{
    void Interact();
}

public interface IGatherable
{
    void Gather();
}
public interface IThrowable
{
    void Throw();
}