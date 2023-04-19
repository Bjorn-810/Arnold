using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "AmmoCount", menuName = "Guns/ AmmoCount", order = 1)]
public class GunSO : ScriptableObject
{
    public GunType Type;
    public string Name;
    public GameObject ModelPrefab;
    public Sprite _GunIcon;

    public ShootConfigurationSO ShootConfiguration;
    public AmmoConfigSO AmmoConfig;
    public TrailConfigurationSO TrailConfiguration;
    public CameraShakeConfigurationSO CameraShakeConfiguration;
    public DamageConfigurationSO DamageConfiguration;

    private MonoBehaviour ActiveMonobeaviour;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem[] ShootSystem;
    private ParticleSystem TrailSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    private CinemachineCameraShake CameraShake;

    /// <summary>
    /// Creates a new gun with its own dedicated trailpool.
    /// </summary>
    /// <param name="Parent"></param>
    /// <param name="ActiveMonobeaviour"></param>
    public void Spawn(Transform Parent, MonoBehaviour ActiveMonobeaviour)
    {
        this.ActiveMonobeaviour = ActiveMonobeaviour;
        LastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        AmmoConfig = Instantiate(AmmoConfig); // Instantiate the GunSO scriptable object

        CameraShake = FindObjectOfType<CinemachineCameraShake>();

        if (Model != null) // Is here to make sure there arent mutliple guns being spawned;
            Destroy(Model);

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);

        ShootSystem = Model.GetComponentsInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Destroys the current weapon and clear the trailrenderer pool of bullets.
    /// </summary>
    public void Despawn()
    {
        this.ActiveMonobeaviour = null;
        LastShootTime = 0;

        TrailPool.Clear();

        ShootSystem = null;

        Destroy(Model);
    }

    public void Shoot()
    {
        // Check if we have any bullets left. If so start the shooting logic else we will auto reload the weapon
        if (AmmoConfig.CurrentClipAmmo > 0)
        {
            if (Time.time > ShootConfiguration.FireRate + LastShootTime)
            {
                CameraShake.ShakeCamera(CameraShakeConfiguration._intensity, CameraShakeConfiguration._frequency, CameraShakeConfiguration._shakeTime);
                AmmoConfig.CurrentClipAmmo--;

                // For every weapon we have attached we should shoot a bullet.
                for (int i = 0; i < ShootSystem.Length; i++)
                {
                    LastShootTime = Time.time;

                    // Adding bullet spread to the shooting direction
                    Vector3 shootdireciton = ShootSystem[i].transform.forward
                        + new Vector3(
                            Random.Range(
                                -ShootConfiguration.Spread.x,
                                ShootConfiguration.Spread.x),
                            Random.Range(
                                -ShootConfiguration.Spread.y,
                                ShootConfiguration.Spread.y),
                            Random.Range(
                                -ShootConfiguration.Spread.z,
                                ShootConfiguration.Spread.z
                                )
                            );

                    shootdireciton.Normalize();

                    // Getting the hit location so we can render a trail towards this point
                    if (Physics.Raycast(
                        ShootSystem[i].transform.position,
                        shootdireciton,
                        out RaycastHit hit,
                        float.MaxValue,
                        ShootConfiguration.Hitmask
                        ))
                    {
                        // Render trail
                        ActiveMonobeaviour.StartCoroutine(PlayTrail
                            (
                                ShootSystem[i].transform.position,
                                hit.point,
                                hit
                            )
                        );
                    }

                    // If we hit nothing render a trail towards the middle of the raycast.
                    else
                    {
                        ActiveMonobeaviour.StartCoroutine(
                            PlayTrail(
                                ShootSystem[i].transform.position,
                                ShootSystem[i].transform.position + (shootdireciton * TrailConfiguration.MissDistance),
                                new RaycastHit())
                            );
                    }

                    // In both instances we should start the particle system.
                    ShootSystem[i].Play();
                }
            }
        }

        // Auto reload weapon
        else
        {
            ActiveMonobeaviour.StartCoroutine(Reload());
        }
    }

    // Reload the weapon
    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(AmmoConfig.ReloadTime);
        AmmoConfig.Reload();
    }

    /// <summary>
    /// Creates a new trail using the TrailConfiguration config.
    /// </summary>
    /// <returns></returns>
    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfiguration.Color;
        trail.material = TrailConfiguration.Material;
        trail.widthCurve = TrailConfiguration.WidthCurve;
        trail.time = TrailConfiguration.Duration;
        trail.minVertexDistance = TrailConfiguration.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    /// <summary>
    /// Play the bullet trailrenderer by taking a trail from the trailpool and use it
    /// to display on screen. Afterwards we put it back into the trailrenderer array.
    /// </summary>
    /// <param name="StartPoint"></param>
    /// <param name="EndPoint"></param>
    /// <param name="Hit"></param>
    /// <returns></returns>
    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null;

        instance.emitting = true;
        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
                );
            remainingDistance -= TrailConfiguration.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        if (Hit.collider != null)
        {
            if (Hit.collider.gameObject.GetComponent<Health>())
            {
                Hit.collider.gameObject.GetComponent<Health>().DealDamage(DamageConfiguration.Damage);
            }
        }

        yield return new WaitForSeconds(TrailConfiguration.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);

        TrailPool.Release(instance);
    }
}
