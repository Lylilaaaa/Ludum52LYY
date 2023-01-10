using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject loader_canvas;
    [SerializeField] private Image progress_bar;

    private float progress_target;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        progress_target = 0;
        progress_bar.transform.localScale = new Vector3(0, 1, 1);

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loader_canvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            progress_target = scene.progress;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        await Task.Delay(2000);
        loader_canvas.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        progress_bar.transform.localScale = new Vector3(Mathf.MoveTowards(progress_bar.transform.localScale.x, progress_target, 3 * Time.deltaTime), 1, 1);
    }
}
