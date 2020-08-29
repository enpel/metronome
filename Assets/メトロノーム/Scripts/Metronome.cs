using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    public AudioSource タップ音;
    public Text BPMの表示;
    public Slider BPMスライダー;
    public Button スタートボタン;

    private BoolReactiveProperty 再生中 = new BoolReactiveProperty(false);
    private double 経過時間 = 0;
    private TimeSpan 鳴らすべき間隔 = TimeSpan.Zero;

    // Start is called before the first frame update
    void Start()
    {
        スタートボタン.OnClickAsObservable()
            .Subscribe(_=>再生中.Value = !再生中.Value)
            .AddTo(this);
        BPMスライダー.minValue = 1;
        BPMスライダー.maxValue = 200;
        BPMスライダー.value = 100;

        BPMスライダー.OnValueChangedAsObservable()
            .Select(x => Mathf.FloorToInt(x))
            .Subscribe(bpm =>
            {
                鳴らすべき間隔 = TimeSpan.FromSeconds(60.0f/bpm);
                BPMの表示.text = bpm.ToString() + "BPM";
            })
            .AddTo(this);
    }

    private void Update()
    {
        if (!再生中.Value)
        {
            return;
        }


        経過時間 += Time.deltaTime;

        bool 音を鳴らす = (経過時間 > 鳴らすべき間隔.TotalSeconds);

        if (音を鳴らす)
        {
            タップ音.PlayOneShot(タップ音.clip);
            経過時間 -= 鳴らすべき間隔.TotalSeconds;
        }

    }
}
