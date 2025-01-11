using System.Collections.Generic;
using UnityEngine;

namespace Match.Skills
{
    public class BombSkill : MonoBehaviour
    {
        public float pushForce = 10f; // İtme kuvveti

        [SerializeField] // Inspector'da görünmesi için
        private ItemSpawner itemSpawner;

        public void UseBombSkill()
        {
            if (itemSpawner == null)
            {
                Debug.LogError("ItemSpawner is not assigned!");
                return;
            }

            // Sahnedeki tüm aktif item'leri al
            List<Item> items = itemSpawner.GetItems();
            if (items == null || items.Count == 0)
            {
                Debug.LogWarning("No items found to push!");
                return;
            }

            Vector3 centerPosition = itemSpawner.transform.position; // Kuvvetin merkez noktası

            // Her item'e merkezden dışa doğru kuvvet uygula
            foreach (var item in items)
            {
                if (item == null || item.transform == null)
                    continue;

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    Debug.LogWarning($"Item {item.name} has no Rigidbody!");
                    continue;
                }

                // Merkezden dışa doğru yön hesapla
                Vector3 direction = (item.transform.position - centerPosition).normalized;

                // Kuvvet uygula
                rb.isKinematic = false; // Fizik etkisini aktif et
                rb.AddForce(direction * pushForce, ForceMode.Impulse);
            }
        }
    }
}