using System;
using UnityEngine;

public interface IMoveToDirection
{
    void MoveToDirection(Vector3 moveDirection);
    void Stop();
}