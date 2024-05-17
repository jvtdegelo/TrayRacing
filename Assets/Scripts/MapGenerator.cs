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

            gameObject = Instantiate(normalRaceTracks[Random.Range(0, normalRaceTracks.Length)], position, Quaternion.identity);
            gameObject.transform.Rotate(new Vector3(0f, rotation, 0f));
            gameObject.name = "PistaN" + i;

        }

        position = this.GetPosition(3);

        gameObject = Instantiate(startRaceTracks[Random.Range(0, startRaceTracks.Length)], position, Quaternion.identity);
        gameObject.transform.Rotate(new Vector3(0f, rotation, 0f));
        gameObject.name = "PistaInicio";

    }


    private Vector3 GetPosition(int i)
    {
        return i switch
        {
            0 => new Vector3(-trackSize, 0.1f, trackSize),  // -50  50
            1 => new Vector3(trackSize, 0.1f, trackSize),   //  50  50
            2 => new Vector3(trackSize, 0.1f, -trackSize),  //  50 -50
            3 => new Vector3(-trackSize, 0.1f, -trackSize), // -50 -50 // used for begin race track
            _ => new Vector3(0, 0.1f, 0),
        };
    }

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
