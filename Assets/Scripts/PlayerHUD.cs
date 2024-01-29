using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text txt;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMP_Text rotationTxt;
    [SerializeField] private GameObject crosshair, crosshair2;
    [SerializeField] private Animator onHitImage;
    [SerializeField] private GameObject miniGameParent;

    [SerializeField] private TMP_Text interactName;
    [SerializeField] private Image interactBar;

    public void DisplayInfo(string _text)
    {
        StopAllCoroutines();
        StartCoroutine(PingInfo(_text));
    }

    private IEnumerator PingInfo(string _txt)
    {
        txt.text = _txt;
        yield return new WaitForSeconds(2f);
        txt.text = string.Empty;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (pauseMenu.activeSelf)
            {
                Manager.Instance.lobbyManager.LeaveSession();
            }
        }
        if(rotationTxt != null)
        {
            rotationTxt.text = transform.eulerAngles.y.ToString("0");
        }
        
    }


    public void ToggleCrosshair(bool _p) { crosshair.SetActive(_p); }
    //public void ToggleOnHitImage(bool _p) { onHitImage.SetActive(_p); }
    public void ToggleMiniGame(bool _p) { miniGameParent.SetActive(_p); }

    public void PlayHitAnimation()
    {
        onHitImage.Play("Shake", -1, 0);
    }

    public void InitInteract(string _interactName, float _fill)
    {
        interactName.gameObject.SetActive(true);
        interactBar.gameObject.SetActive(true);

        interactName.text = _interactName;
        interactBar.fillAmount = _fill;
    }

    public void SetInteractPercent(float _fill)
    {
        interactBar.fillAmount = _fill;
    }

    public void StopInteract()
    {
        interactName.gameObject.SetActive(false);
        interactBar.gameObject.SetActive(false);
    }

    public void SetCrosshair(byte _index)
    {
        crosshair.SetActive(_index == 0);
        crosshair2.SetActive(_index == 1);
    }
}