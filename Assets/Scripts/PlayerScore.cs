using Events;
using Slimes;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private const string ScoreTxt = "Score: ";
    [SerializeField] private TextMeshProUGUI _scoreTMP;

    private void Awake()
    {
        _scoreTMP.text = ScoreTxt + 0;
    }

    private void OnEnable()
    {
        PlayerEvents.PlayerBaitConsume += OnPlayerBaitConsume;
    }

    private void OnPlayerBaitConsume(int playerScore)
    {
        _scoreTMP.text = ScoreTxt + playerScore;
    }

    private void OnDisable()
    {
        PlayerEvents.PlayerBaitConsume -= OnPlayerBaitConsume;
    }
}