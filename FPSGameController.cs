using UnityEngine;

/// 
/// FPSGameController
/// This script is attached to an empty GameObject called "Controller".
/// 
/// It adds a GUI to the game to display the player's score,
/// and provides a public method to add points to the score.
/// 
public class FPSGameController : MonoBehaviour
{
    public GameObject targetPrefab;
    private int score = 0;

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Score: " + score);
    }

    public void AddPoints(int points)
    {
        score += points;
    }
}
