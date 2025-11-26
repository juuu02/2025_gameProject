using UnityEngine;

public class Hit_check : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 대신 키보드의 K 키를 눌러 테스트
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Shoot Function Called by K key"); // 메시지도 변경
            Shoot();
        }
    }

    void Shoot()
    {
        int layerMask_LeftLeg = LayerMask.GetMask("LeftLeg");
        int layerMask_Head = LayerMask.GetMask("Head");

        int combinedLayerMask = layerMask_LeftLeg | layerMask_Head;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 500f, combinedLayerMask))
        {
            Debug.Log("Shoot Function Called");
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Head"))
            {
                Debug.Log("머리 피격! (높은 데미지 적용)");
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("LeftLeg"))
            {
                Debug.Log("왼쪽 다리 피격! (일반 데미지 적용)");
            }

            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
    }
}
