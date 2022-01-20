using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GoldManagerVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text goldValueText;

    [SerializeField] private TMP_Text unitysTotal;

    [SerializeField] private GoldManager goldManager;

    [SerializeField] private UnitysManager unitysManager;

    [SerializeField] private MinerSlot[] minersSlots;

    [SerializeField] private Sprite minerSprite;

    [SerializeField] private Sprite emptySprite;

    private void Awake()
    {
        unitysManager.OnAddObj += OnNewUnity;

        unitysManager.OnRemoveObj += OnRemoveUnity;

        goldManager.OnActiveNewGenerator += OnActiveNewGenerator;

        goldManager.OnDisableGenerator += OnDisableGenerator;

        goldManager.OnUpdateGold += OnUpdateGold;

        goldValueText.text = goldManager.GetCurrentGold().ToString();


        foreach(MinerSlot ui in minersSlots)
        {
            ui.timer.OnPassTime += OnGenereteGold;
        }
    }

    private void OnRemoveUnity(GameObject obj)
    {
        unitysTotal.text = unitysManager.GetCurrentUnitysValue()+"";
    }

    private void OnNewUnity(int unitysCount)
    {
        unitysTotal.text = unitysManager.GetCurrentUnitysValue() + "";
    }

    private void OnUpdateGold()
    {
        goldValueText.text = goldManager.GetCurrentGold().ToString();
    }
   
    private void OnGenereteGold(UiTimer uiTimer)
    {
        for (int i = 0; i < minersSlots.Length; i++)
        {
            MinerSlot slot = minersSlots[i];

            if (slot.timer==uiTimer)
            {

                slot.UIMove.StartMove();
                slot.UIScale.StartScale();

                break;
            }
        }
    }

    private void OnDisableGenerator(GoldGenerator goldGenerator,int id)
    {
        for (int i = 0; i < minersSlots.Length; i++)
        {
            MinerSlot slot = minersSlots[i];

            if (slot.goldGenerator == goldGenerator && slot.executionId == id)
            {              

                slot.image.sprite = emptySprite;

                slot.goldGenerator = null;

                break;
            }
        }
    }

    private void OnActiveNewGenerator(GoldGenerator goldGenerator, int id)
    {
        for (int i = 0; i < minersSlots.Length; i++)
        {
            MinerSlot slot = minersSlots[i];

            if (slot.goldGenerator == null)
            {
                //slot.slot.SetActive(true);

                slot.image.sprite = minerSprite;

                slot.goldGenerator = goldGenerator;

                slot.executionId = id;

                slot.timer.StartTimerWhitEvent(goldGenerator.GetTotalTimer(),goldGenerator.GetGoldInterval());

                break;

            }
        }
    }
}

[System.Serializable]
public class MinerSlot
{
    public GoldGenerator goldGenerator;
    public int executionId;
    public GameObject slot;
    public Image image;
    public UiTimer timer;
    //public GameObject goldValue;
    public UIMove UIMove;
    public UIScale UIScale;
}

