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

            float currentSpeedAlpha = Mathf.Lerp(0.95f, 1.0f, currentSpeed / currentAlphaMax); //1 - (currentSpeed / currentAlphaMax); 
            float currentBalanceAlpha = Mathf.Lerp(0.95f, 1.0f, currentBalance / currentAlphaMax);
            float currentTricksAlpha = Mathf.Lerp(0.95f, 1.0f, currentTricks / currentAlphaMax);

            SelectedCharacterSpeedDisplay.material.SetFloat("_Cutoff", currentSpeedAlpha);
            SelectedCharacterBalanceDisplay.material.SetFloat("_Cutoff", currentBalanceAlpha);
            SelectedCharacterTricksDisplay.material.SetFloat("_Cutoff", currentTricksAlpha);

            // debug values
            SelectedCharacterNameText.text = CharacterAttributes[currentActiveIndex].Name;
            SelectedCharacterSpeedText.text = "SPEED: " + CharacterAttributes[currentActiveIndex].Speed;
            SelectedCharacterBalanceText.text = "BALANCE: " + CharacterAttributes[currentActiveIndex].Balance;
            SelectedCharacterTricksText.text = "TRICKS: " + CharacterAttributes[currentActiveIndex].Tricks;
        }
    }
}
