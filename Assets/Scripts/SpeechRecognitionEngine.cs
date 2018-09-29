using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRecognitionEngine : MonoBehaviour
{
    public string[] keywords = new string[] { "on", "off", "light", "speaker", "light on", "light off", "speaker on", "speaker off" };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float speed = 1;

    public Text results;
    public Image target;

    protected PhraseRecognizer recognizer;
    protected string word = "light off";

    private void Start()
    {
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        results.text = "You said: <b>" + word + "</b>";
    }

    private void Update()
    {
        var x = target.transform.position.x;
        var y = target.transform.position.y;

        switch (word)
        {
			case "on":
				break;
			case "off":
				break;
			case "light":
				break;
			case "speaker":
				break;
			case "light on":
                x -= speed;
				File.WriteAllText("Assets/Scripts/action.txt", "LIGHT ON");
				word = "";
				break;
            case "light off":
                x += speed;
				File.WriteAllText("Assets/Scripts/action.txt", "LIGHT OFF");
				word = "";
				break;
			case "speaker on":
				y -= speed;
				File.WriteAllText("Assets/Scripts/action.txt", "SPEAKER ON");
				word = "";
				break;
			case "speaker off":
				y += speed;
				File.WriteAllText("Assets/Scripts/action.txt", "SPEAKER OFF");
				word = "";
				break;
			default:
				break;
        }

        target.transform.position = new Vector3(x, y, 0);
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
