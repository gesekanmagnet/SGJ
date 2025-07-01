using UnityEngine;

public enum PartType { A, B, C }

public abstract class Part : MonoBehaviour
{
    public PartType type;
}