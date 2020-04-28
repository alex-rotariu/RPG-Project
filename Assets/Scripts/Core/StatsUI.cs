using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RPG.Core;
using RPG.Combat;
using System.Text;

public class StatsUI : MonoBehaviour
{
    private TextMeshProUGUI tmpText;
    private string upperRow = "Viata jucator: ";
    private string lowerRow = "Viata inamic: ";
    private GameObject player;

    private void Start() {
        tmpText = GetComponent<TextMeshProUGUI>();
        player = GameObject.FindWithTag("Player");
    }

    void Update() {
        var sb = new StringBuilder();
        sb.Append(upperRow);
        var playerHealth = player.GetComponent<Health>().getHealth();
        sb.Append(playerHealth[0] + "/" + playerHealth[1]);
        sb.AppendLine();
        sb.Append(lowerRow);
        var enemyHealth = player.GetComponent<Fighter>().GetEnemyHealth();
        if (enemyHealth != null) {
            sb.Append(enemyHealth[0] + "/" + enemyHealth[1]);
        }     
        tmpText.text = sb.ToString();
    }
}
