using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class StartScreenAnimation : MonoBehaviour
{
    public Image logo;
    public Image light1;
    public Image light2;
    public TextMeshProUGUI loadingText;
    
    public Transform base1; // Watermelon
    public Transform base2; // Orange
    public Transform base3; // Capsicum
    public Transform tile;  // TILE image

    private float animationDuration = 3.5f;

    private Coroutine lightCoroutine;
    private Coroutine textCoroutine;
    private Coroutine base1Coroutine, base2Coroutine, base3Coroutine;
    private Coroutine tileCoroutine;

    void Start()
    {
        StartCoroutine(BouncyLogoEffect());
        lightCoroutine = StartCoroutine(BlinkLights());
        textCoroutine = StartCoroutine(PulseLoadingText());

        base1Coroutine = StartCoroutine(RandomBouncyMovement(base1));
        base2Coroutine = StartCoroutine(RandomBouncyMovement(base2));
        base3Coroutine = StartCoroutine(RandomBouncyMovement(base3));
        
        tileCoroutine = StartCoroutine(TileFloatingMotion());

        Invoke(nameof(StopAnimations), animationDuration);
    }

    // Logo Bouncy Scale-In Effect
    IEnumerator BouncyLogoEffect()
    {
        logo.transform.localScale = Vector3.zero;
        float time = 0f;
        float duration = 1f;
        Vector3 targetScale = Vector3.one;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float bounce = Mathf.Sin(t * Mathf.PI * 2) * (1 - t);
            logo.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, targetScale, t + bounce);
            yield return null;
        }
        logo.transform.localScale = targetScale;
    }

    // Blinking Lights Effect (Only One Light is ON at a Time)
    IEnumerator BlinkLights()
    {
        bool isLight1On = true;

        while (true)
        {
            light1.enabled = isLight1On;
            light2.enabled = !isLight1On;
            
            isLight1On = !isLight1On; // Toggle state
            
            yield return new WaitForSeconds(0.4f);
        }
    }

    // Pulsing "Loading..." Text Effect
    IEnumerator PulseLoadingText()
    {
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            float scale = 1f + Mathf.Sin(time * 4f) * 0.1f;
            loadingText.transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
    }

    // Floating Motion for TILE Image
    IEnumerator TileFloatingMotion()
    {
        Vector3 startPos = tile.localPosition;
        float floatStrength = 10f; // Adjust for more/less motion
        float speed = 1.5f; // Adjust speed

        while (true)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * speed) * floatStrength;
            tile.localPosition = new Vector3(startPos.x, newY, startPos.z);
            yield return null;
        }
    }

    // Random Bouncy Motion for Fruits
    IEnumerator RandomBouncyMovement(Transform obj)
    {
        Vector3 startPos = obj.localPosition;
        Vector3 randomDirection = new Vector3(
            Random.Range(-30f, 30f), // X direction (left or right)
            Random.Range(15f, 40f),  // Y direction (upwards movement)
            0
        );

        float time = 0f;
        float duration = Random.Range(1.2f, 2f); // Random bounce speed

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float bounce = Mathf.Sin(t * Mathf.PI * 2) * (1 - t) * 15f; // Bounce effect
            obj.localPosition = Vector3.Lerp(startPos, startPos + randomDirection, t) + new Vector3(0, bounce, 0);
            yield return null;
        }

        obj.localPosition = startPos + randomDirection; // Final position after bouncing
    }

    // Stop all animations
    public void StopAnimations() // Made public for external access
    {
        if (lightCoroutine != null) StopCoroutine(lightCoroutine);
        if (textCoroutine != null) StopCoroutine(textCoroutine);
        if (base1Coroutine != null) StopCoroutine(base1Coroutine);
        if (base2Coroutine != null) StopCoroutine(base2Coroutine);
        if (base3Coroutine != null) StopCoroutine(base3Coroutine);
        if (tileCoroutine != null) StopCoroutine(tileCoroutine);

        light1.enabled = false;
        light2.enabled = false;
        loadingText.transform.localScale = Vector3.one;
        tile.localPosition = tile.localPosition; // Stop at last position

        Debug.Log("All animations stopped!");
    }

    // Update function to sync animation with loading progress
    public void UpdateAnimation(float progress)
    {
       // loadingText.text = $"Loading {Mathf.RoundToInt(progress * 100)}%";
    }
}
