using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMap : MonoBehaviour
{
    public int width = 20;
    public int height = 20;

    public GameObject cube;

    void Start()
    {
        cube.SetActive(false);

        int mid_width = width / 2;
        int mid_height = height / 2;

        float cube_size = cube.transform.localScale.x; // We could get x, y or z values

        // Offset origin
        mid_width += (int)transform.position.x;
        mid_height += (int)transform.position.y;

        for(float i = -mid_width; i < mid_width; i += cube_size)
        {
            for(float j = -mid_height; j < mid_height; j += cube_size)
            {
                Vector3 position_cube = new Vector3(i, transform.position.y, j);
                Instantiate(cube, position_cube, Quaternion.identity);
            }
        }
    }

    void Update()
    {
        
    }
}
