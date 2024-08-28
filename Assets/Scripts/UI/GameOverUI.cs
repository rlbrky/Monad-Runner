using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void RestrartGame()
    {
        FindObjectOfType<PlayerInput>().gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("GameScene");
    }
}
