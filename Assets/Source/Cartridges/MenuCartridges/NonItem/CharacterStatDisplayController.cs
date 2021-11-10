using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatDisplayController : MonoBehaviour
{
    iMessageClient c_client;

    [SerializeField] private Text SelectedCharacterNameText;
    [SerializeField] private Text SelectedCharacterSpeedText;
    [SerializeField] private Text SelectedCharacterBalanceText;
    [SerializeField] private Text SelectedCharacterTricksText;

    [SerializeField] private Image SelectedCharacterSpeedDisplay;
    [SerializeField] private Image SelectedCharacterBalanceDisplay;
    [SerializeField] private Image SelectedCharacterTricksDisplay;

    [SerializeField] List<CharacterAttributeData> CharacterAttributes;

    public int currentIndex;
    private int currentActiveIndex;

    private float targetSpeedAlpha;
    private float targetBalanceAlpha;
    private float targetTrickAlpha;

    // Start is called before the first frame update
    void Start()
    {
        c_client = new StatDisplayMessageClient(ref SelectedCharacterSpeedText, this);
        MessageServer.Subscribe(ref c_client, MessageID.MENU_ITEM_CHANGED);
        currentActiveIndex = -1; // force update on start
    }

    // Update is called once per frame
    void Update()
    {
        if (currentActiveIndex != currentIndex)
        {
            currentActiveIndex = currentIndex;
            int currentAlphaMax = CharacterAttributes[currentActiveIndex].MaxStats;

            float currentSpeed = CharacterAttributes[currentActiveIndex].Speed;
            float currentBalance = CharacterAttributes[currentActiveIndex].Balance;
            float currentTricks = CharacterAttributes[currentActiveIndex].Tricks;

            targetSpeedAlpha = Mathf.Lerp(1.0f, 0, currentSpeed / currentAlphaMax); //1 - (currentSpeed / currentAlphaMax); 
            targetBalanceAlpha = Mathf.Lerp(1.0f, 0, currentBalance / currentAlphaMax);
            targetTrickAlpha = Mathf.Lerp(1.0f, 0, currentTricks / currentAlphaMax);

            // debug values
            SelectedCharacterNameText.text = CharacterAttributes[currentActiveIndex].Name;
            SelectedCharacterSpeedText.text = "SPEED: " + CharacterAttributes[currentActiveIndex].Speed;
            SelectedCharacterBalanceText.text = "BALANCE: " + CharacterAttributes[currentActiveIndex].Balance;
            SelectedCharacterTricksText.text = "TRICKS: " + CharacterAttributes[currentActiveIndex].Tricks;
        }

        // it's easier to just do a bespoke implementation rather than force the state machine pattern here
        float currentSpeedAlpha = SelectedCharacterSpeedDisplay.material.GetFloat("_Cutoff");
        float currentBalanceAlpha = SelectedCharacterBalanceDisplay.material.GetFloat("_Cutoff");
        float currentTricksAlpha = SelectedCharacterTricksDisplay.material.GetFloat("_Cutoff");

        float target = Mathf.Lerp(currentSpeedAlpha, targetSpeedAlpha, 0.01f);
        SelectedCharacterSpeedDisplay.material.SetFloat("_Cutoff", target);

        target = Mathf.Lerp(currentBalanceAlpha, targetBalanceAlpha, 0.01f);
        SelectedCharacterBalanceDisplay.material.SetFloat("_Cutoff", target);

        target = Mathf.Lerp(currentTricksAlpha, targetTrickAlpha, 0.01f);
        SelectedCharacterTricksDisplay.material.SetFloat("_Cutoff", target);


    }
}
