using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAfterStart : MonoBehaviour {
    
    [SerializeField] private List<GameObject> gos;
    [SerializeField] private float timer = 0.1f;
    private void Start() {
        StartCoroutine(EnableAfter());
    }

    private IEnumerator EnableAfter() {
        
        yield return new WaitForSeconds(timer);
        foreach (var go in gos) {
            go.SetActive(true);

        }
        
    }
    
}
