using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] private GameObject StartGame;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject Score;
    private int ScoreInt = 0;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        ScoreInt = Convert.ToInt32(Math.Ceiling(Player.transform.position.x - 1));
        Score.GetComponent<Text>().text = "Score: " + ScoreInt;
    }

    public void StartButton()
    {
        Destroy(StartGame);
        Player.GetComponent<PlayerController>().Run();
    }

    public void DeathMenu()
    {
        GameOver.SetActive(true);    
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
