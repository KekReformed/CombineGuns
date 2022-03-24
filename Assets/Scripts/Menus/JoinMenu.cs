using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class JoinMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button continueButton;

    void Start()
    {
        SetUpInputField();
    }

    void Update()
    {
        
    }


    private void SetUpInputField()
    {
        string defaultName = PlayerPrefs.GetString("PlayerName", "Player");
        
        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        string playerName = nameInputField.text;

        PhotonNetwork.NickName = playerName;

        PlayerPrefs.SetString("PlayerName", playerName);
    }
}
