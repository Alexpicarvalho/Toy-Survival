using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private Image healthImage;
    [SerializeField] private Image staminaImage;
    [SerializeField] private Image decayImage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.maxHealth != 0) healthImage.fillAmount = player.health / player.maxHealth;
        if (player.maxStamina != 0) staminaImage.fillAmount = player.stamina / player.maxStamina;
        if (player.maxDecay != 0) decayImage.fillAmount = player.decay / player.maxDecay;
    }
}
