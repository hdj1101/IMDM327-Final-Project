using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyEffectManager : MonoBehaviour
{
    public static SkyEffectManager Instance;
    public GameObject starPrefab; // Star prefab to spawn
    public Transform starParent;  // Parent object to keep the stars organized
    public Material skyboxMaterial; // Skybox material to change the brightness
    public float maxStars = 100; // Max number of stars to spawn on screen at once
    public float maxSkyboxBrightness = 0.7f; // Max skybox brightness
    public float lerpSpeed = 0.1f; // Gradual lerp speed for skybox brightness
    public float minStarRadius = 250.0f; // Minimum spawn distance from center
    public float maxStarRadius = 700.0f; // Maximum spawn distance from center
    public float minLifetime = 5f; // Minimum lifetime for a star
    public float maxLifetime = 30f; // Maximum lifetime for a star
    public float fadeDuration = 2f; // Duration for fading in and out

    private List<GameObject> activeStars = new List<GameObject>(); // List to keep track of active stars

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SUPERFUNTIME()
    {
        StartCoroutine(ContinuousSkyEffectUpdate());
    }

    private IEnumerator ContinuousSkyEffectUpdate()
    {
        StartCoroutine(SpawnStars());

        while (true)
        {
            float brightness = Mathf.Lerp(RenderSettings.skybox.GetFloat("_Exposure"), 0.8f, lerpSpeed * Time.deltaTime);
            RenderSettings.skybox.SetFloat("_Exposure", brightness);

            yield return null;
        }
    }

    private IEnumerator SpawnStars()
    {
        while (true)
        {
            if (activeStars.Count < maxStars)
            {
                int starsToSpawn = Random.Range(5, 16);

                for (int i = 0; i < starsToSpawn; i++)
                {
                    Vector3 spawnPosition = Random.onUnitSphere * Random.Range(minStarRadius, maxStarRadius);
                    spawnPosition.y = Mathf.Abs(spawnPosition.y);

                    Quaternion randomRotation = Random.rotation;

                    GameObject newStar = Instantiate(starPrefab, spawnPosition, randomRotation, starParent);
                    activeStars.Add(newStar);

                    float lifetime = Random.Range(minLifetime, maxLifetime);

                    // Fade in the star
                    StartCoroutine(FadeStar(newStar, 0f, 1f, fadeDuration));

                    // Fade out the star
                    StartCoroutine(DestroyStarAfterTime(newStar, lifetime));
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator DestroyStarAfterTime(GameObject star, float lifetime)
    {
        yield return new WaitForSeconds(lifetime - fadeDuration);

        StartCoroutine(FadeStar(star, 1f, 0f, fadeDuration));
        yield return new WaitForSeconds(fadeDuration);

        activeStars.Remove(star);
        Destroy(star);
    }

    private IEnumerator FadeStar(GameObject star, float startAlpha, float endAlpha, float duration)
    {
        if (star.TryGetComponent<Renderer>(out Renderer renderer))
        {
            Material starMaterial = renderer.material;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                // Update alpha
                Color color = starMaterial.color;
                color.a = Mathf.Lerp(startAlpha, endAlpha, t);
                starMaterial.color = color;

                yield return null;
            }

            Color finalColor = starMaterial.color;
            finalColor.a = endAlpha;
            starMaterial.color = finalColor;
        }
    }
}
