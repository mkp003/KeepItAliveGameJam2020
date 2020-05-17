using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIndicator : MonoBehaviour
{
    [SerializeField]
    private Text loadingText;

    private bool isLoading = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingActions());
    }

    private IEnumerator LoadingActions()
    {
        int count = 0;
        while (isLoading)
        {
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Please Wait";
            for(int i = 0; i <= count; i++)
            {
                loadingText.text = loadingText.text + ".";
            }
            if(count == 5)
            {
                count = 0;
            }
            count++;
        }
        
    }


    public void SetLoadingStatus(bool _val)
    {
        isLoading = false;
    }
}
