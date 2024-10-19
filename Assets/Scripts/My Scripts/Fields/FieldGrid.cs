using UnityEngine;

public class FieldGrid : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if(collision != null)
        {
            if(collision.CompareTag("Pillar"))
            {
                Destroy(gameObject);
            }
        }
    }
}
