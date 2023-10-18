using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image _greenBarImage;
    [SerializeField] private Image _redBarImage;
    public void UpdateHealthBar(float _maxHealth, float _health, float _previousLifePoints)
    {
        _greenBarImage.fillAmount = (float) _health/_maxHealth;

        float _targetHealth = _health/_maxHealth;
        _previousLifePoints = _previousLifePoints / _maxHealth;
        StartCoroutine(HealthBarAnimation(_targetHealth, _previousLifePoints));

    }

    IEnumerator HealthBarAnimation(float _targetHealth, float _previousLifePoints)
    {
        float _transitionTime = 0.5f;
        float _elapsedTime = 0f;

        while (_elapsedTime < _transitionTime)
        {
            _elapsedTime += Time.deltaTime;
            _redBarImage.fillAmount = Mathf.Lerp(_previousLifePoints, _targetHealth, _elapsedTime / _transitionTime);
            yield return null;
            _redBarImage.fillAmount = _targetHealth;
        }
    }
}
