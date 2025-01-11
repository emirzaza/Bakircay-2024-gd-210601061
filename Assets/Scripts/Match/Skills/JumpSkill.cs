using System.Collections.Generic;
using UnityEngine;

namespace Match.Skills
{
    public class JumpSkill : MonoBehaviour
    {
        public float jumpForce = 5f; // Zıplama kuvveti

        [SerializeField] // Inspector'da görünmesi için
        private ItemSpawner itemSpawner;

        // Skill'i çalıştıran ana metot
        public void UseJumpSkill()
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
                Debug.LogWarning("No items found to jump!");
                return;
            }

            // Her item'e yukarı doğru zıplama kuvveti uygula
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

                // Zıplama kuvveti uygula
                rb.isKinematic = false; // Fizik etkisini aktif et
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
