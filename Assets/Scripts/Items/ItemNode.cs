﻿using UnityEngine;

public class ItemNode : MonoBehaviour
{
    [Header("Item properties")]
    [Tooltip("Multiplies the default spawn rate. If the spawn odds are 0.4 and modifier 2, this node will spawn an item 80% of the time.")]
    public float rarityModifier = 1f;

    [Tooltip("Bias towards a category. Selected cateogry will have 50% higher drop rate here.")]
    public Bias bias = Bias.Anything;

    [Tooltip("If checked, the biased item will have a 100% drop rate here.")]
    public bool forceBias = false;

    [Header("GameObjects")]
    [Tooltip("The item gameobject will be attached to this object floating up and down")]
    public GameObject itemHolder;

    [Tooltip("Speed of the floating")]
    public float speed = 2f;

    [Tooltip("Height of the floating")]
    public float height = 2.5f;

    [Header("ItemNode")]
    public ParticleSystem particles;

    public bool hasItem;

    public enum Bias
    {
        Anything = 0,
        Weapon = 1,
        Ammo = 2,
        Healing = 3
    }

    private void Start()
    {
        //itemHolder.GetComponent<Renderer>().material.color = Color.clear;
        //GetComponent<MeshRenderer>().enabled = false;
        for (int i = 0; i < 1000; i++)
        {
            Item x = MGetItem();
            if (x != null)
            {

                Debug.Log(x.name);
            }
        }
    }

    private void Update()
    {
        //Make the item float
        itemHolder.transform.localPosition = new Vector3(0, Mathf.Sin(Time.time * speed) * height + 15, 0);
        itemHolder.transform.RotateAround(transform.position, transform.up, Time.deltaTime * 10f);
    }

    /// <summary>
    /// Generates the item that should spawn here.
    /// Ran by the master client only.
    /// </summary>
    public Item MGetItem()
    {
        //calculate spawn odds
        if (UnityEngine.Random.Range(0, 100) > Core.SpawnOdds * rarityModifier * 100) return null;

        //try 20 times to spawn here
        for (int i = 0; i < 20; i++)
        {
            Item rnd = ItemDatabase.Instance.weightedItems[Random.Range(0, ItemDatabase.Instance.weightedItems.Count)];
            //if anything can spawn, just pick the first
            if (bias == Bias.Anything) return rnd;

            //if bias should be forced, take first item if correct
            else if (forceBias && (int)rnd.type == (int)bias) return rnd;

            //bias is active but not forced, 60% chance to discard incorrect items
            else if ((int)rnd.type == (int)bias && Random.Range(0, 100) < 60) return rnd;
        }
        return null;
    }
    public void SetItem(Item item)
    {

    }
}