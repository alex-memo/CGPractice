using System.Collections;
using UnityEngine;

public class GameManager : InstanceFactory<GameManager>
{
    public OnDealDamage OnDealDamage;
    public OnGameOver OnGameOver;
    [SerializeField] private Material screenLutMat;
    [SerializeField] private Material screenVignetteMat;
    private Coroutine lutCoroutine;
    public int Hp { get; private set; } = 100;

    protected override void Awake()
    {
        base.Awake();
        resetMats();
        OnDealDamage += triggerLut;
        OnDealDamage += evaluateVignette;
    }

    private void Update()
    {
        if (Hp <= 0)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Hp -= 10;
            OnDealDamage?.Invoke();
            if (Hp <= 0)
            {
                OnGameOver?.Invoke(false);
            }
        }
    }

    private void evaluateVignette()
    {
        screenVignetteMat.SetFloat("_Alpha", 1 - (Hp / 100f));
    }

    private void triggerLut()
    {
        lutCoroutine ??= StartCoroutine(flashLut());
    }

    private IEnumerator flashLut()
    {
        float _timer = 0;
        const float _duration = 0.1f;
        while (_timer < _duration)
        {
            _timer += Time.deltaTime;
            yield return null;
            screenLutMat.SetFloat("_Contribution", _timer / _duration);
        }

        _timer = _duration;
        //now the other way around, it sucks, but its to do quick
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            yield return null;
            screenLutMat.SetFloat("_Contribution", _timer / _duration);
        }

        //screenLutMat.SetFloat("_Alpha", 1);
        //yield return new WaitForSeconds(0.1f);
        lutCoroutine = null;
    }

    private void resetMats()
    {
        screenLutMat.SetFloat("_Contribution", 0);
        screenVignetteMat.SetFloat("_Alpha", 0);
    }

    private void OnDestroy()
    {
        OnDealDamage -= triggerLut;
        OnDealDamage -= evaluateVignette;
        resetMats();
        lutCoroutine = null;
    }
}

public delegate void OnDealDamage();

public delegate void OnGameOver(bool _win);