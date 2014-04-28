using UnityEngine;
using System.Collections;

public class OpenTwitterPage : MonoBehaviour {
    public void OnClick(dfControl control, dfMouseEventArgs mouseEvent) {
        Application.OpenURL("http://www.twitter.com/oatsbarley");
    }
}