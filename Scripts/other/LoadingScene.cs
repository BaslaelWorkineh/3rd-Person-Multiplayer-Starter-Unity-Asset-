using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class LoadingScene : MonoBehaviour
{
    public TextMeshProUGUI percent;
   public GameObject LoadingScreen;
   public Slider loadingBar;

   public void LoadScene(int sceneId)
   {
        StartCoroutine(LoadSceneAsync(sceneId));
   }

   IEnumerator LoadSceneAsync(int sceneId)
   {
    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

   
    LoadingScreen.SetActive(true);
    while(!operation.isDone)
    {
        percent.text = (operation.progress * 100).ToString() + "%";
        loadingBar.value = operation.progress;
        yield return null;
    }

   }
}
