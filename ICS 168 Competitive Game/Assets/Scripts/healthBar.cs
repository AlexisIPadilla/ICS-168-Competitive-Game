using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    Vector3 initialPosition;
    RectTransform myTransform;
    float initialWidth;
    public PlayerUnit player;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        myTransform = GetComponent<RectTransform>();
        initialWidth = myTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
            myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, initialWidth * player.health/player.maxHealth);
    }
}
