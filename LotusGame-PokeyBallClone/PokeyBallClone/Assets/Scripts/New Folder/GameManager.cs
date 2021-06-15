using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IEndGameObserver
{
    [SerializeField] float levelEndPoint_Y;
    [SerializeField] float levelLosePoint_Y;
    public bool Finish;
    public bool Lose;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject startPanel;


    #region Observer Funcs
    private List<IEndGameObserver> endGameObservers;

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    private void NotifyObservers()
    {
        foreach (IEndGameObserver observer in endGameObservers)
        {
            observer.Notify();
        }
    }

    void Awake()
    {
        endGameObservers = new List<IEndGameObserver>();
    }
    #endregion


    void Start()
    {
        AddObserver(this);
        AddObserver(FindObjectOfType<InputHandler>());        
        restartButton.SetActive(false);
    }

    void Update()
    {
        if (!Finish && FindObjectOfType<InputHandler>().ball.transform.position.y > levelEndPoint_Y)
        {
            Finish = true;
            StopAllCoroutines();
            NotifyObservers();
        }
        if (!Finish && FindObjectOfType<InputHandler>().ball.transform.position.y < levelLosePoint_Y)
        {
            Lose = true;
            Restart();
        }
    }

    public void StartButton()
    {
        startPanel.SetActive(false);
        FindObjectOfType<InputHandler>().GameStart();
    }

    public void Restart()
    {
        if (Lose)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }

    public void Notify()
    {

        if (Finish)
        {
            StartCoroutine(CompleteEnumerator());
        }
    }
    IEnumerator CompleteEnumerator()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(3f);
            Debug.Log("Won");
            //Sıradaki Level Gelecek!
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}