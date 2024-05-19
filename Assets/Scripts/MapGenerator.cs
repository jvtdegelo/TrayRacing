using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] startRaceTracks;
    [SerializeField] GameObject[] normalRaceTracks;
    readonly int trackSize = 150 / 2; // half of size of track

    // Start is called before the first frame update
    void Start()
    {
        int rotation = 0;
        Vector3 position;
        GameObject gameObject;

        for (int i = 0; i <= 2; i++, rotation += 90) // 3 normal race tracks
        {
            position = this.GetPosition(i);

            gameObject = Instantiate(normalRaceTracks[Random.Range(0, normalRaceTracks.Length)], position, Quaternion.identity, transform);

            // 50% chance to flip the map
            bool flipMap = Random.value > 0.5f;
            if (flipMap)
                FlipMap(gameObject, i);
            else
                gameObject.transform.Rotate(new Vector3(0f, rotation, 0f));

            gameObject.name = "PistaN" + i;
        }

        position = this.GetPosition(3);

        gameObject = Instantiate(startRaceTracks[Random.Range(0, startRaceTracks.Length)], position, Quaternion.identity, transform);
        gameObject.transform.Rotate(new Vector3(0f, rotation, 0f));
        gameObject.name = "PistaInicio";
    }

    private Vector3 GetPosition(int i)
    {
        Vector3 position = i switch
        {
            0 => new Vector3(-trackSize, 0.1f, trackSize),  // -50  50
            1 => new Vector3(trackSize, 0.1f, trackSize),   //  50  50
            2 => new Vector3(trackSize, 0.1f, -trackSize),  //  50 -50
            3 => new Vector3(-trackSize, 0.1f, -trackSize), // -50 -50 // used for start race track
            _ => new Vector3(0, 0.1f, 0),
        };

        return position;
    }

    private void FlipMap(GameObject gameObject, int i)
    {

        Vector3 rotate = i switch
        {
            // rotates on X, Y or Z axis
            0 => new Vector3(0, -90, 180),
            1 => new Vector3(0, 180, 0),
            2 => new Vector3(0, 90, -180),
            _ => new Vector3(0, 0, 0),
        };

        Vector3 scale = i switch
        {
            // flips on X, Y or Z axis
            0 => new Vector3(1, -1, 1),
            1 => new Vector3(1, 1, -1),
            2 => new Vector3(1, -1, 1),
            _ => new Vector3(1, 1, 1),
        };

        gameObject.transform.Rotate(rotate);
        gameObject.transform.localScale = scale;
    }
}
