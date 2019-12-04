using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Integrity : MonoBehaviour
{
    public int health = 10;
    public Texture2D healthbar;
    public string team;

    private float startHealth;

    //
    void Awake()
    {
        startHealth = health;
    }

    public void TakeDamage(int damage)
    {
        if (health > 0) {
            health -= damage;
            if (health <= 0) {
                SendMessage("Death"); //call the Death function on any script in this GameObject
            }
        }
    }

    //

    void OnGUI()
    {

        return;

        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);

        // Healthbar
        GUI.color = Color.red;
        GUI.DrawTexture(new Rect(pos.x - 10, Screen.height - pos.y - 25, startHealth , 4), healthbar);

        GUI.color = Color.green;
        GUI.DrawTexture(new Rect(pos.x - 10, Screen.height - pos.y - 25, health      , 4), healthbar);

    }
}
