using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    private List<Collider> detectedObjects = new List<Collider>();
    private float detectionWaitTime = 3f; // Bekleme s�resi
    private bool isChecking = false;
    private Vector3 ejectDirection = new Vector3(0f, 0.5f, 1f).normalized; // Hafif e�imli ileri y�n
    private float ejectForce = 10f; // D��ar� atma kuvveti

    private void OnTriggerEnter(Collider other)
    {
        // Sadece Matchable tag'ine sahip nesneler i�lenir.
        if (IsMatchableTag(other.tag))
        {
            detectedObjects.Add(other);

            // E�er zaten kontrol eden bir coroutine yoksa ba�lat.
            if (!isChecking)
            {
                StartCoroutine(CheckForMatches());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Nesne tetikleme alan�ndan ��karsa listeden kald�r�l�r.
        if (detectedObjects.Contains(other))
        {
            detectedObjects.Remove(other);
        }
    }

    private IEnumerator CheckForMatches()
    {
        isChecking = true;

        // Bekleme s�resi boyunca ba�ka bir nesne alg�lan�rsa kontrol yap�l�r.
        yield return new WaitForSeconds(detectionWaitTime);

        // E�er en az iki nesne varsa, e�le�me kontrol� yap�l�r.
        if (detectedObjects.Count >= 2)
        {
            var firstObject = detectedObjects[0];
            var secondObject = detectedObjects[1];

            if (AreObjectsMatching(firstObject, secondObject))
            {
                // E�le�en nesneleri yok et.
                Destroy(firstObject.gameObject);
                Destroy(secondObject.gameObject);

                // Listeyi temizle.
                detectedObjects.Clear();
            }
            else
            {
                // E�er e�le�me yoksa, ikinci nesneyi d��ar� at.
                EjectObject(secondObject);
                detectedObjects.Remove(secondObject);
            }
        }

        isChecking = false;
    }

    private void EjectObject(Collider obj)
    {
        // D��ar� atmak i�in fizik kuvveti uygular.
        Rigidbody rb = obj.attachedRigidbody;
        if (rb != null)
        {
            rb.isKinematic = false; // Fizik etkile�imini a�
            rb.AddForce(ejectDirection * ejectForce, ForceMode.Impulse);
        }
        else
        {
            // E�er Rigidbody yoksa pozisyonu do�rudan g�ncelle.
            obj.transform.position += ejectDirection * ejectForce;
        }
    }

    private bool AreObjectsMatching(Collider first, Collider second)
    {
        // Tag'lerin e�le�mesi durumunda true d�ner.
        return first.tag == second.tag;
    }

    private bool IsMatchableTag(string tag)
    {
        // Sadece belirli tag'ler i�lenir.
        return tag == "box" || tag == "sphere" || tag == "cylinder";
    }
}