using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{

    private LineRenderer _line;
    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.right, out hit))
        {
            if (hit.collider)
            {
                _line.SetPosition(1, new Vector3(hit.distance, 0, 0));
            }
        }
        else
        {
            _line.SetPosition(1, new Vector3(5000, 0, 0));
        }
    }
}
