using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide : MonoBehaviour
{
    [SerializeField] Transform playerPosition;
    [SerializeField] Image guideFlickImage;
    [SerializeField] Text guideFlickText;
    [SerializeField] Image guideWhitePollImage;
    [SerializeField] Text guideWhitePollText;
    [SerializeField] Image guideColorPollImage;
    [SerializeField] Text guideColorPollText;
    [SerializeField] Image guideObstacleImage;
    [SerializeField] Text guideObstacleText;
    [SerializeField] Image guideGameOverImage;
    [SerializeField] Text guideGameOverText;
    [SerializeField] Image guideSwitchImage;
    [SerializeField] Text guideSwitchText;
    [SerializeField] GameObject guideSwitchFocus;
    private GameObject focusObject;
    [SerializeField] float displayTime = 5.0f;
    private bool displayGuideFlag;


    void Start()
    {
        if(PlayerPrefs.GetInt("Guide") == 0)
        {
            displayGuideFlag = true;
        }
        else
        {
            displayGuideFlag = false;
        }
    }
    void Update()
    {
        if(displayGuideFlag)
        {
            DisplayGuide();
        }
    }

    enum GuideDisPlayPosition
    {
        Flick = 100,
        WhitePoll = 200,
        ColorPoll = 320,
        Obstacle = 520,
        GameOverRule = 670,
        StageGateSwitch = 3020
    }


    private void DisplayGuide()
    {
        switch((int)Mathf.Floor(playerPosition.position.z))
        {
            case (int)GuideDisPlayPosition.Flick:
            guideFlickImage.enabled = true;
            guideFlickText.enabled = true;
            StartCoroutine(DisactivateGuide(guideFlickImage, guideFlickText, displayTime));
            break;
            
            case (int)GuideDisPlayPosition.WhitePoll:
            guideWhitePollImage.enabled = true;
            guideWhitePollText.enabled = true;
            StartCoroutine(DisactivateGuide(guideWhitePollImage, guideWhitePollText, displayTime));
            break;

            case (int)GuideDisPlayPosition.ColorPoll:
            guideColorPollImage.enabled = true;
            guideColorPollText.enabled = true;
            StartCoroutine(DisactivateGuide(guideColorPollImage, guideColorPollText, displayTime));
            break;

            case (int)GuideDisPlayPosition.Obstacle:
            guideObstacleImage.enabled = true;
            guideObstacleText.enabled = true;
            StartCoroutine(DisactivateGuide(guideObstacleImage, guideObstacleText, displayTime));
            break;

            case (int)GuideDisPlayPosition.GameOverRule:
            guideGameOverImage.enabled = true;
            guideGameOverText.enabled = true;
            StartCoroutine(DisactivateGuide(guideGameOverImage, guideGameOverText, displayTime));
            break;

            case (int)GuideDisPlayPosition.StageGateSwitch:
            guideSwitchImage.enabled = true;
            guideSwitchText.enabled = true;
            StartCoroutine(DisactivateGuide(guideSwitchImage, guideSwitchText, displayTime)); 
            GameObject check = GameObject.FindWithTag("SwitchFocus");
            if(check == null)
            {
                focusObject = Instantiate(guideSwitchFocus);
                StartCoroutine(DisactivateFocusSwitch(focusObject, displayTime));
            }
            break;

            default:
            break;

        }
    }

    IEnumerator DisactivateGuide(Image image, Text text, float delay)
    {
        yield return new WaitForSeconds(delay);
        image.enabled = false;
        text.enabled = false;
    }

    IEnumerator DisactivateFocusSwitch(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(focusObject);
    }


}
