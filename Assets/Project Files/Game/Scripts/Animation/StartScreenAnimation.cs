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

    private float animationDuration = 4f;
    private bool isAnimating = true;

    public event System.Action OnAnimationsComplete;

    void Start()
    {
        StartCoroutine(BouncyLogoEffect());
        StartCoroutine(BlinkLights());
        StartCoroutine(PulseLoadingText());
        StartCoroutine(RandomBouncyMovement(base1));
        StartCoroutine(RandomBouncyMovement(base2));
        StartCoroutine(RandomBouncyMovement(base3));
        StartCoroutine(TileFloatingMotion());

        StartCoroutine(AnimationSequence()); // Master timer
    }

    // Master Timer: Stops animations after full duration
    private IEnumerator AnimationSequence()
    {
        yield return new WaitForSeconds(animationDuration);
        isAnimating = false;
        StopAnimations();
        OnAnimationsComplete?.Invoke();
    }

    // Bouncy Logo Scale Effect (Smooth without jerks)
    IEnumerator BouncyLogoEffect()
    {
        Vector3 baseScale = Vector3.one * 0.7f; // ✅ Fixed base scale (prevents continuous growth)
        float bounceStrength = 0.1f; // ✅ Adjust bounce intensity
        float speedMultiplier = 1.1f; // ✅ Adjust bounce speed

        while (isAnimating)
        {
            float elapsed = Time.time % animationDuration; // ✅ Keeps the bounce looping
            float bounce = Mathf.Sin(elapsed * Mathf.PI * 2 * speedMultiplier) * bounceStrength; // ✅ Smooth bounce

            // ✅ Keep the base scale constant & only apply bouncing effect
            logo.transform.localScale = baseScale + new Vector3(bounce, bounce, 0);

            yield return null;
        }

        logo.transform.localScale = baseScale; // ✅ Reset scale after stopping
    }




    // Blinking Lights Effect
   IEnumerator BlinkLights()
{
    bool isLight1On = true;
    float blinkSpeed = 0.2f; // ✅ Adjust this to increase or decrease speed

    while (isAnimating)
    {
        light1.enabled = isLight1On;
        light2.enabled = !isLight1On;
        isLight1On = !isLight1On;

        yield return new WaitForSeconds(blinkSpeed); // ✅ Adjust blink speed dynamically
    }

    light1.enabled = false;
    light2.enabled = false;
}


    // Pulsing "Loading..." Text
  IEnumerator PulseLoadingText()
{
    string baseText = "Loading"; // ✅ Base text without dots
    int dotCount = 0; // ✅ Tracks dots (1, 2, 3, 1, 2, 3 pattern)

    while (isAnimating)
    {
        dotCount = (dotCount % 3) + 1; // ✅ Cycles through 1 → 2 → 3 → 1 → 2 → 3
        loadingText.text = baseText + new string('.', dotCount);

        yield return new WaitForSeconds(0.5f); // ✅ Controls speed (adjust for faster/slower animation)
    }

    loadingText.text = baseText; // ✅ Reset text when stopping
}



    // Floating Motion for TILE (No sudden jumps)
   IEnumerator TileFloatingMotion()
{
    Debug.Log("Tile animation started"); 
    Vector3 startPos = tile.position; 
    Quaternion startRot = tile.rotation; 
    Vector3 startScale = tile.localScale; 

    float floatStrength = 3f; 
    float tiltStrength = 1.5f; 
    float scaleStrength = 0.05f; 
    float speedMultiplier = 0.1f; // ✅ Increase speed by adjusting this

    float startTime = Time.time;

    while (isAnimating)
    {
        float elapsed = (Time.time - startTime) * speedMultiplier; // ✅ Faster animation

        // ✅ Faster Floating motion
        float newY = startPos.y + Mathf.Sin(elapsed * Mathf.PI * 4) * floatStrength; 

        // ✅ Faster Tilting motion
        float tiltAngle = Mathf.Sin(elapsed * Mathf.PI * 3) * tiltStrength;
        Quaternion newRotation = startRot * Quaternion.Euler(0, 0, tiltAngle); 

        // ✅ Faster Zoom-in/Zoom-out effect
        float scaleFactor = 1f + Mathf.Sin(elapsed * Mathf.PI * 2) * scaleStrength;
        Vector3 newScale = startScale * scaleFactor;

        // ✅ Apply transformations
        tile.position = new Vector3(startPos.x, newY, startPos.z);
        tile.rotation = newRotation;
        tile.localScale = newScale;

        yield return null;
    }

    // ✅ Reset position, rotation, and scale after stopping
    tile.position = startPos;
    tile.rotation = startRot;
    tile.localScale = startScale;
}




    public void UpdateAnimation(float progress) 
{
    // Add animation update logic here if needed
}

    // Bouncing Motion for Fruits (Smooth movement)
  IEnumerator RandomBouncyMovement(Transform obj)
{
    Debug.Log(obj.name + " animation started"); // ✅ Debugging Check
    Vector3 startPos = obj.position;

    float speedMultiplierX = Random.Range(2f, 3f); // ✅ Unique horizontal speed
    float speedMultiplierY = Random.Range(2f, 3f); // ✅ Unique vertical speed
    float bounceHeight = Random.Range(5f, 10f); // ✅ Unique bounce intensity
    float movementRangeX = Random.Range(5f, 15f); // ✅ Unique X movement range
    float movementRangeY = Random.Range(5f, 15f); // ✅ Unique Y movement range
    float startTime = Time.time; // ✅ Each object starts independently

    while (isAnimating)
    {
        float elapsed = Time.time - startTime;
        if (elapsed >= animationDuration) // ✅ Stops at exact duration
            break;

        // ✅ Independent smooth movement for each object
        float moveX = Mathf.Sin(elapsed * Mathf.PI * 2 * speedMultiplierX / animationDuration) * movementRangeX;
        float moveY = Mathf.Cos(elapsed * Mathf.PI * 2 * speedMultiplierY / animationDuration) * movementRangeY;
        float bounce = Mathf.Sin(elapsed * Mathf.PI * 2 / animationDuration) * bounceHeight;

        obj.position = startPos + new Vector3(moveX, moveY + bounce, 0);
        yield return null;
    }

    obj.position = startPos; // ✅ Reset position after stopping
}




    // Stop all animations
    public void StopAnimations()
    {
        isAnimating = false;
        StopAllCoroutines();

        light1.enabled = false;
        light2.enabled = false;
        loadingText.transform.localScale = Vector3.one;

        Debug.Log("All animations stopped!");
    }
}
