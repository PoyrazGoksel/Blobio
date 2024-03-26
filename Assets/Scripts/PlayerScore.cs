using Events;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreTMP;
    private int _playerScore;
    
    private void OnEnable()
    {
        PlayerEvents.PlayerBaitConsume += OnPlayerBaitConsume;
    }

    private void OnPlayerBaitConsume()
    {
        _playerScore ++;

        _scoreTMP.text = "Score: " + _playerScore;
    }

    private void OnDisable()
    {
        PlayerEvents.PlayerBaitConsume -= OnPlayerBaitConsume;
    }
}