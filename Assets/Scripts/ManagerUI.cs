using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    public static ManagerUI MUI;

    public Text textTurn;
    public Text textTimerWhite, textTimerBlack;
    public Text textCheck;

    public Text textWin;
    public GameObject buttonRestart;

    private void Awake()
    {
        MUI = this;
    }

    public void RestartMatch()
    {
        UpdateTimer(true, Settings.S.timePlayer);
        UpdateTimer(false, Settings.S.timePlayer);
        UpdateTurn(false);
        UpdateCheck(false);
        textWin.gameObject.SetActive(false);
        buttonRestart.SetActive(false);
    }

    public void UpdateTimer(bool _timerBlack, float _timer)
    {
        if (_timerBlack)
            textTimerBlack.text = Mathf.FloorToInt(_timer / 60).ToString("D2") + ":" + Mathf.FloorToInt(_timer % 60).ToString("D2");
        else
            textTimerWhite.text = Mathf.FloorToInt(_timer / 60).ToString("D2") + ":" + Mathf.FloorToInt(_timer % 60).ToString("D2");
    }

    public void UpdateTurn(bool _blackTurn)
    {
        textTurn.text = (_blackTurn ? "Black" : "White") + " Turn";
    }

    public void UpdateCheck(bool _check)
    {
        textCheck.gameObject.SetActive(_check);
    }

    public void EndGame(bool _blackTurn)
    {
        textWin.gameObject.SetActive(true);
        textWin.text = (_blackTurn ? "Black" : "White") + " Wins!";
        buttonRestart.SetActive(true);
    }
}
