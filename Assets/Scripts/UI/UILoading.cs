using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [SerializeField] Canvas loadingCanvas;
    [SerializeField] Image background;
    [SerializeField] Text loadPercentField;
    [SerializeField] Text loadTextField;

    [SerializeField] float delayBeforeClosing = 1f;

    public void UILoadEnable(bool enabled) {
        if (enabled) {
            UIOpen();
        }
        else {
            SetLoadPercentage(100);
            Invoke("UIClose", delayBeforeClosing);
        }
    }

    public void SetLoadPercentage(int percent) { //TODO: �������� ����� ����� ���� � ��������
        if (percent >= 0 && percent <= 100) {
            loadPercentField.text = "!";
        }
    }
    public void SetLoadText() { //TODO: �� �������� ����� ����� ���� � ��������

    }

    private void UIOpen()
    {
        loadingCanvas.enabled = true;
    }
    private void UIClose()
    {
        SetLoadPercentage(0);
        loadingCanvas.enabled = false;
    }
}
