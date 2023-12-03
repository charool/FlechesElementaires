using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    bool didHit = false;
    bool isShoot = false;
    [SerializeField]
    float destroyTime;
    [SerializeField]
    float speed;
    [SerializeField]
    float maxTime;
    [SerializeField]
    float time=0f;

    [SerializeField]
    ArrowType type = ArrowType.Clasique;
    private void Update()
    {
        if (isShoot && !didHit)
        {
            time += Time.deltaTime;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if(time > maxTime)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Transform spawn)
    {
        transform.SetParent(null);
        transform.rotation = spawn.rotation;
        transform.position = spawn.position;
        isShoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("hit");

        if (!isShoot || didHit) { return; }
        if(other.tag == "arrow") { return; }
        didHit = true;
        transform.SetParent(other.transform,true);
        IHitable target = other.GetComponent<IHitable>();
        if (target != null && other.gameObject != gameObject)
        {
            target.Hit(transform.forward, type);
        }
        StartCoroutine(DestroyAftertime());
    }
    private IEnumerator DestroyAftertime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
public enum ArrowType
{
    None,Clasique,Dirt,Wind,Elec,Ice,Fire
}
public enum Element
{
    Dirt, Wind, Elec, Ice, Fire
}