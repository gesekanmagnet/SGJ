using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGS24.Module.Leaderboard
{
    public class LeaderboardItem : MonoBehaviour
    {
        [SerializeField] TMP_Text rankText;
        [SerializeField] TMP_Text usernameText;
        [SerializeField] TMP_Text scoreText;
        [SerializeField] TMP_Text badgeText;
        [SerializeField] TMP_Text dateText;
        [SerializeField] Image background;

        public void SetContent(string rank, string username, string score, string badge, string date, Color backgroundColor)
        {
            rankText.SetText(rank);
            usernameText.SetText(username);
            scoreText.SetText(score);
            badgeText.SetText(badge);
            dateText.SetText(date);
            background.color = backgroundColor;
        }
    }
}