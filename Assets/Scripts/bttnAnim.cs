
using UnityEngine;
using UnityEngine.UI;
public class bttnAnim : MonoBehaviour
{

    public Image[] buttons; // ������ ����������� ������
    public Sprite[] pushedSprites; // ������ �������� ��� �������������� ������
    public Sprite[] notPushedSprites; // ������ �������� ��� ����������� ������
    public Image shopImg;
    private bool[] isPushed; // ������ ��������� ������

    void Start()
    {
        // ������������� ������� ���������
        isPushed = new bool[buttons.Length];

        // ������ ������ ������ ��������
        isPushed[0] = true;
        buttons[0].sprite = pushedSprites[0];

        // ��������� ������ ���������
        for (int i = 1; i < isPushed.Length; i++)
        {
            isPushed[i] = false;
            buttons[i].sprite = notPushedSprites[i];
        }
    }

    public void OpenButton(int buttonIndex)
    {
        // �������������� ��������� ������
        isPushed[buttonIndex] = !isPushed[buttonIndex];

        // ��������� ������� � ����������� �� ��������� ������
        buttons[buttonIndex].sprite = isPushed[buttonIndex] ? pushedSprites[buttonIndex] : notPushedSprites[buttonIndex];

        // ����� ��������� ������ ������
        for (int i = 0; i < isPushed.Length; i++)
        {
            if (i != buttonIndex)
            {
                isPushed[i] = false;
                buttons[i].sprite = notPushedSprites[i];
            }
        }

        // ���� ������ ���� �������, ���������� ������ �������������� ������ ������
        if (!isPushed[buttonIndex])
        {
            isPushed[0] = true;
            buttons[0].sprite = pushedSprites[0];
        }
    }
}
