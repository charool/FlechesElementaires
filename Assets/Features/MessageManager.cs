using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance { get; private set; }

    public bool Shown
    {
        get => gameObject.activeSelf;

        set
        {
            gameObject.SetActive(value);
            InventoryBar.Instance.gameObject.SetActive(!value);
        }
    }

    public string CurrentMessageName { get; private set; }

    [SerializeField] private List<Message> messages;

    protected void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Broadcast(string __name)
    {
        if (CurrentMessageName != __name) {
            Message msg = messages.Find(message => message.Name == __name);

            gameObject.GetComponent<TextMeshProUGUI>().text = msg.Content;
            CurrentMessageName = msg.Name;

            print($"Broadcasting message '{CurrentMessageName}'!");

            Shown = true;
        } else if (Shown) {
            print($"'{CurrentMessageName}' is already being broadcasted!");
        } else {
            Shown = true;
        }

    }
}
