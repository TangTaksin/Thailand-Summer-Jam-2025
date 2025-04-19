using System.Collections.Generic;
using UnityEngine;

public class CloudLayer : MonoBehaviour
{
    public SpriteRenderer cloudSprite;

    List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    List<Transform> transforms = new List<Transform>();
    public Vector3 size = new Vector3(4.8f, 2.7f, 0);
    public float cloudsPerUnit = 5f;
    public float noiseScale = 0.1f;

    void Start()
    {
        for (int x = 0; x < size.x * cloudsPerUnit; x++)
        {
            for (int y = 0; y < size.y * cloudsPerUnit; y++)
            {
                Vector3 position = new Vector3(x / cloudsPerUnit, y / cloudsPerUnit, 0);
                SpriteRenderer cloud = Instantiate(cloudSprite, transform.position - (size / 2f) + position, Quaternion.identity);
                cloud.gameObject.SetActive(true);
                cloud.transform.parent = transform;
                sprites.Add(cloud);
                transforms.Add(cloud.transform);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < transforms.Count; i++)
        {
            Vector3 pos = transforms[i].position;

            float x = (Mathf.PerlinNoise(pos.x * noiseScale, Time.time * noiseScale) - 0.5f) * noiseScale;
            float y = (Mathf.PerlinNoise(pos.y * noiseScale, Time.time * noiseScale) - 0.5f) * noiseScale;

            transforms[i].position += new Vector3(x, y, 0) * Time.deltaTime;
        }
    }
}
