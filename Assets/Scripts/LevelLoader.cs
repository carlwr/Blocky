using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{

    public bool isCreditScene = false;

    private IEnumerator goToMenu()
    {
        yield return new WaitForSeconds(5);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadLevelOne()
    {
        SceneManager.LoadScene("DagmarLevel1");
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void LoadLevelThree()
    {
        SceneManager.LoadScene("FinalLevel");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isCreditScene)
        {
            StartCoroutine(goToMenu());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
