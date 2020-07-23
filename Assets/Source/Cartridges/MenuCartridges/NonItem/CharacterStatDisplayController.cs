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
            SelectedCharacterNameText.text = CharacterAttributes[currentActiveIndex].Name;
            SelectedCharacterSpeedText.text = "SPEED: " + CharacterAttributes[currentActiveIndex].Speed;
            SelectedCharacterBalanceText.text = "BALANCE: " + CharacterAttributes[currentActiveIndex].Balance;
            SelectedCharacterTricksText.text = "TRICKS: " + CharacterAttributes[currentActiveIndex].Tricks;
        }
    }
}
