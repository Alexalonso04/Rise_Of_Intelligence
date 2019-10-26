using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stat", menuName = "Player Stats")]
public class Player_Stats: ScriptableObject {
    public Transform playerPosition;
    public float health;
}
