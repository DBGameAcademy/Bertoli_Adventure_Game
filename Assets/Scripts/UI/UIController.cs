using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController>
{
    public GameObject DamageTagObject;
    public GameObject DamageTagContainer;
    public int DamageTagCount = 5;
    public float DamageTagDuration = 0.5f;
    List<DamageTag> DamageTags = new List<DamageTag>();

    public CharacterWindowUI PlayerHUD;
    public PlayerInventoryUI PlayerInventoryUI;
    public TownUI TownUI;
    public GameObject InventoryButton;

    private void Start()
    {
        DamageTag tag = new DamageTag(DamageTagObject);
        DamageTags.Add(tag);
        for(int i = 0; i < DamageTagCount; i++)
        {
            GameObject newTag = GameObject.Instantiate(DamageTagObject, DamageTagContainer.transform);
            tag = new DamageTag(newTag);
            DamageTags.Add(tag);
        }

        ExitTown();
    }

    private void Update()
    {
        for (int i = 0; i < DamageTags.Count; i++)
        {
            if (DamageTags[i].RemainingDuration > 0)
            {
                DamageTags[i].RemainingDuration -= Time.deltaTime;
                DamageTags[i].DamageTagObject.transform.position += Vector3.up * Time.deltaTime;
            }
            else
            {
                DamageTags[i].DamageTagObject.SetActive(false);
            }
        }
    }

    public void ShowDamageTag(Vector3 _position, string _text)
    {
        DamageTag tag = null;
        for (int i = 0; i < DamageTags.Count; i++)
        {
            if (!DamageTags[i].DamageTagObject.activeSelf)
            {
                tag = DamageTags[i];
                break;
            }
        }
        if (tag != null)
        {
            tag.Text.text = _text;
            tag.DamageTagObject.SetActive(true);
            tag.DamageTagObject.transform.position = _position;
            tag.RemainingDuration = DamageTagDuration;
        }
    }

    public void EnterTown()
    {
        PlayerInventoryUI.gameObject.SetActive(false);
        InventoryButton.SetActive(false);
        TownUI.gameObject.SetActive(true);
        TownUI.GoToLobby();
    }

    public void ExitTown()
    {
        InventoryButton.SetActive(true);
        TownUI.gameObject.SetActive(false);
    }

    public void ToggleInventory()
    {
        PlayerInventoryUI.gameObject.SetActive(!PlayerInventoryUI.gameObject.activeSelf);
    }
}

class DamageTag
{
    public Text Text;
    public GameObject DamageTagObject;
    public float RemainingDuration;

    public DamageTag(GameObject _tagObj)
    {
        Text = _tagObj.GetComponentInChildren<Text>();
        DamageTagObject = _tagObj;
        DamageTagObject.SetActive(false);
    }
}