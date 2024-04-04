using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class FirstLoader : MonoBehaviour
{

   public Slider loadingBar;
   public TextMeshProUGUI percent;

   private void Start()
   {
    LoadScene(1);
   }

   public void LoadScene(int sceneId)
   {
        StartCoroutine(LoadSceneAsync(sceneId));
   }

   IEnumerator LoadSceneAsync(int sceneId)
   {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        
        while(!operation.isDone)
        {
            percent.text = (operation.progress * 100).ToString() + "%";
            loadingBar.value = operation.progress;
            yield return null;
        }
    }
}
