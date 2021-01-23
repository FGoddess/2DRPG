using System.Collections;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private static DialogManager _inctance;
    public static DialogManager Inctance
    {
        get
        {
            return _inctance;
        }
    }

    private void Awake()
    {
        _inctance = this;
    }

    [SerializeField] private string[] _sentences;
    [SerializeField] private float _textSpeed;
    [SerializeField] private TextMeshProUGUI _dialogText;
    [SerializeField] private GameObject _dialogBox;
    [SerializeField]
    public GameObject DialogBox
    {
        get { return _dialogBox; }
    }

    private int _index = 0;

    public void StartDialog(string[] sentences)
    {
        _dialogBox.SetActive(true);
        _dialogText.text = string.Empty;
        _sentences = sentences;
        StartCoroutine(DialogCour());
    }

    private IEnumerator DialogCour()
    {
        foreach (var ch in _sentences[_index].ToCharArray())
        {
            _dialogText.text += ch;
            yield return new WaitForSeconds(_textSpeed);
        }
    }

    public void NextSentence()
    {
        if (_index < _sentences.Length - 1)
        {
            _dialogText.text = string.Empty;
            _index++;
            StartCoroutine(DialogCour());
        }
        else
        {
            _index = 0;
            _dialogBox.SetActive(false);
        }
    }
}
