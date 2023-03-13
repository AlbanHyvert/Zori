using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : Singleton<LoadingSceneManager>
{
    [SerializeField] private GameObject _loadingScreen = null;
    [SerializeField] private GameObject _loadingIcon = null;
    [SerializeField] private TMPro.TextMeshProUGUI _loadingText = null;
    [Space(10)]
    [SerializeField] private float _fakeLoading = 0.1f;

    public void LoadLevelAsync(string levelName)
    {
        StartCoroutine(LoadSceneAsync(levelName));
    }

    private IEnumerator LoadSceneAsync(string levelName)
    {
        _loadingScreen.SetActive(true);

        yield return new WaitForSecondsRealtime(_fakeLoading); 

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);

        if(Player.Instance)
            Player.Instance.Loaded(false);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            _loadingIcon.transform.rotation *= Quaternion.Euler(0, 0, -5f);
            _loadingText.text = "Loading " + (int)(progress * 100f) + "%";
            yield return null;
        }

        if(Player.Instance)
            Player.Instance.Loaded(true);

        _loadingScreen.SetActive(false);

        StopAllCoroutines();
    }
}
