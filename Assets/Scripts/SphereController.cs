using UnityEngine;
using System.Collections;
using TMPro;

public class SphereController : MonoBehaviour
{
    public Transform center;
    public Vector3 startPosition;

    [SerializeField] private float radius;
    [SerializeField] private float radiusDiff;
    [SerializeField] private float speed;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float acceleration;
    private float angle = 0f;
    private float t = 0f;

    public ParticleSystem fireworks;
    public TextMeshProUGUI distanceText;

    private float distanceTraveled;
    private bool isStopped;

    private Material sphereMaterial;
    private Animation scaleChange;
    private Coroutine movementCoroutine;
    private Coroutine colorCoroutine;

    private void Start()
    {
        isStopped = true;
        sphereMaterial = GetComponent<Renderer>().material;
        scaleChange = GetComponent<Animation>();

        fireworks.Stop();
        currentSpeed = 0.0f;
    }

    private IEnumerator MoveAlongSpiral()
    {
        startPosition = transform.position;

        while (!isStopped)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, speed, acceleration * Time.deltaTime);

            angle += currentSpeed * Time.deltaTime;
            t += Time.deltaTime * currentSpeed / (2 * Mathf.PI * radius);

            if (t >= 1f)
            {
                t = 0f;
                startPosition = transform.position;
            }

            float x = center.position.x + Mathf.Cos(angle) * radius;
            float z = center.position.y + Mathf.Sin(angle) * radius;
            transform.position = new Vector3(x, transform.position.y, z);
            distanceTraveled += Vector3.Distance(transform.position, startPosition);

            radius -= radiusDiff;

            if (radius <= 0.0f)
            {
                transform.position = new Vector3(center.position.x, transform.position.y, center.position.z);
                isStopped = true;
                StartCoroutine(Fireworks());
            }

            yield return null;
        }
    }

    private IEnumerator ChangeColor()
    {
        while (!isStopped)
        {
            // Change color of the sphere so it's color changes based on the position and the position on the RGB color wheel
            sphereMaterial.color = new Color(
                transform.position.x / 10,
                0,
                transform.position.z / 10);
            
            yield return null;
        }
    }

    public void StopMovement()
    {
        if (!isStopped)
        {
            isStopped = true;
            StopCoroutine(colorCoroutine);
            StartCoroutine(StopSphere());
        }
    }

    private IEnumerator StopSphere()
    {
        currentSpeed = 0.0f;
        distanceText.gameObject.SetActive(true);
        distanceText.text = "Distance traveled: " + distanceTraveled;
        yield return new WaitForSeconds(5f);
        distanceText.gameObject.SetActive(false);
        isStopped = false;
        movementCoroutine = StartCoroutine(MoveAlongSpiral());
    }

    public void StartMovement()
    {
        isStopped = false;
        movementCoroutine = StartCoroutine(MoveAlongSpiral());
        colorCoroutine = StartCoroutine(ChangeColor());
    }

    private IEnumerator Fireworks()
    {
        scaleChange.Play();
        yield return new WaitForSeconds(0.5f);
        fireworks.Play();
    }
}
