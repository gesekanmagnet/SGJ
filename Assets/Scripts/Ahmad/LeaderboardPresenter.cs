using Dan.Main;
using Dan.Models;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ProjectGS24.Module.Leaderboard
{
    public class LeaderboardPresenter : MonoBehaviour
    {
        [SerializeField] string publicKey;
        [SerializeField] LeaderboardItem template;
        [SerializeField] Transform itemContainer;
        [SerializeField] TMP_InputField usernameInputField;
        [SerializeField] UnityEvent onLoadComplete;

        Entry[] entries;

        private void Start()
        {

            if (PlayerPrefs.HasKey("Username"))
            {
                usernameInputField.text = PlayerPrefs.GetString("Username");
            }
            else
            {
                PlayerPrefs.SetString("Username", "-");
            }
            usernameInputField.onValueChanged.AddListener(SetUsername);

            LeaderboardCreator.GetLeaderboard(publicKey, (msg) => {
                onLoadComplete?.Invoke();
                entries = msg;
                int i = 1;
                foreach (var gameRecord in msg)
                {
                    GameObject obj = Instantiate(template.gameObject, itemContainer);
                    Color color = Color.white;
                    if (gameRecord.IsMine())
                        color = Color.yellow;
                    TimeSpan timeDifference = CalculateTimeDifference(gameRecord.Date);
                    obj.GetComponent<LeaderboardItem>().SetContent(i.ToString(), gameRecord.Username, $"{gameRecord.Score}", "", $"{FormatTimeAgo(timeDifference)}", color);
                    obj.SetActive(true);
                    i++;
                }
            });
        }

        // Method to calculate the time difference based on the timestamp.
        public static TimeSpan CalculateTimeDifference(ulong timestamp)
        {
            // Convert the ulong timestamp to DateTime.
            DateTimeOffset timestampDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)timestamp);
            DateTime timestampDate = timestampDateTimeOffset.DateTime;

            // Get the current date and time.
            DateTime currentDate = DateTime.UtcNow;

            // Return the time difference.
            return currentDate - timestampDate;
        }

        // Function to format time difference
        string FormatTimeAgo(TimeSpan timeDifference)
        {
            if (timeDifference.TotalDays >= 365)
                return $"{(int)(timeDifference.TotalDays / 365)} years ago";
            else if (timeDifference.TotalDays >= 30)
                return $"{(int)(timeDifference.TotalDays / 30)} months ago";
            else if (timeDifference.TotalDays >= 1)
                return $"{(int)timeDifference.TotalDays} days ago";
            else if (timeDifference.TotalHours >= 1)
                return $"{(int)timeDifference.TotalHours} hours ago";
            else if (timeDifference.TotalMinutes >= 1)
                return $"{(int)timeDifference.TotalMinutes} minutes ago";
            else
                return "Just now";
        }

        public void SetUsername(string username)
        {
            PlayerPrefs.SetString("Username", username);
        }

        public void SubmitUsername()
        {
            Leaderboards.ProjectSGJ2025.UploadNewEntry(PlayerPrefs.GetString("Username"), PlayerPrefs.GetInt("Highscore"), callback => {
                Debug.Log($"{usernameInputField.text} score has been uploaded");
            });

            SceneManager.LoadScene(0);
        }
    }
}